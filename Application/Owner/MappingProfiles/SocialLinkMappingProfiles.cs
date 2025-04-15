using Application.Owner.Commands.SocialLinkCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class SocialLinkMappingProfiles : Profile
    {
        public SocialLinkMappingProfiles()
        {
            CreateMap<AddEditSocialLinkCommand, SocialLink>();
        }
    }
}
