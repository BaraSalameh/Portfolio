using Application.Admin.Commands.EducationLevelCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.EducationLevelHandlers
{
    public class DeleteEducationLevelCommandHandler : IRequestHandler<DeleteEducationLevelCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteEducationLevelCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteEducationLevelCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var EducationLevelToDelete =
                await _context.LKP_EducationLevel
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (EducationLevelToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Education level not found");
                return Vm;
            }

            try
            {
                EducationLevelToDelete.IsDeleted = true;
                EducationLevelToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Education level");
            }

            return Vm;
        }
    }
}
