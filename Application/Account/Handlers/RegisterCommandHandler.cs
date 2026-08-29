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

            var defaultRoleExists = await _context.Role
                .AsNoTracking()
                .AnyAsync(role => role.ID == RoleIdentifiers.Owner, cancellationToken);
            if (!defaultRoleExists)
            {
                response.lstError.Add("Default user role not found.");
                return response;
            }

            var newEntity = _mapper.Map<User>(request);
            newEntity.Firstname = request.Firstname.Trim();
            newEntity.Lastname = request.Lastname.Trim();
            newEntity.Email = EmailNormalizer.Normalize(request.Email);
            newEntity.Username = UsernameGenerator.Create(newEntity.Firstname, newEntity.Lastname);
            newEntity.RoleID = RoleIdentifiers.Owner;
            newEntity.CreatedAt = _dateTimeProvider.UtcNow;
            newEntity.Password = _passwordService.Hash(newEntity, request.Password);

            var confirmation = _pendingEmailConfirmationService.Create(newEntity, request.RememberMe ?? false);
            var outboxMessage = _emailOutboxService.EnqueueConfirmation(confirmation);

            await _context.User.AddAsync(newEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await _emailOutboxService.AttemptImmediateDispatchAsync(outboxMessage.ID, cancellationToken);

            return response;
        }
    }
}
