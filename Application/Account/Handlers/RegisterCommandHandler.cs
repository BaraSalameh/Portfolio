using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Identity;

namespace Application.Account.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailOutboxService _emailOutboxService;
        private readonly IPendingEmailConfirmationService _pendingEmailConfirmationService;
        private readonly IPasswordService _passwordService;

        public RegisterCommandHandler(
            IAppDbContext context,
            IMapper mapper,
            IDateTimeProvider dateTimeProvider,
            IEmailOutboxService emailOutboxService,
            IPendingEmailConfirmationService pendingEmailConfirmationService,
            IPasswordService passwordService
        )
        {
            _context = context;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _emailOutboxService = emailOutboxService;
            _pendingEmailConfirmationService = pendingEmailConfirmationService;
            _passwordService = passwordService;
        }

        public async Task<CommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            var normalizedEmail = EmailNormalizer.Normalize(request.Email);

            var existingUser = await _context.User
                .Include(user => user.LstPendingEmailConfirmations)
                .SingleOrDefaultAsync(user => user.Email == normalizedEmail, cancellationToken);
            if (existingUser?.IsConfirmed == true)
            {
                response.lstError.Add("An account with this email address already exists.");
                return response;
            }

            var defaultRoleExists = await _context.Role
                .AsNoTracking()
                .AnyAsync(role => role.ID == RoleIdentifiers.Owner, cancellationToken);
            if (!defaultRoleExists)
            {
                response.lstError.Add("Default user role not found.");
                return response;
            }

            var newEntity = existingUser ?? _mapper.Map<User>(request);
            if (existingUser is null)
            {
                newEntity.ID = Guid.NewGuid();
                newEntity.CreatedAt = _dateTimeProvider.UtcNow;
            }
            else
            {
                foreach (var pendingConfirmation in newEntity.LstPendingEmailConfirmations
                    .Where(confirmation => confirmation.RevokedAt == null))
                {
                    pendingConfirmation.RevokedAt = _dateTimeProvider.UtcNow;
                }

                newEntity.UpdatedAt = _dateTimeProvider.UtcNow;
            }

            newEntity.Firstname = request.Firstname.Trim();
            newEntity.Lastname = request.Lastname.Trim();
            newEntity.Email = normalizedEmail;
            newEntity.Username = UsernameGenerator.Create(newEntity.Firstname, newEntity.Lastname);
            newEntity.RoleID = RoleIdentifiers.Owner;
            newEntity.Password = _passwordService.Hash(newEntity, request.Password);

            if (existingUser is null)
            {
                await _context.User.AddAsync(newEntity, cancellationToken);
            }

            var confirmation = _pendingEmailConfirmationService.Create(newEntity, request.RememberMe ?? false);
            _context.PendingEmailConfirmation.Add(confirmation);
            var outboxMessage = _emailOutboxService.EnqueueConfirmation(confirmation);
            await _context.SaveChangesAsync(cancellationToken);
            await _emailOutboxService.AttemptImmediateDispatchAsync(outboxMessage.ID, cancellationToken);

            return response;
        }
    }
}
