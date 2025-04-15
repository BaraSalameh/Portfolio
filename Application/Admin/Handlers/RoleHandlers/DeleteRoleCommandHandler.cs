using Application.Admin.Commands.RoleCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteRoleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var RoleToDelete =
                await _context.Role
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (RoleToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Role not found");
                return Vm;
            }

            try
            {
                RoleToDelete.IsDeleted = true;
                RoleToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Role");
            }

            return Vm;
        }
    }
}
