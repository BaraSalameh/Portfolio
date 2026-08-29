using Application.Admin.Commands.RoleCommands;
using Application.Common.Entities;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;

        public DeleteRoleCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.ID == RoleIdentifiers.Admin || request.ID == RoleIdentifiers.Owner)
            {
                response.lstError.Add("System roles cannot be deleted.");
                return response;
            }

            var existingEntity = await _context.Role
                .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("Role not found.");
                return response;
            }

            if (await _context.User.AnyAsync(user => user.RoleID == request.ID, cancellationToken))
            {
                response.lstError.Add("Role cannot be deleted while it is assigned to users.");
                return response;
            }

            existingEntity.IsDeleted = true;
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
