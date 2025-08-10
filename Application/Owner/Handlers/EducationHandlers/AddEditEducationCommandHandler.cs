using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class AddEditEducationCommandHandler : IRequestHandler<AddEditEducationCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserSkillRelationService _userSkillRelation;

        public AddEditEducationCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper, IUserSkillRelationService userSkillRelation)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
            _userSkillRelation = userSkillRelation;
        }

        public async Task<CommandResponse> Handle(AddEditEducationCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            var userId = _currentUser.UserID;
            var isEdit = request.ID.HasValue;

            try
            {
                if (isEdit)
                {
                    var existingEntity = await _context.Education
                        .Include(c => c.LstUserSkillEducations)
                        .ThenInclude(usc => usc.UserSkill)
                        .FirstOrDefaultAsync(x =>
                            x.UserID == userId &&
                            x.ID == request.ID &&
                            x.IsDeleted == false,
                            cancellationToken
                        );

                    if (existingEntity == null)
                    {
                        response.lstError.Add("Education not found.");
                        return response;
                    }

                    _mapper.Map(request, existingEntity);
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                    await _userSkillRelation.UpdateUserSkillRelationsAsync<Education, UserSkillEducation>(
                        existingEntity,
                        request.LstSkills,
                        userId!.Value,
                        c => c.LstUserSkillEducations,
                        usc => usc.UserSkill,
                        (usc, us) => usc.UserSkill = us,
                        usc => usc.UserSkill.LKP_SkillID,
                        (skillId, userId) => new UserSkillEducation { EducationID = existingEntity.ID },
                        cancellationToken
                    );
                }
                else
                {
                    var newEntity = _mapper.Map<Education>(request);
                    newEntity.UserID = userId!.Value;

                    if (request.LstSkills != null && request.LstSkills.Any())
                    {
                        newEntity.LstUserSkillEducations = await _userSkillRelation.CreateUserSkillRelationsAsync<UserSkillEducation>(
                            request.LstSkills,
                            userId!.Value,
                            newEntity.ID,
                            us => us.LstEducations,
                            usc => usc.EducationID,
                            (usc, id) => usc.EducationID = id,
                            cancellationToken
                        );
                    }

                    await _context.Education.AddAsync(newEntity, cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the Education.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
