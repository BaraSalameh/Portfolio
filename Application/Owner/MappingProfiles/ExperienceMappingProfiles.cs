using Application.Owner.Commands.ExperienceCommands;
using Application.Owner.Queries.ExperienceQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ExperienceMappingProfiles : Profile
    {
        public ExperienceMappingProfiles()
        {
            CreateMap<AddEditExperienceCommand, Experience>()
                .ForMember(dest => dest.LstUserSkillExperiences, opt => opt.Ignore());

            CreateMap<Experience, ELQ_Response>()
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillExperiences.Select(use => use.UserSkill).Select(us => us.LKP_Skill)));
            CreateMap<LKP_Skill, ELQ_LKP_Skill>();
        }
    }
}
