using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ContactMessageCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ContactMessageHandlers
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;

        public DeleteMessageCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = context;

        }

        public async Task<CommandResponse> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var existingEntity = await _context.ContactMessage
                    .FirstOrDefaultAsync(m =>
                        m.UserID == _currentUserService.UserID!.Value &&
                        m.ID == request.ID &&
                        m.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("Message not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
            

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the Message.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
