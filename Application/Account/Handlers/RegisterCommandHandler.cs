using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Functions;
using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CommandResponse<RC_Response>>
    {
        private readonly IAuthService _authService;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;


        public RegisterCommandHandler(IAuthService authService, IAppDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider, IUserNotificationService userNotificationService, IPendingEmailConfirmationService pendingEmailConfirmationService)
        {
            _authService = authService;
            _context = context;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _userNotificationService = userNotificationService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
        }

        public async Task<CommandResponse<RC_Response>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse<RC_Response>();

            try
            {
                var role = await _context.Role.FindAsync(RoleIdentifiers.Owner, cancellationToken);
                if (role == null)
                {
                    response.ResultType = ResultType.ServerError;
                    response.lstError.Add("Default user role not found.");
                    return response;
                }

                request.Password = request.Password!.Encrypt(true);
                var baseUserName = $"{request.Firstname}-{request.Lastname}".ToLower().Replace(" ", "-");
                var guidSuffix = Guid.NewGuid().ToString("N").Substring(0, 6);

                var newEntity = _mapper.Map<User>(request);
                newEntity.Username = $"{baseUserName}-{guidSuffix}";
                newEntity.RoleID =  RoleIdentifiers.Owner;
                newEntity.Role = role;
                newEntity.CreatedAt = _dateTimeProvider.UtcNow;

                newEntity.IsConfirmed = true;


                //_pendingEmailConfirmationService.GenerateAsync(newEntity, request.RememberMe);

                await _context.User.AddAsync(newEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _authService.AuthSetupAsync(newEntity, request.RememberMe);
                await _context.SaveChangesAsync(cancellationToken);

                response.Data = new RC_Response
                {
                    Username = newEntity.Username,
                    Role = newEntity.Role.Name!
                };

                // choose one way for emailing when email structure is on
                //await _userNotificationService.SendEmailConfirmationAsync(newEntity);
                //await _userNotificationService.SendEmailConfirmationMailjetAsync(newEntity);
            }
            catch (DbUpdateException dbEx)
            {
                response.ResultType = ResultType.Conflict;
                response.lstError.Add("Email is already registered.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
//TO_CHANGE:
// 1- Line 59: delete
// 2- Line 62: Uncomment
// 3- Line 67: Move before the first saveAsync
// 4- Line 68: delete
// 5- Line 77, 78: Choose emailing structure