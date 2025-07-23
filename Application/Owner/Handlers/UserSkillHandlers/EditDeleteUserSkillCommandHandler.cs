using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserSkillCommands;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserSkillHandlers
{
    public class EditDeleteUserSkillCommandHandler : IRequestHandler<EditDeleteUserSkillCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EditDeleteUserSkillCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(EditDeleteUserSkillCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.LstUserSkills == null)
            {
                response.lstError.Add("Skill list can't be null.");
                return response;
            }

            try
            {

                var existingEntity = await _context.User
                    .Include(y => y.LstUserSkills)
                    .FirstOrDefaultAsync(u => u.ID == _currentUser.UserID!.Value, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("User not found.");
                    return response;
                }

                var RequestedSkills = request.LstUserSkills.Select(x => x.LKP_SkillID).ToList();

                var LKP_SkillIDs = await _context.LKP_Skill
                    .AsNoTracking()
                    .Where(l => RequestedSkills.Contains(l.ID))
                    .Select(l => l.ID)
                    .ToListAsync(cancellationToken);

                if (RequestedSkills.Count != LKP_SkillIDs.Count)
                {
                    response.lstError.Add("Wrong Skills Entry.");
                    return response;
                }

                _mapper.Map(request, existingEntity);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while editting/deleting the Skill.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred");
            }

            return response;
        }
    }
}
