using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using Application.Owner.Queries.ExperienceQueries;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class AddEditExperienceCommandHandler : IRequestHandler<AddEditExperienceCommand, CommandResponse<ELQ_Response>>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserSkillRelationService _userSkillRelation;

        public AddEditExperienceCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper, IUserSkillRelationService userSkillRelation)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
            _userSkillRelation = userSkillRelation;
        }

        public async Task<CommandResponse<ELQ_Response>> Handle(AddEditExperienceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse<ELQ_Response>();
            var userId = _currentUser.UserID;
            var isEdit = request.ID.HasValue;
            Experience savedEntity;

            if (request.EndDate.HasValue && request.EndDate < request.StartDate)
            {
                response.lstError.Add("EndDate cannot be earlier than StartDate.");
                return response;
            }

            if (!await _userSkillRelation.AreValidSkillIdsAsync(request.LstSkills ?? [], cancellationToken))
            {
                response.lstError.Add("One or more skills are invalid.");
                return response;
            }

            if (isEdit)
            {
                var existingEntity = await _context.Experience
                    .Include(c => c.LstUserSkillExperiences)
                    .ThenInclude(usc => usc.UserSkill)
                    .FirstOrDefaultAsync(x =>
                        x.UserID == userId &&
                        x.ID == request.ID &&
                        x.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("Experience not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
                savedEntity = existingEntity;
                await _userSkillRelation.UpdateUserSkillRelationsAsync<Experience, UserSkillExperience>(
                    existingEntity,
                    request.LstSkills ?? [],
                    userId!.Value,
                    c => c.LstUserSkillExperiences,
                    usc => usc.UserSkill,
                    (usc, us) => usc.UserSkill = us,
                    usc => usc.UserSkill.LKP_SkillID,
                    (skillId, userId) => new UserSkillExperience { ExperienceID = existingEntity.ID },
                    cancellationToken
                );
            }
            else
            {
                var newEntity = _mapper.Map<Experience>(request);
                newEntity.UserID = userId!.Value;
                savedEntity = newEntity;

                if (request.LstSkills != null && request.LstSkills.Any())
                {
                    newEntity.LstUserSkillExperiences = await _userSkillRelation.CreateUserSkillRelationsAsync<UserSkillExperience>(
                        request.LstSkills,
                        userId!.Value,
                        newEntity.ID,
                        us => us.LstExperiences,
                        usc => usc.ExperienceID,
                        (usc, id) => usc.ExperienceID = id,
                        cancellationToken
                    );
                }

                await _context.Experience.AddAsync(newEntity, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            response.Data = await _mapper.ProjectTo<ELQ_Response>(_context.Experience
                .AsNoTracking()
                .Where(entity => entity.ID == savedEntity.ID && entity.UserID == userId && !entity.IsDeleted))
                .SingleAsync(cancellationToken);

            return response;
        }
    }
}
