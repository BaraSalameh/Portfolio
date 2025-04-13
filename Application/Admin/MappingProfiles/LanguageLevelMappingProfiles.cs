using Application.Admin.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Admin.MappingProfiles
{
    public class LanguageLevelMappingProfiles
    {
        public IMapper AddEditLanguageLevelCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditLanguageLevelCommand, LKP_LanguageLevel>();
            });
            return config.CreateMapper();
        }
    }
}
