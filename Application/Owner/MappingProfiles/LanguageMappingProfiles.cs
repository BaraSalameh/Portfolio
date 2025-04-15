using Application.Owner.Commands.LanguageCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class LanguageMappingProfiles : Profile
    {
        public LanguageMappingProfiles()
        {
            CreateMap<AddEditLanguageCommand, Language>();
        }
    }
}
