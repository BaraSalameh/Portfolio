using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers;

public class AddEditCertificateCommandHandler : IRequestHandler<AddEditCertificateCommand, CommandResponse>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;
    private readonly IUserSkillRelationService _userSkillRelation;

    public AddEditCertificateCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper, IUserSkillRelationService userSkillRelation)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
        _userSkillRelation = userSkillRelation;
    }

    public async Task<CommandResponse> Handle(AddEditCertificateCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse();
        var userId = _currentUser.UserID;
        var isEdit = request.ID.HasValue;

        if (request.ExpirationDate.HasValue && request.IssueDate.HasValue && request.ExpirationDate < request.IssueDate)
        {
            response.lstError.Add("ExpirationDate cannot be earlier than IssueDate.");
            return response;
        }

        var requestedMedia = request.LstCertificateMedias ?? [];
        if (requestedMedia.Any(media => media is null))
        {
            response.lstError.Add("Certificate media URLs must not be null.");
            return response;
        }

        var mediaUrls = requestedMedia
            .Select(media => media.Trim())
            .ToArray();
        if (mediaUrls.Distinct(StringComparer.Ordinal).Count() != mediaUrls.Length)
        {
            response.lstError.Add("Duplicate certificate media URLs are not allowed.");
            return response;
        }

        if (mediaUrls.Any(media =>
                media.Length > 2048 ||
                !Application.Common.Validation.HttpUrlAttribute.IsValidHttpUrl(media)))
        {
            response.lstError.Add("Certificate media URLs must be valid HTTP or HTTPS URLs without embedded credentials and up to 2048 characters.");
            return response;
        }

        if (!await _context.LKP_Certificate.AnyAsync(
                entity => entity.ID == request.LKP_CertificateID,
                cancellationToken))
        {
            response.lstError.Add("Certificate type is invalid.");
            return response;
        }

        if (!await _userSkillRelation.AreValidSkillIdsAsync(request.LstSkills ?? [], cancellationToken))
        {
            response.lstError.Add("One or more skills are invalid.");
            return response;
        }

        if (isEdit)
        {
            var existingEntity = await _context.Certificate
                .Include(c => c.LstCertificateMedias)
                .Include(c => c.LstUserSkillCertificates)
                .ThenInclude(usc => usc.UserSkill)
                .AsSplitQuery()
                .FirstOrDefaultAsync(x =>
                    x.UserID == userId &&
                    x.ID == request.ID &&
                    x.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                response.lstError.Add("Certificate not found.");
                return response;
            }

            _mapper.Map(request, existingEntity);
            ReconcileMedia(existingEntity, mediaUrls);
            await _userSkillRelation.UpdateUserSkillRelationsAsync<Certificate, UserSkillCertificate>(
                existingEntity,
                request.LstSkills ?? [],
                userId!.Value,
                c => c.LstUserSkillCertificates,
                usc => usc.UserSkill,
                (usc, us) => usc.UserSkill = us,
                usc => usc.UserSkill.LKP_SkillID,
                (skillId, userId) => new UserSkillCertificate { CertificateID = existingEntity.ID },
                cancellationToken
            );
        }
        else
        {
            var newEntity = _mapper.Map<Certificate>(request);
            newEntity.UserID = userId!.Value;
            newEntity.LstCertificateMedias = CreateMedia(mediaUrls);

            if (request.LstSkills != null && request.LstSkills.Any())
            {
                newEntity.LstUserSkillCertificates = await _userSkillRelation.CreateUserSkillRelationsAsync<UserSkillCertificate>(
                    request.LstSkills,
                    userId!.Value,
                    newEntity.ID,
                    us => us.LstCertificates,
                    usc => usc.CertificateID,
                    (usc, id) => usc.CertificateID = id,
                    cancellationToken
                );
            }

            await _context.Certificate.AddAsync(newEntity, cancellationToken);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return response;
    }

    private static List<CertificateMedia> CreateMedia(IEnumerable<string> urls) =>
        urls.Select(url => new CertificateMedia { Url = url }).ToList();

    private static void ReconcileMedia(Certificate certificate, IReadOnlyCollection<string> requestedUrls)
    {
        var requested = requestedUrls.ToHashSet(StringComparer.Ordinal);
        var retainedUrls = new HashSet<string>(StringComparer.Ordinal);

        foreach (var existingMedia in certificate.LstCertificateMedias)
        {
            if (!requested.Contains(existingMedia.Url) || !retainedUrls.Add(existingMedia.Url))
            {
                existingMedia.IsDeleted = true;
            }
        }

        certificate.LstCertificateMedias.AddRange(requestedUrls
            .Where(url => !retainedUrls.Contains(url))
            .Select(url => new CertificateMedia { Url = url }));
    }
}
