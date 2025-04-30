using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectTechnologyCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectTechnologyHandlers
{
    public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public DeleteProjectCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<CommandResponse> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var existingEntity = await _context.Project
                    .FirstOrDefaultAsync(p =>
                        p.UserID == _currentUser.UserID!.Value &&
                        p.ID == request.ID &&
                        p.IsDeleted == false,
                        cancellationToken
                    );

                if(existingEntity == null)
                {
                    response.lstError.Add("Project not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the Project.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
