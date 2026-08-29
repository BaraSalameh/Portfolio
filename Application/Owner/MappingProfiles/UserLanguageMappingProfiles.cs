using Application.Owner.Commands.UserLanguageCommands;
using Application.Owner.Queries.LKP_LanguageQuieries;
using Application.Owner.Queries.UserLanguageQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserLanguageMappingProfiles : Profile
    {
        public UserLanguageMappingProfiles()
        {
            CreateMap<UserLanguage, ULLQ_Response>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LKP_Language))
                .ForMember(dest => dest.LanguageProficiency, opt => opt.MapFrom(src => src.LKP_LanguageProficiency));
            CreateMap<LKP_Language, ULLQ_LKP_Language>();
            CreateMap<LKP_LanguageProficiency, ULLQ_LKP_LanguageProficiency>();

            CreateMap<LKP_Language, LKP_LLQ_Response>();

            CreateMap<LKP_LanguageProficiency, LKP_LPLQ_Response>();
        }
    }
}
