using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Common.Entities;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageProficiencyHandlers
{
    public class DeleteLKP_LanguageProficiencyCommandHandler : IRequestHandler<DeleteLKP_LanguageProficiencyCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_LanguageProficiencyCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteLKP_LanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.LKP_LanguageProficiency
                .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("LKP_LanguageProficiency not found.");
                return response;
            }

            if (await _context.UserLanguage.AnyAsync(
                relation => relation.LKP_LanguageProficiencyID == request.ID,
                cancellationToken))
            {
                response.lstError.Add("Language proficiency cannot be deleted while it is assigned to users.");
                return response;
            }

            existingEntity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
