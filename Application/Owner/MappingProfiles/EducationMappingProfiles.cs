using Application.Owner.Commands.EducationCommands;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class EducationMappingProfiles : Profile
    {
        public EducationMappingProfiles()
        {
            CreateMap<AddEditEducationCommand, Education>();
            CreateMap<Education, ELQ_Educations>();
        }
    }
}
