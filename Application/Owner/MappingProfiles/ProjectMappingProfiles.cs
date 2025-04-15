using Application.Owner.Commands.ProjectCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ProjectMappingProfiles : Profile
    {
        public ProjectMappingProfiles()
        {
            CreateMap<AddEditProjectCommand, Project>();
            CreateMap<AEPC_Technology, Technology>();
        }
    }
}
