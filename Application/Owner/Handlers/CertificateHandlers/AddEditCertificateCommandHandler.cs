using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers;
public class AddEditDeleteCertificateCommandHandler : IRequestHandler<AddEditCertificateCommand, CommandResponse>
{
    private readonly ICurrentUserService _currentUser;
    private readonly IAppDbContext _context;
    private readonly IMapper _mapper;

    public AddEditDeleteCertificateCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
    {
        _context = context;
        _currentUser = currentUser;
        _mapper = mapper;
    }

    public async Task<CommandResponse> Handle(AddEditCertificateCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse();
        var userId = _currentUser.UserID;
        var isEdit = request.ID.HasValue;

        if (isEdit)
        {
            var existingEntity = await _context.Certificate
                .Include(c => c.LstUserSkills)
                .FirstOrDefaultAsync(c => c.ID == request.ID && c.UserID == userId, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("Certificate not found.");
                return response;
            }

            _mapper.Map(request, existingEntity);
            await UpdateCertificateSkillsAsync(existingEntity, request.LstSkills, userId!.Value, cancellationToken);
        }
        else
        {
            var newEntity = _mapper.Map<Certificate>(request);
            newEntity.UserID = userId!.Value;
            newEntity.LstUserSkills = await CreateUserSkillsAsync(request.LstSkills, userId!.Value, newEntity.ID, cancellationToken);

            _context.Certificate.Add(newEntity);
        }

        await _context.SaveChangesAsync(cancellationToken);
        return response;
    }

    private async Task UpdateCertificateSkillsAsync(Certificate cert, List<Guid> newSkillIds, Guid userId, CancellationToken cancellationToken)
    {
        var existingSkillIds = cert.LstUserSkills
            .Where(us => us.CertificateID == cert.ID)
            .Select(us => us.LKP_SkillID)
            .ToHashSet();

        var newSkillIdSet = newSkillIds.ToHashSet();

        // Skills to detach (set CertificateID = null)
        var toDetach = cert.LstUserSkills
            .Where(us => !newSkillIdSet.Contains(us.LKP_SkillID))
            .ToList();

        foreach (var us in toDetach)
        {
            us.CertificateID = null;
        }

        // Skills to add (reattach or create)
        var toAdd = newSkillIds.Except(existingSkillIds);

        foreach (var skillId in toAdd)
        {
            var existingDetached = await _context.UserSkill.FirstOrDefaultAsync(us =>
                us.UserID == userId &&
                us.LKP_SkillID == skillId &&
                us.CertificateID == null,
                cancellationToken);

            if (existingDetached != null)
            {
                existingDetached.CertificateID = cert.ID;
            }
            else
            {
                cert.LstUserSkills.Add(new UserSkill
                {
                    UserID = userId,
                    LKP_SkillID = skillId,
                    CertificateID = cert.ID
                });
            }
        }
    }

    private async Task<List<UserSkill>> CreateUserSkillsAsync(List<Guid> skillIds, Guid userId, Guid certificateId, CancellationToken cancellationToken)
    {
        var userSkills = new List<UserSkill>();

        foreach (var skillId in skillIds)
        {
            var existingDetached = await _context.UserSkill.FirstOrDefaultAsync(us =>
                us.UserID == userId &&
                us.LKP_SkillID == skillId &&
                us.CertificateID == null,
                cancellationToken);

            if (existingDetached != null)
            {
                existingDetached.CertificateID = certificateId;
                userSkills.Add(existingDetached);
            }
            else
            {
                userSkills.Add(new UserSkill
                {
                    UserID = userId,
                    LKP_SkillID = skillId,
                    CertificateID = certificateId
                });
            }
        }

        return userSkills;
    }
}
