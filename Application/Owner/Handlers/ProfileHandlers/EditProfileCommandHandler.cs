using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.Profile;
using AutoMapper;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.Profile
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, CommandResponse>
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

        public async Task<CommandResponse> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.User
                .FirstOrDefaultAsync(u => u.ID == _currentUserService.UserID!.Value, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("User not found.");
                return response;
            }

            _mapper.Map(request, existingEntity);


            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
