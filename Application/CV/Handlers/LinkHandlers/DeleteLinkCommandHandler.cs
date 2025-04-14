
using Application.Common.Entities;
using Application.CV.Commands.LinkCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CV.Handlers.LinkHandlers
{
    public class DeleteLinkCommandHandler : IRequestHandler<DeleteLinkCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteLinkCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var LinkToDelete =
                await _context.Link
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (LinkToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Link not found");
                return Vm;
            }

            try
            {
                LinkToDelete.IsDeleted = true;
                LinkToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Link");
            }

            return Vm;
        }
    }
}
