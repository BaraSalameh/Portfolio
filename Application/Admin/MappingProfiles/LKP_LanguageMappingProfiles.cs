using Application.Admin.Commands.LKP_LanguageCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class LKP_LanguageMappingProfiles : Profile
    {
        public LKP_LanguageMappingProfiles()
        {
            CreateMap<AddEditLKP_LanguageCommand, LKP_Language>();
        }
    }
}
