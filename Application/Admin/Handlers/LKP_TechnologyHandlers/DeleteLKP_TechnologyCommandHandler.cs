using Application.Admin.Commands.LKP_TechnologyCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_TechnologyHandlers
{
    public class DeleteLKP_TechnologyCommandHandler : IRequestHandler<DeleteLKP_TechnologyCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_TechnologyCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteLKP_TechnologyCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var LKP_TechnologyToDelete =
                await _context.LKP_Technology
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (LKP_TechnologyToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("LKP_Technology not found");
                return Vm;
            }

            try
            {
                LKP_TechnologyToDelete.IsDeleted = true;
                LKP_TechnologyToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the LKP_Technology");
            }

            return Vm;
        }
    }
}
