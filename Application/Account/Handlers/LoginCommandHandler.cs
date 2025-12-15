using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using Domain.Enums;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, CommandResponse<LC_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthService _authService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public LoginCommandHandler(
            IAppDbContext context,
            IAuthService authService,
            IPendingEmailConfirmationService pendingEmailConfirmationService,
            IUserNotificationService userNotificationService,
            IPasswordHasher<User> passwordHasher
        )
        {
            _context = context;
            _authService = authService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _userNotificationService = userNotificationService;
            _passwordHasher = passwordHasher;
        }

        public async Task<CommandResponse<LC_Response>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse<LC_Response>();

            var existingEntity =
                 await _context.User
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (existingEntity == null)
            {
                response.ResultType = ResultType.NotFound;
                response.lstError.Add("Wrong username/password");
                return response;
            }

            var verifiedResult = _passwordHasher.VerifyHashedPassword(existingEntity, existingEntity.Password, request.Password);
            if (verifiedResult != PasswordVerificationResult.Success)
            {
                response.ResultType = ResultType.NotFound;
                response.lstError.Add("Wrong username/password");
                return response;
            }

            try
            {
                if (!existingEntity.IsConfirmed)
                {
                    _context.PendingEmailConfirmation.RemoveRange(
                        _context.PendingEmailConfirmation.Where(p => p.UserID == existingEntity.ID)
                    );

                    var rawToken = _pendingEmailConfirmationService.Create(existingEntity, request.RememberMe);
                    await _context.SaveChangesAsync(cancellationToken);

                    await _userNotificationService.SendEmailConfirmationAsync(existingEntity, rawToken);

                    response.ResultType = ResultType.Forbidden;
                    response.lstError.Add("User lacks confirmation.");
                    return response;
                }

                await _authService.AuthSetupAsync(existingEntity, request.RememberMe);
                await _context.SaveChangesAsync(cancellationToken);

                response.Data = new LC_Response
                {
                    Username = existingEntity.Username!,
                    Role = existingEntity.Role.Name!
                };
            } catch (DbUpdateException dbEx)
            {
                response.ResultType = ResultType.ServerError;
                response.lstError.Add("An error occurred while updating user authentication data.");
            } catch (Exception ex)
            {
                response.ResultType = ResultType.ServerError;
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
} 