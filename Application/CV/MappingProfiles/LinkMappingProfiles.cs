using Application.CV.Commands.LinkCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.CV.MappingProfiles
{
    public class LinkMappingProfiles
    {
        public IMapper AddEditLinkCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditLinkCommand, Link>();
            });
            return config.CreateMapper();
        }
    }
}
