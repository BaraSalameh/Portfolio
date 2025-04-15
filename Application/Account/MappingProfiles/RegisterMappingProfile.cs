using Application.Account.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Account.MappingProfiles
{
    public class RegisterMappingProfiles : Profile
    {
        public RegisterMappingProfiles()
        {
            CreateMap<RegisterCommand, User>();
        }
    }
}