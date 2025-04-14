using Application.CV.Commands.HeaderCommands;
using AutoMapper;

namespace Application.CV.MappingProfiles
{
    public class HeaderMappingProfiles
    {
        public IMapper AddEditHeaderCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditHeaderCommand, Domain.Entities.Profile>();
            });
            return config.CreateMapper();
        }
    }
}
