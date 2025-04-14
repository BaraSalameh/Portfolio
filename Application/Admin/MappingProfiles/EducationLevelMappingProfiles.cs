using Application.Admin.Commands.EducationLevelCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class EducationLevelMappingProfiles
    {
        public IMapper AddEditEducationLevelCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditEducationLevelCommand, LKP_EducationLevel>();
            });
            return config.CreateMapper();
        }
    }
}
