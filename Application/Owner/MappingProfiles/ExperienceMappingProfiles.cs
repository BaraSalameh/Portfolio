using Application.Owner.Commands.ExperienceCommands;
using Application.Owner.Queries.ExperienceQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ExperienceMappingProfiles : Profile
    {
        public ExperienceMappingProfiles()
        {
            CreateMap<Experience, ELQ_Response>();
            CreateMap<AddEditExperienceCommand, Experience>();
        }
    }
}
