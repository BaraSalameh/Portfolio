using Application.CV.Commands.LanguageCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.CV.MappingProfiles
{
    class LanguageMappingProfiles
    {
        public IMapper AddEditLanguageCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditLanguageCommand, Language>();
            });
            return config.CreateMapper();
        }
    }
}
