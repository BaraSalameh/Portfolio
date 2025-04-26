using Application.Account.Commands;
using Application.Common.Functions;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, LC_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IUserNotificationService _userNotificationService;


        public LoginCommandHandler(IAppDbContext context, IAuthService authService, IPendingEmailConfirmationService pendingEmailConfirmationService, IUserNotificationService userNotificationService)
        {
            _context = context;
            _authService = authService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _userNotificationService = userNotificationService;
        }

        public async Task<LC_Response> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var Vm = new LC_Response();
            string EncryptedPassword = request.Password.Encrypt(true);
            var user =
                 await _context.User
                    .Where(u => u.Email == request.Email && u.Password == EncryptedPassword)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                Vm.lstError.Add("Wrong username/password");
                Vm.status = false;
                Vm.IsConfirmed = false;
                return Vm;
            }
            else if (!user.IsConfirmed)
            {
                _context.PendingEmailConfirmation.RemoveRange(
                    _context.PendingEmailConfirmation.Where(p => p.UserID == user.ID)
                );
                _pendingEmailConfirmationService.GenerateAsync(user, request.RememberMe);
                await _context.SaveChangesAsync(cancellationToken);

                Vm.status = true;
                Vm.Username = user.Username!;
                Vm.Role = user.Role.Name!;
                Vm.IsConfirmed = false;
                Vm.lstError.Add("User lacks confirmation.");

                await _userNotificationService.SendEmailConfirmationAsync(user);

                return Vm;

            }

            _authService.AuthSetupAsync(user, request.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            Vm.status = true;
            Vm.Username = user.Username!;
            Vm.Role = user.Role.Name!;
            Vm.IsConfirmed = true;

            return Vm;
        }
    }
} 