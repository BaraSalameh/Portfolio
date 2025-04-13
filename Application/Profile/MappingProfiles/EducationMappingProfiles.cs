using Application.Account.Commands.RegisterCommands;
using Application.Profile.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Profile.MappingProfiles
{
    public class EducationMappingProfiles
    {
        public IMapper AddEditEducationCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditEducationCommand, Education>();
            });
            return config.CreateMapper();
        }
    }
}