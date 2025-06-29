using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Functions;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResponse<LC_Response>>
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

        public async Task<CommandResponse<LC_Response>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse<LC_Response>();

            string EncryptedPassword = request.Password.Encrypt(true);
            var existingEntity =
                 await _context.User
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == request.Email && u.Password == EncryptedPassword, cancellationToken);

            if (existingEntity == null)
            {
                response.ResultType = ResultType.NotFound;
                response.lstError.Add("Wrong username/password");
                return response;
            }
            
            if (!existingEntity.IsConfirmed)
            {
                _context.PendingEmailConfirmation.RemoveRange(
                    _context.PendingEmailConfirmation.Where(p => p.UserID == existingEntity.ID)
                );
                _pendingEmailConfirmationService.GenerateAsync(existingEntity, request.RememberMe);
                await _context.SaveChangesAsync(cancellationToken);

                response.ResultType = ResultType.Forbidden;
                response.lstError.Add("User lacks confirmation.");
                //await _userNotificationService.SendEmailConfirmationAsync(existingEntity);
                await _userNotificationService.SendEmailConfirmationMailjetAsync(existingEntity);
                return response;
            }

            await _authService.AuthSetupAsync(existingEntity, request.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            response.Data = new LC_Response
            {
                Username = existingEntity.Username!,
                Role = existingEntity.Role.Name!
            };

            return response;
        }
    }
} 