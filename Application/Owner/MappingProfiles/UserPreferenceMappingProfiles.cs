using Application.Owner.Commands.PreferenceCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserPreferenceMappingProfiles : Profile
    {
        public UserPreferenceMappingProfiles()
        {
            CreateMap<EditUserPreferenceCommand, UserPreference>();
        }
    }
}
