using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.BlogPostCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.BlogPostHandlers
{
    public class DeleteBlogPostCommandHandler: IRequestHandler<DeleteBlogPostCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteBlogPostCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var existingEntity = await _context.BlogPost
                    .FirstOrDefaultAsync(x =>
                        x.UserID == _currentUser.UserID!.Value &&
                        x.ID == request.ID &&
                        x.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("BlogPost not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the BlogPost.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred");
            }

            return response;
        }
    }
}
