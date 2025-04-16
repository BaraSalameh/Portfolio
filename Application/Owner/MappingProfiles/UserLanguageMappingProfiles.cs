using Application.Owner.Commands.UserLanguageCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserLanguageMappingProfiles : Profile
    {
        public UserLanguageMappingProfiles()
        {
            CreateMap<AddEditUserLanguageCommand, UserLanguage>();
        }
    }
}
