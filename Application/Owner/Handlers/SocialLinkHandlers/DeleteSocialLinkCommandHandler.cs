using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SocialLinkCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SocialLinkHandlers
{
    public class DeleteSocialLinkCommandHandler: IRequestHandler<DeleteSocialLinkCommand, CommandResponse>
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

            try
            {
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
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the SocialLink.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
