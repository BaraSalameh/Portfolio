
using Application.Common.Entities;
using Application.CV.Commands.EducationCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CV.Handlers.EducationHandlers
{
    public class DeleteEducationCommandHandler : IRequestHandler<DeleteEducationCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public DeleteEducationCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteEducationCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var EducationToDelete =
                await _context.Education
                    .Where(x => x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync();

            if (EducationToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Education not found");
                return Vm;
            }

            try
            {
                EducationToDelete.IsDeleted = true;
                EducationToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Education");
            }

            return Vm;
        }
    }
}
