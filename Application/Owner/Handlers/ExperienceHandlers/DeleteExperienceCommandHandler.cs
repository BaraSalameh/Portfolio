using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class DeleteExperienceCommandHandler: IRequestHandler<DeleteExperienceCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteExperienceCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteExperienceCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var ExperienceToDelete =
                await _context.Experience
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync(cancellationToken);

            if (ExperienceToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Experience not found");
                return Vm;
            }

            try
            {
                ExperienceToDelete.IsDeleted = true;
                ExperienceToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Experience");
            }

            return Vm;
        }
    }
}
