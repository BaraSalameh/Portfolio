using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Functions;
using Application.Common.Services.Interface;
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
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;


        public RegisterCommandHandler(IAppDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider, IUserNotificationService userNotificationService, IPendingEmailConfirmationService pendingEmailConfirmationService)
        {
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

            
                _pendingEmailConfirmationService.GenerateAsync(newEntity, request.RememberMe);
                await _context.User.AddAsync(newEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                response.Data = new RC_Response
                {
                    Username = newEntity.Username,
                    Role = newEntity.Role.Name!
                };

                await _userNotificationService.SendEmailConfirmationAsync(newEntity);
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