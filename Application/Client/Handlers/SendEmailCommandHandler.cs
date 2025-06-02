using Application.Client.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Client.Handlers
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolver;
        private readonly IUserNotificationService _userNotificationService;

        public SendEmailCommandHandler(IAppDbContext context, IMapper mapper, IUserResolverService userResolver, IUserNotificationService userNotificationService)
        {
            _context = context;
            _mapper = mapper;
            _userResolver = userResolver;
            _userNotificationService = userNotificationService;
        }
        public async Task<CommandResponse> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var user = await _userResolver.GetUserByEmailAsync(request.EmailTo, cancellationToken);

                if (user == null || user.ID == null)
                {
                    response.lstError.Add("User not found.");
                    return response;
                }

                var newEntity = _mapper.Map<ContactMessage>(request);
                newEntity.UserID = user.ID.Value;

                await _context.ContactMessage.AddAsync(newEntity, cancellationToken);
                await _context.SaveChangesAsync();

                await _userNotificationService.SendContactMessageNotificationEmail(request);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Unable to send email.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
