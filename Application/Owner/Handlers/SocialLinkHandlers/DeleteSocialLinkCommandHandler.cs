using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SocialLinkCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SocialLinkHandlers
{
    public class DeleteSocialLinkCommandHandler : IRequestHandler<DeleteSocialLinkCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteSocialLinkCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteSocialLinkCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.SocialLink
                .FirstOrDefaultAsync(x =>
                    x.UserID == _currentUser.UserID!.Value &&
                    x.ID == request.ID &&
                    x.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                response.lstError.Add("SocialLink not found.");
                return response;
            }

            existingEntity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
