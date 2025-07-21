using Application.Owner.Commands.SkillCommands;
using Application.Owner.Queries.SkillQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class SkillMappingProfiles : Profile
    {
        public SkillMappingProfiles()
        {
            CreateMap<Skill, SLQ_Response>()
                .ForMember(dest => dest.Skill,
                    opt => opt.MapFrom(src => src.LKP_Skill)
                )
                .ForMember(dest => dest.Education,
                    opt => opt.MapFrom(src => src.Education)
                )
                .ForMember(dest => dest.Experience,
                    opt => opt.MapFrom(src => src.Experience)
                )
                .ForMember(dest => dest.Project,
                    opt => opt.MapFrom(src => src.Project)
                );
            CreateMap<LKP_Skill, SLQ_LKP_Skill>()
                .ForMember(dest => dest.SkillCategory,
                    opt => opt.MapFrom(src => src.LKP_SkillCategory)
                );
            CreateMap<Education, SLQ_PS_Education>()
                .ForMember(dest => dest.Institution,
                    opt => opt.MapFrom(src => src.LKP_Institution)
                );
            CreateMap<Experience, SLQ_PS_Experience>();
            CreateMap<Project, SLQ_Project>();
            CreateMap<LKP_SkillCategory, SLQ_LKP_SkillCategory>();
            CreateMap<LKP_Institution, SLQ_LKP_Institution>();

            CreateMap<EditDeleteSkillCommand, User>()
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src =>
                    (src.LstSkills ?? new List<EDSC_LKP_Skill>()).Select(id => new Skill
                    {
                        LKP_SkillID = id.LKP_SkillID,
                        Proficiency = id.Proficiency,
                        EducationID = id.EducationID,
                        ExperienceID = id.ExperienceID,
                        ProjectID = id.ProjectID,
                        Description = id.Description
                    }).ToList()
                ));

            CreateMap<LKP_SkillCategory, LKP_SCLQ_Response>();

            CreateMap<LKP_Skill, LKP_SLQ_Response>();
        }
    }
}
