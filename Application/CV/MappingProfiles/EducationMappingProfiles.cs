using Application.CV.Commands.EducationCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.CV.MappingProfiles
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