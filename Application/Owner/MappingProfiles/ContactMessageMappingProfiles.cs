using Application.Owner.Queries.ContactMessageQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ContactMessageMappingProfiles : Profile
    {
        public ContactMessageMappingProfiles()
        {
            CreateMap<ContactMessage, CMLQ_Response>();
        }
    }
}
