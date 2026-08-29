using Application.Admin.Commands.LKP_PreferenceCommands;
using Application.Common.Entities;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_PreferenceHandlers
{
    public class DeleteLKP_PreferenceCommandHandler : IRequestHandler<DeleteLKP_PreferenceCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_PreferenceCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteLKP_PreferenceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.LKP_Preference
                .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("LKP_Preference not found.");
                return response;
            }

            if (await _context.UserPreference.AnyAsync(
                relation => relation.LKP_PreferenceID == request.ID,
                cancellationToken))
            {
                response.lstError.Add("Preference cannot be deleted while it is assigned to users.");
                return response;
            }

            existingEntity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
