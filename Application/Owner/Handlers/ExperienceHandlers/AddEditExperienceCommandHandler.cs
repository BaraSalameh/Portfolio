using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class AddEditExperienceCommandHandler : IRequestHandler<AddEditExperienceCommand, CommandResponse>
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

        public async Task<CommandResponse> Handle(AddEditExperienceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            var userId = _currentUser.UserID;
            var isEdit = request.ID.HasValue;

            try
            {
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
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                    await _userSkillRelation.UpdateUserSkillRelationsAsync<Experience, UserSkillExperience>(
                        existingEntity,
                        request.LstSkills,
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
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the Experience.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
