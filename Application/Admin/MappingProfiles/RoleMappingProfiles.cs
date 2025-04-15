using Application.Admin.Commands.RoleCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class RoleMappingProfiles : Profile
    {
        public RoleMappingProfiles()
        {
            CreateMap<AddEditRoleCommand, Role>();
        }
    }
}
