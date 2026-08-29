using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.BlogPostCommands;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.BlogPostHandlers
{
    public class AddEditBlogPostCommandHandler : IRequestHandler<AddEditBlogPostCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _clock;

        public AddEditBlogPostCommandHandler(
            IAppDbContext context,
            ICurrentUserService currentUser,
            IMapper mapper,
            IDateTimeProvider clock)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
            _clock = clock;
        }

        public async Task<CommandResponse> Handle(AddEditBlogPostCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.PublishedAt == default)
            {
                response.lstError.Add("PublishedAt must be a valid date.");
                return response;
            }

            var requestedStatus = request.LKP_BlogPostStatusID;
            if (requestedStatus.HasValue &&
                requestedStatus != BlogPostStatusIdentifiers.Draft &&
                requestedStatus != BlogPostStatusIdentifiers.Published)
            {
                response.lstError.Add("Blog posts may only transition between Draft and Published through this endpoint.");
                return response;
            }
            if (requestedStatus == BlogPostStatusIdentifiers.Published &&
                request.PublishedAt > DateOnly.FromDateTime(_clock.UtcNow))
            {
                response.lstError.Add("A published blog post cannot have a future publication date.");
                return response;
            }

            if (request.ID == null)
            {
                var newEntity = _mapper.Map<BlogPost>(request);
                newEntity.UserID = _currentUser.UserID!.Value;
                newEntity.LKP_BlogPostStatusID = requestedStatus ?? BlogPostStatusIdentifiers.Draft;
                await _context.BlogPost.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                var existingEntity = await _context.BlogPost
                    .FirstOrDefaultAsync(x =>
                        x.UserID == _currentUser.UserID!.Value &&
                        x.ID == request.ID &&
                        x.IsDeleted == false,
                        cancellationToken
                    );

                if (existingEntity == null)
                {
                    response.lstError.Add("BlogPost not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
                if (requestedStatus.HasValue)
                {
                    existingEntity.LKP_BlogPostStatusID = requestedStatus.Value;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
