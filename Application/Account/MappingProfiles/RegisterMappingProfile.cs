using Application.Account.Commands.RegisterCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Account.MappingProfiles
{
    public class RegisterMappingProfiles
    {
        public IMapper RegisterCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RegisterCommand, User>();
            });
            return config.CreateMapper();
        }
    }
}