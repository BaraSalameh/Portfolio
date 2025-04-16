using Application.Admin.Commands.LKP_TechnologyCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class LKP_TechnologyMappingProfiles : Profile
    {
        public LKP_TechnologyMappingProfiles()
        {
            CreateMap<AddEditLKP_TechnologyCommand, LKP_Technology>();
        }
    }
}
