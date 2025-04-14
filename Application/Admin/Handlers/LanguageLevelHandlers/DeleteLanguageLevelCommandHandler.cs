using Application.Admin.Commands.LanguageLevelCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LanguageLevelHandlers
{
    public class DeleteLanguageLevelCommandHandler : IRequestHandler<DeleteLanguageLevelCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteLanguageLevelCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteLanguageLevelCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var LanguageLevelToDelete =
                await _context.LKP_LanguageLevel
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (LanguageLevelToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Language level not found");
                return Vm;
            }

            try
            {
                LanguageLevelToDelete.IsDeleted = true;
                LanguageLevelToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the language level");
            }

            return Vm;
        }
    }
}
