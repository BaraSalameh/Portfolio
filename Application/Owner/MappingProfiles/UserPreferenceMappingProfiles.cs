using Application.Owner.Commands.PreferenceCommands;
using Application.Owner.Queries.UserPreferenceQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserPreferenceMappingProfiles : Profile
    {
        public UserPreferenceMappingProfiles()
        {
            CreateMap<EditUserPreferenceCommand, UserPreference>();

            CreateMap<UserPreference, UPLQ_Response>()
                .ForMember(dest => dest.Preference, opt => opt.MapFrom(src => src.LKP_Preference));
            CreateMap<LKP_Preference, UPLQ_LKP_Preference>();

            CreateMap<LKP_Preference, LKP_PLQ_Response>();
        }
    }
}
