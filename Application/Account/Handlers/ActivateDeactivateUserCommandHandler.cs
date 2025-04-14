using Application.Account.Commands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class ActivateDeactivateUserCommandHandler : IRequestHandler<ActivateDeactivateUserCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public ActivateDeactivateUserCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(ActivateDeactivateUserCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var userToActivate =
                await _context.User
                    .Where(x => x.ID == request.ID)
                    .FirstOrDefaultAsync();

            if (userToActivate == null)
            {
                Vm.status = false;
                Vm.lstError.Add("User not found");
                return Vm;
            }

            try
            {
                userToActivate.IsActive = !userToActivate.IsActive;
                userToActivate.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
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
