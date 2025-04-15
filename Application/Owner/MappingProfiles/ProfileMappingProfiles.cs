using Application.Owner.Commands.Profile;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ProfileMappingProfiles : Profile
    {
        public ProfileMappingProfiles()
        {
            CreateMap<EditProfileCommand, User>();
        }
    }
}
