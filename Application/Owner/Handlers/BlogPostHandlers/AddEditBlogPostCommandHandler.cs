using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.BlogPostCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.BlogPostHandlers
{
    public class AddEditBlogPostCommandHandler : IRequestHandler<AddEditBlogPostCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditBlogPostCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditBlogPostCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<BlogPost>(request);
                    newEntity.UserID = _currentUser.UserID!.Value;
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
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the BlogPost.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
