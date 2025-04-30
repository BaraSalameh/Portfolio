using Application.Admin.Commands.LKP_TechnologyCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_TechnologyHandlers
{
    public class DeleteLKP_TechnologyCommandHandler : IRequestHandler<DeleteLKP_TechnologyCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_TechnologyCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteLKP_TechnologyCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var existingEntity = await _context.LKP_Technology
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("LKP_Technology not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the LKP_Technology.");
            }
            catch(Exception ex)
            {
                response.lstError.Add("Unexpected error occured.");
            }

            return response;
        }
    }
}
