using Application.Owner.Commands.EducationCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class EducationMappingProfiles : Profile
    {
        public EducationMappingProfiles()
        {
            CreateMap<AddEditEducationCommand, Education>();
        }
    }
}
