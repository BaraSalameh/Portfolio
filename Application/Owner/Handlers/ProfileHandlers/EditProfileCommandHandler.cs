using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Common.Services.Service;
using Application.Owner.Commands.Profile;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.Profile
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EditProfileCommandHandler(IAppDbContext context, ICurrentUserService currentUserService, IMapper mapper)
        {
            _currentUserService = currentUserService;
            _context = context;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if(!_currentUserService.IsAuthenticated || _currentUserService.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }
            
            var oldUser =
                await _context.User
                    .Where(u => u.ID == _currentUserService.UserID.Value)
                    .FirstOrDefaultAsync();

            if (oldUser == null)
            {
                Vm.status = false;
                Vm.lstError.Add("User not found");
                return Vm;
            }

            _mapper.Map(request, oldUser);
            oldUser.UpdatedAt = DateTime.UtcNow;
            

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the User");
            }

            return Vm;
        }
    }
}
