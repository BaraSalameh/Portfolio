using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SkillCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SkillHandlers
{
    public class DeleteSkillCommandHandler: IRequestHandler<DeleteSkillCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteSkillCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var SkillToDelete =
                await _context.Skill
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && x.IsDeleted == false)
                    .FirstOrDefaultAsync(cancellationToken);

            if (SkillToDelete == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Skill not found");
                return Vm;
            }

            try
            {
                SkillToDelete.IsDeleted = true;
                SkillToDelete.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Skill");
            }

            return Vm;
        }
    }
}
