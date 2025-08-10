using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using AutoMapper;
using DataAccess.Interfaces;
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

        try
        {
            if (isEdit)
            {
                var existingEntity = await _context.Certificate
                    .Include(c => c.LstUserSkillCertificates)
                    .ThenInclude(usc => usc.UserSkill)
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
                existingEntity.UpdatedAt = DateTime.UtcNow;
                await _userSkillRelation.UpdateUserSkillRelationsAsync<Certificate, UserSkillCertificate>(
                    existingEntity,
                    request.LstSkills,
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
        }
        catch (DbUpdateException dbEx)
        {
            response.lstError.Add("Error while adding/updating the Certificate.");
        }
        catch (Exception ex)
        {
            response.lstError.Add("Unexpected error occurred.");
        }

        return response;
    }
}
