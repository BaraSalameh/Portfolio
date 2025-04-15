using Application.Admin.Commands.RoleCommands;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class AddEditRoleCommandHandler : IRequestHandler<AddEditRoleCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public AddEditRoleCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<AbstractViewModel> Handle(AddEditRoleCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<Role>(request);

            if (request.ID == null)
            {
                await _context.Role.AddAsync(ResultToDB);
            }
            else
            {
                var oldRole =
                    await _context.Role
                        .Where(x => x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                        .FirstOrDefaultAsync();

                if (oldRole == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Role not found");
                    return Vm;
                }

                _mapper.Map(request, oldRole);
                oldRole.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the Role");
            }

            return Vm;
        }
    }
}
