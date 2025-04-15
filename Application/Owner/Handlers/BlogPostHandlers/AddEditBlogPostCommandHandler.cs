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
    public class AddEditBlogPostCommandHandler : IRequestHandler<AddEditBlogPostCommand, AbstractViewModel>
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

        public async Task<AbstractViewModel> Handle(AddEditBlogPostCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            if (request.ID == null)
            {
                var ResultToDB = _mapper.Map<BlogPost>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;
                await _context.BlogPost.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldBlogPost = await _context.BlogPost
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

                if (oldBlogPost == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("BlogPost not found.");
                    return Vm;
                }

                _mapper.Map(request, oldBlogPost);
                oldBlogPost.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the BlogPost.");
            }

            return Vm;
        }
    }
}
