using Application.Admin.Commands.LKP_PreferenceCommands;
using Application.Common.Entities;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_PreferenceHandlers
{
    public class AddEditLKP_PreferenceCommandHandler : IRequestHandler<AddEditLKP_PreferenceCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLKP_PreferenceCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditLKP_PreferenceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.ID == null)
            {
                var newEntity = _mapper.Map<LKP_Preference>(request);
                await _context.LKP_Preference.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                var existingEntity = await _context.LKP_Preference
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("LKP_Preference not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
