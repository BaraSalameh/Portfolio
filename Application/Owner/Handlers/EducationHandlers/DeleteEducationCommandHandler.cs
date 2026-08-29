using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class DeleteEducationCommandHandler : IRequestHandler<DeleteEducationCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteEducationCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.Education
                .Include(e => e.LstUserSkillEducations)
                .FirstOrDefaultAsync(x =>
                    x.UserID == _currentUser.UserID!.Value &&
                    x.ID == request.ID &&
                    x.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                response.lstError.Add("Education not found.");
                return response;
            }

            existingEntity.IsDeleted = true;
            existingEntity.LstUserSkillEducations.Clear();
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
