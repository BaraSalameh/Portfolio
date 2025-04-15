using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.BlogPostCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.BlogPostHandlers
{
    public class DeleteBlogPostCommandHandler: IRequestHandler<DeleteBlogPostCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteBlogPostCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteBlogPostCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var BlogPostToDelete =
                await _context.BlogPost
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync(cancellationToken);

            if (BlogPostToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("BlogPost not found");
                return Vm;
            }

            try
            {
                BlogPostToDelete.IsDeleted = true;
                BlogPostToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the BlogPost");
            }

            return Vm;
        }
    }
}
