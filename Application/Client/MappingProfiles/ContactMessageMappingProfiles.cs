using Application.Client.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Client.MappingProfiles
{
    public class ContactMessageMappingProfiles : Profile
    {
        public ContactMessageMappingProfiles()
        {
            CreateMap<SendEmailCommand, ContactMessage>();
        }
    }
}
