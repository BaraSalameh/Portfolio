using Application.Owner.Commands.SkillCommands;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class SkillMappingProfiles : Profile
    {
        public SkillMappingProfiles()
        {
            CreateMap<AddEditSkillCommand, Skill>();
        }
    }
}
