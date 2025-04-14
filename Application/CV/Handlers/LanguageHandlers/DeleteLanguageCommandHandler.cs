
using Application.Common.Entities;
using Application.CV.Commands.LanguageCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CV.Handlers.LanguageHandlers
{
    public class DeleteLanguageCommandHandler : IRequestHandler<DeleteLanguageCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteLanguageCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteLanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var LanguageToDelete =
                await _context.Language
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (LanguageToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Language not found");
                return Vm;
            }

            try
            {
                LanguageToDelete.IsDeleted = true;
                LanguageToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Language");
            }

            return Vm;
        }
    }
}
