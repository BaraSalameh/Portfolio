using Application.Admin.Commands.LKP_PreferenceCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class LKP_PreferenceMappingProfiles: Profile
    {
        public LKP_PreferenceMappingProfiles()
        {
            CreateMap<AddEditLKP_PreferenceCommand, LKP_Preference>();
        }
    }
}
