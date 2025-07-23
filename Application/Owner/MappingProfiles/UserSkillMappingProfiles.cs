using Application.Owner.Commands.UserSkillCommands;
using Application.Owner.Queries.UserSkillQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class SkillMappingProfiles : Profile
    {
        public SkillMappingProfiles()
        {
            CreateMap<UserSkill, USLQ_Response>()
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
            CreateMap<LKP_Skill, USLQ_LKP_Skill>()
                .ForMember(dest => dest.SkillCategory,
                    opt => opt.MapFrom(src => src.LKP_SkillCategory)
                );
            CreateMap<Education, USLQ_PS_Education>()
                .ForMember(dest => dest.Institution,
                    opt => opt.MapFrom(src => src.LKP_Institution)
                );
            CreateMap<Experience, USLQ_PS_Experience>();
            CreateMap<Project, USLQ_S_Project>();
            CreateMap<LKP_SkillCategory, USLQ_LKP_SkillCategory>();
            CreateMap<LKP_Institution, USLQ_LKP_Institution>();

            CreateMap<EditDeleteUserSkillCommand, User>()
                .ForMember(dest => dest.LstUserSkills, opt => opt.MapFrom(src =>
                    (src.LstUserSkills ?? new List<EDUSC_LKP_Skill>()).Select(id => new UserSkill
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

            CreateMap<LKP_Skill, LKP_SLQ_Response>()
                .ForMember(dest => dest.SkillCategory,
                    opt => opt.MapFrom(src => src.LKP_SkillCategory)
                );
            CreateMap<LKP_SkillCategory, LKP_SLQ_SkillCategory>();
        }
    }
}
