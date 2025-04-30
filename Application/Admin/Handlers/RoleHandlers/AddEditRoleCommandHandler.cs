using Application.Admin.Commands.RoleCommands;
using Application.Common.Entities;
using Application.Common.Functions;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

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
            request.Name = request.Name.ToPascalCase();
            
            try
            {
                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<Role>(request);
                    await _context.Role.AddAsync(newEntity);
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
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the Role");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
