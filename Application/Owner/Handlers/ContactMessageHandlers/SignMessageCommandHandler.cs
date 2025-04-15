using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ContactMessageCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ContactMessage
{
    public class SignMessageCommandHandler : IRequestHandler<SignMessageCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;
        public SignMessageCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(SignMessageCommand request, CancellationToken cancellationToken)
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
                    .Where(m => m.UserID == _currentUserService.UserID.Value && m.ID == request.ID && m.IsRead == false && m.IsDeleted == false)
                    .FirstOrDefaultAsync(cancellationToken);

            if (oldMessage == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Message not found or it's already been read");
                return Vm;
            }

            oldMessage.IsRead = true;
            oldMessage.UpdatedAt = DateTime.UtcNow;
            
            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while reading the Message");
            }

            return Vm;
        }
    }
}
