using Application.Common.Entities;
using Application.Common.Services;
using Application.Owner.Commands.ContactMessageCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ContactMessageHandlers
{
    public class DeleteMessageCommandHandler : IRequestHandler<DeleteMessageCommand, AbstractViewModel>
    {
        private readonly CurrentUserService _currentUserService;
        private readonly IAppDbContext _context;

        public DeleteMessageCommandHandler(IAppDbContext context, CurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = context;

        }

        public async Task<AbstractViewModel> Handle(DeleteMessageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if(!_currentUserService.IsAuthenticated || _currentUserService.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user");
                return Vm;
            }

            var oldMessage =
                await _context.ContactMessage
                    .Where(m => m.UserID == _currentUserService.UserID.Value && m.ID == request.ID && (m.IsDeleted == false || m.IsDeleted == null))
                    .FirstOrDefaultAsync();

            if (oldMessage == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Message not found");
                return Vm;
            }

            oldMessage.IsDeleted = true;
            oldMessage.DeletedAt = DateTime.UtcNow;
            

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while deleting the Message");
            }

            return Vm;
        }
    }
}
