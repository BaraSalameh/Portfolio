using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
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

            try
            {
                var existingEntity = await _context.LKP_LanguageProficiency
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("LKP_LanguageProficiency not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the LKP_LanguageProficiency.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
