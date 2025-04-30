using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
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

            try
            {
                var existingEntity = await _context.LKP_Language
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("LKP_Language not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the LKP_Language.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
