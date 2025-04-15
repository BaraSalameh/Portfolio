using Application.Owner.Commands.ExperienceCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ExperienceMappingProfiles : Profile
    {
        public ExperienceMappingProfiles()
        {
            CreateMap<AddEditExperienceCommand, Experience>();
        }
    }
}
