using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectHandlers
{
    public class AddEditProjectCommandHandler : IRequestHandler<AddEditProjectCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditProjectCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditProjectCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            var userId = _currentUser.UserID;
            var isEdit = request.ID.HasValue;

            if (isEdit)
            {
                var existingEntity = await _context.Project
                    .Include(c => c.LstUserSkills)
                    .FirstOrDefaultAsync(c => c.ID == request.ID && c.UserID == userId, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("Project not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
                await UpdateProjectSkillsAsync(existingEntity, request.LstSkills, userId!.Value, cancellationToken);
            }
            else
            {
                var newEntity = _mapper.Map<Project>(request);
                newEntity.UserID = userId!.Value;
                newEntity.LstUserSkills = await CreateUserSkillsAsync(request.LstSkills, userId!.Value, newEntity.ID, cancellationToken);

                _context.Project.Add(newEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return response;
        }

        private async Task UpdateProjectSkillsAsync(Project proj, List<Guid> newSkillIds, Guid userId, CancellationToken cancellationToken)
        {
            var existingSkillIds = proj.LstUserSkills
                .Where(us => us.ProjectID == proj.ID)
                .Select(us => us.LKP_SkillID)
                .ToHashSet();

            var newSkillIdSet = newSkillIds.ToHashSet();

            // Skills to detach (set CertificateID = null)
            var toDetach = proj.LstUserSkills
                .Where(us => !newSkillIdSet.Contains(us.LKP_SkillID))
                .ToList();

            foreach (var us in toDetach)
            {
                us.ProjectID = null;
            }

            // Skills to add (reattach or create)
            var toAdd = newSkillIds.Except(existingSkillIds);

            foreach (var skillId in toAdd)
            {
                var existingDetached = await _context.UserSkill.FirstOrDefaultAsync(us =>
                    us.UserID == userId &&
                    us.LKP_SkillID == skillId &&
                    us.ProjectID == null,
                    cancellationToken);

                if (existingDetached != null)
                {
                    existingDetached.ProjectID = proj.ID;
                }
                else
                {
                    proj.LstUserSkills.Add(new UserSkill
                    {
                        UserID = userId,
                        LKP_SkillID = skillId,
                        ProjectID = proj.ID
                    });
                }
            }
        }

        private async Task<List<UserSkill>> CreateUserSkillsAsync(List<Guid> skillIds, Guid userId, Guid projectId, CancellationToken cancellationToken)
        {
            var userSkills = new List<UserSkill>();

            foreach (var skillId in skillIds)
            {
                var existingDetached = await _context.UserSkill.FirstOrDefaultAsync(us =>
                    us.UserID == userId &&
                    us.LKP_SkillID == skillId &&
                    us.ProjectID == null,
                    cancellationToken);

                if (existingDetached != null)
                {
                    existingDetached.ProjectID = projectId;
                    userSkills.Add(existingDetached);
                }
                else
                {
                    userSkills.Add(new UserSkill
                    {
                        UserID = userId,
                        LKP_SkillID = skillId,
                        ProjectID = projectId
                    });
                }
            }

            return userSkills;
        }
    }
}
