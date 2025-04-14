using Application.Admin.Commands.RoleCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class RoleMappingProfiles
    {
        public IMapper AddEditRoleCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditRoleCommand, Role>();
            });
            return config.CreateMapper();
        }
    }
}
