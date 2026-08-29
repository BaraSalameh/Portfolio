using Application.Client.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Application.Common.Constants;
using Application.Common.Identity;

namespace Application.Client.Handlers
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolver;
        private readonly IEmailOutboxService _emailOutboxService;
        private readonly IContactSubmissionGuard _submissionGuard;

        public SendEmailCommandHandler(
            IAppDbContext context,
            IMapper mapper,
            IUserResolverService userResolver,
            IEmailOutboxService emailOutboxService,
            IContactSubmissionGuard submissionGuard)
        {
            _context = context;
            _mapper = mapper;
            _userResolver = userResolver;
            _emailOutboxService = emailOutboxService;
            _submissionGuard = submissionGuard;
        }
        public async Task<CommandResponse> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var user = await _userResolver.GetConfirmedUserByEmailAsync(
                request.EmailTo,
                cancellationToken);

            if (user == null)
            {
                // Preserve the same public success response for unknown targets so this
                // endpoint cannot be used to enumerate registered email addresses.
                return response;
            }

            var normalizedSenderEmail = EmailNormalizer.Normalize(request.Email);
            await _submissionGuard.ExecuteIfAllowedAsync(
                user.ID,
                normalizedSenderEmail,
                ContactSubmissionPolicy.SenderCooldown,
                async transactionCancellationToken =>
                {
                    var newEntity = _mapper.Map<ContactMessage>(request);
                    newEntity.ID = Guid.NewGuid();
                    newEntity.UserID = user.ID;
                    newEntity.Email = normalizedSenderEmail;
                    _emailOutboxService.EnqueueContactNotification(newEntity);

                    await _context.ContactMessage.AddAsync(newEntity, transactionCancellationToken);
                    await _context.SaveChangesAsync(transactionCancellationToken);
                },
                cancellationToken);

            return response;
        }
    }
}
