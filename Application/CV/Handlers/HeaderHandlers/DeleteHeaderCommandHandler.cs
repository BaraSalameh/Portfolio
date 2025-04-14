using Application.CV.Commands.HeaderCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CV.Handlers.HeaderHandlers
{
    public class DeleteHeaderCommandHandler : IRequestHandler<DeleteHeaderCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteHeaderCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteHeaderCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var headerToDelete =
                await _context.Profile
                    .Where(x => x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

            if (headerToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Header not found");
                return Vm;
            }

            try
            {
                headerToDelete.IsDeleted = true;
                headerToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Header");
            }

            return Vm;
        }
    }
}
