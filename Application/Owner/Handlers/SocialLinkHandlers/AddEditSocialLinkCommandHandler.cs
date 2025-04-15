using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SocialLinkCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SocialLinkHandlers
{
    public class AddEditSocialLinkCommandHandler : IRequestHandler<AddEditSocialLinkCommand, AbstractViewModel>
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

        public async Task<AbstractViewModel> Handle(AddEditSocialLinkCommand request, CancellationToken cancellationToken)
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
                var ResultToDB = _mapper.Map<SocialLink>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;
                await _context.SocialLink.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldSocialLink = await _context.SocialLink
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

                if (oldSocialLink == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("SocialLink not found.");
                    return Vm;
                }

                _mapper.Map(request, oldSocialLink);
                oldSocialLink.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the SocialLink.");
            }

            return Vm;
        }
    }
}
