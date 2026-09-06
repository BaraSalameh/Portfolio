using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SocialLinkCommands;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Constants;

namespace Application.Owner.Handlers.SocialLinkHandlers
{
    public class AddEditSocialLinkCommandHandler : IRequestHandler<AddEditSocialLinkCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditSocialLinkCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditSocialLinkCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.ID == null)
            {
                var activeOrders = await _context.SocialLink
                    .Where(link => link.UserID == _currentUser.UserID!.Value && !link.IsDeleted)
                    .Select(link => link.Order)
                    .Take(ProfileLimits.MaximumSocialLinks)
                    .ToListAsync(cancellationToken);
                if (activeOrders.Count >= ProfileLimits.MaximumSocialLinks)
                {
                    response.lstError.Add($"A profile can contain at most {ProfileLimits.MaximumSocialLinks} site links.");
                    return response;
                }

                var newEntity = _mapper.Map<SocialLink>(request);
                newEntity.UserID = _currentUser.UserID!.Value;
                newEntity.Order = activeOrders.Count == 0 ? 1 : activeOrders.Max() + 1;
                await _context.SocialLink.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                var existingEntity = await _context.SocialLink
                    .FirstOrDefaultAsync(x =>
                        x.UserID == _currentUser.UserID!.Value &&
                        x.ID == request.ID &&
                        x.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("SocialLink not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
