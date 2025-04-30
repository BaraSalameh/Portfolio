using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class DeleteExperienceCommandHandler: IRequestHandler<DeleteExperienceCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteExperienceCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var existingEntity = await _context.Experience
                    .FirstOrDefaultAsync(x =>
                        x.UserID == _currentUser.UserID!.Value &&
                        x.ID == request.ID &&
                        x.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("Experience not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the Experience.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
