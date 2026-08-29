using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserPreferenceCommands;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserPreferenceHandlers
{
    public class EditUserPreferenceCommandHandler : IRequestHandler<EditUserPreferenceCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EditUserPreferenceCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(EditUserPreferenceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (!await _context.LKP_Preference.AsNoTracking().AnyAsync(
                preference => preference.ID == request.LKP_PreferenceID,
                cancellationToken))
            {
                response.lstError.Add("Preference not found.");
                return response;
            }

            var existingEntity = await _context.UserPreference
                .FirstOrDefaultAsync(x =>
                    x.UserID == _currentUser.UserID!.Value &&
                    x.LKP_PreferenceID == request.LKP_PreferenceID &&
                    x.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                var newEntity = _mapper.Map<UserPreference>(request);
                newEntity.UserID = _currentUser.UserID!.Value;
                await _context.UserPreference.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                _mapper.Map(request, existingEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
