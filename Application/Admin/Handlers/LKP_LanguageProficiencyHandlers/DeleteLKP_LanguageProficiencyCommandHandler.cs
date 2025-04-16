using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageProficiencyHandlers
{
    public class DeleteLKP_LanguageProficiencyCommandHandler : IRequestHandler<DeleteLKP_LanguageProficiencyCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_LanguageProficiencyCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteLKP_LanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var LKP_LanguageProficiencyToDelete =
                await _context.LKP_LanguageProficiency
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (LKP_LanguageProficiencyToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("LKP_LanguageProficiency not found");
                return Vm;
            }

            try
            {
                LKP_LanguageProficiencyToDelete.IsDeleted = true;
                LKP_LanguageProficiencyToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the LKP_LanguageProficiency");
            }

            return Vm;
        }
    }
}
