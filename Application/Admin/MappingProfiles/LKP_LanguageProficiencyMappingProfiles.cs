using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class LKP_LanguageProficiencyMappingProfiles : Profile
    {
        public LKP_LanguageProficiencyMappingProfiles()
        {
            CreateMap<AddEditLKP_LanguageProficiencyCommand, LKP_LanguageProficiency>();
        }
    }
}
