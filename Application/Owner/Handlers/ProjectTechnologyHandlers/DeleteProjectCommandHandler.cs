using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectTechnologyCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectTechnologyHandlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public DeleteProjectCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<AbstractViewModel> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var ProjectToDelete = await _context.Project
                .Where(p => p.UserID == _currentUser.UserID.Value && p.ID == request.ID && (p.IsDeleted == false || p.IsDeleted == null))
                .FirstOrDefaultAsync(cancellationToken);

            if(ProjectToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Project not found");
                return Vm;
            }

            try
            {
                ProjectToDelete.IsDeleted = true;
                ProjectToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Project");
            }


            return Vm;
        }
    }
}
