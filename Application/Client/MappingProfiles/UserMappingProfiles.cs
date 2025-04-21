using Application.Client.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Client.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<User, ULQ_Response>();
        }
    }
}
