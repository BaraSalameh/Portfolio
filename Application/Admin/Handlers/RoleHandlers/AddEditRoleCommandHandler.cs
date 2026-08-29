using Application.Admin.Commands.RoleCommands;
using Application.Common.Entities;
using Application.Common.Functions;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Domain.Enums;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class AddEditRoleCommandHandler : IRequestHandler<AddEditRoleCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditRoleCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<CommandResponse> Handle(AddEditRoleCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.ID == RoleIdentifiers.Admin || request.ID == RoleIdentifiers.Owner)
            {
                response.lstError.Add("System roles cannot be modified.");
                return response;
            }

            request.Name = request.Name.ToPascalCase();

            if (request.ID == null)
            {
                var newEntity = _mapper.Map<Role>(request);
                await _context.Role.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                var existingEntity = await _context.Role
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("Role not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
