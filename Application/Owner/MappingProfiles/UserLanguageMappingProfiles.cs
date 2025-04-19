using Application.Owner.Commands.UserLanguageCommands;
using Application.Owner.Queries.LKP_LanguageQuieries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserLanguageMappingProfiles : Profile
    {
        public UserLanguageMappingProfiles()
        {
            CreateMap<EditDeleteUserLanguageCommand, User>()
                .ForMember(dest => dest.LstUserLanguages, opt => opt.MapFrom(src =>
                    (src.LstLanguages ?? new List<EDULC_LKP_Language>()).Select(id => new UserLanguage
                    {
                        LKP_LanguageID = id.LKP_LanguageID,
                        LKP_LanguageProficiencyID = id.LKP_LanguageProficiencyID
                    }).ToList()
                ));

            CreateMap<LKP_Language, LKP_LanguageListQuery>();

        }
    }
}
