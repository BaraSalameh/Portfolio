using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;

namespace Application.Account.Handlers
{
    public class ActivateDeactivateUserCommandHandler : IRequestHandler<ActivateDeactivateUserCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IUserResolverService _userResolverService;

        public ActivateDeactivateUserCommandHandler(IAppDbContext context, IUserResolverService userResolverService)
        {
            _context = context;
            _userResolverService = userResolverService;
        }

        public async Task<AbstractViewModel> Handle(ActivateDeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var userToActivate = await _userResolverService.GetUserByEmailAsync(request.Email);

            if(userToActivate == null || userToActivate.ID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("User Not found");
                return Vm;
            }

            try
            {
                userToActivate.IsActive = !userToActivate.IsActive;
                userToActivate.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while activating/deactivating the user");
            }

            return Vm;
        }
    }
}
