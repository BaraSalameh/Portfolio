using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectCommands;
using AutoMapper;
using Application.Common.Persistence;
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
        private readonly IUserSkillRelationService _userSkillRelation;

        public AddEditProjectCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper, IUserSkillRelationService userSkillRelation)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
            _userSkillRelation = userSkillRelation;
        }

        public async Task<CommandResponse> Handle(AddEditProjectCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            var userId = _currentUser.UserID;
            var isEdit = request.ID.HasValue;

            if (!await _userSkillRelation.AreValidSkillIdsAsync(request.LstSkills ?? [], cancellationToken))
            {
                response.lstError.Add("One or more skills are invalid.");
                return response;
            }

            if (request.EducationID.HasValue && !await _context.Education.AnyAsync(
                    entity => entity.ID == request.EducationID && entity.UserID == userId,
                    cancellationToken))
            {
                response.lstError.Add("Education must belong to the current user.");
                return response;
            }

            if (request.ExperienceID.HasValue && !await _context.Experience.AnyAsync(
                    entity => entity.ID == request.ExperienceID && entity.UserID == userId,
                    cancellationToken))
            {
                response.lstError.Add("Experience must belong to the current user.");
                return response;
            }

            if (isEdit)
            {
                var existingEntity = await _context.Project
                    .Include(c => c.LstUserSkillProjects)
                    .ThenInclude(usc => usc.UserSkill)
                    .FirstOrDefaultAsync(x =>
                        x.UserID == userId &&
                        x.ID == request.ID &&
                        x.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("Project not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
                await _userSkillRelation.UpdateUserSkillRelationsAsync<Project, UserSkillProject>(
                    existingEntity,
                    request.LstSkills ?? [],
                    userId!.Value,
                    c => c.LstUserSkillProjects,
                    usc => usc.UserSkill,
                    (usc, us) => usc.UserSkill = us,
                    usc => usc.UserSkill.LKP_SkillID,
                    (skillId, userId) => new UserSkillProject { ProjectID = existingEntity.ID },
                    cancellationToken
                );
            }
            else
            {
                var newEntity = _mapper.Map<Project>(request);
                newEntity.UserID = userId!.Value;

                if (request.LstSkills != null && request.LstSkills.Any())
                {
                    newEntity.LstUserSkillProjects = await _userSkillRelation.CreateUserSkillRelationsAsync<UserSkillProject>(
                        request.LstSkills,
                        userId!.Value,
                        newEntity.ID,
                        us => us.LstProjects,
                        usc => usc.ProjectID,
                        (usc, id) => usc.ProjectID = id,
                        cancellationToken
                    );
                }

                await _context.Project.AddAsync(newEntity, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
