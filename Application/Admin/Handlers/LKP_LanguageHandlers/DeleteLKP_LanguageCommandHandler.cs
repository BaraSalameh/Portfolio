using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageHandlers
{
    public class DeleteLKP_LanguageCommandHandler : IRequestHandler<DeleteLKP_LanguageCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_LanguageCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteLKP_LanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var LKP_LanguageToDelete =
                await _context.LKP_Language
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (LKP_LanguageToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("LKP_Language not found");
                return Vm;
            }

            try
            {
                LKP_LanguageToDelete.IsDeleted = true;
                LKP_LanguageToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the LKP_Language");
            }

            return Vm;
        }
    }
}
