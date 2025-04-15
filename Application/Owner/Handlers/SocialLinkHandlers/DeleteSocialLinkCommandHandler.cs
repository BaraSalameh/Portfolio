using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SocialLinkCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SocialLinkHandlers
{
    public class DeleteSocialLinkCommandHandler: IRequestHandler<DeleteSocialLinkCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteSocialLinkCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteSocialLinkCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var SocialLinkToDelete =
                await _context.SocialLink
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync(cancellationToken);

            if (SocialLinkToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("SocialLink not found");
                return Vm;
            }

            try
            {
                SocialLinkToDelete.IsDeleted = true;
                SocialLinkToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the SocialLink");
            }

            return Vm;
        }
    }
}
