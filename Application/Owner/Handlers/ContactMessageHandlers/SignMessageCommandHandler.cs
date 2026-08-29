using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ContactMessageCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ContactMessage
{
    public class SignMessageCommandHandler : IRequestHandler<SignMessageCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;
        public SignMessageCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<CommandResponse> Handle(SignMessageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.ContactMessage
                .FirstOrDefaultAsync(m =>
                    m.UserID == _currentUserService.UserID!.Value &&
                    m.ID == request.ID &&
                    m.IsRead == false &&
                    m.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                response.lstError.Add("Message not found.");
                return response;
            }

            existingEntity.IsRead = true;

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
