using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class DeleteEducationCommandHandler: IRequestHandler<DeleteEducationCommand, CommandResponse>
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

            try
            {
                var existingEntity = await _context.Education
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
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the Education.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
