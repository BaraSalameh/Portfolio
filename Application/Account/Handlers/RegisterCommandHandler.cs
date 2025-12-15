using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserNotificationService _userNotificationService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IPasswordHasher<User> _passwordHasher;

        public RegisterCommandHandler(
            IAppDbContext context,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            IUserNotificationService userNotificationService,
            IPendingEmailConfirmationService pendingEmailConfirmationService,
            IPasswordHasher<User> passwordHasher
        )
        {
            _context = context;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _userNotificationService = userNotificationService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _passwordHasher = passwordHasher;
        }

        public async Task<CommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var role = await _context.Role.FindAsync(RoleIdentifiers.Owner, cancellationToken);
                if (role == null)
                {
                    response.lstError.Add("Default user role not found.");
                    return response;
                }

                var baseUserName = $"{request.Firstname}-{request.Lastname}".ToLower().Replace(" ", "-");
                var guidSuffix = Guid.NewGuid().ToString("N").Substring(0, 6);

                var newEntity = _mapper.Map<User>(request);
                newEntity.Username = $"{baseUserName}-{guidSuffix}";
                newEntity.RoleID =  RoleIdentifiers.Owner;
                newEntity.Role = role;
                newEntity.CreatedAt = _dateTimeProvider.UtcNow;
                newEntity.Password = _passwordHasher.HashPassword(newEntity, request.Password);

                var rawToken = _pendingEmailConfirmationService.Create(newEntity, request.RememberMe ?? false);

                await _context.User.AddAsync(newEntity, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await _userNotificationService.SendEmailConfirmationAsync(newEntity, rawToken);

            }
            catch (DbUpdateException dbEx)
            {
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