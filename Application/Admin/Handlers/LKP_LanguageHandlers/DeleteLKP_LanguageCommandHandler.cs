using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Common.Entities;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageHandlers
{
    public class DeleteLKP_LanguageCommandHandler : IRequestHandler<DeleteLKP_LanguageCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_LanguageCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteLKP_LanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.LKP_Language
                .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("LKP_Language not found.");
                return response;
            }

            if (await _context.UserLanguage.AnyAsync(
                relation => relation.LKP_LanguageID == request.ID,
                cancellationToken))
            {
                response.lstError.Add("Language cannot be deleted while it is assigned to users.");
                return response;
            }

            existingEntity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
