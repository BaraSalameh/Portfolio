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
                )
                .ForMember(dest => dest.Certificate,
                    opt => opt.MapFrom(src => src.Certificate)
                );
            CreateMap<LKP_Skill, USLQ_LKP_Skill>();
            CreateMap<Education, USLQ_PS_Education>()
                .ForMember(dest => dest.Institution,
                    opt => opt.MapFrom(src => src.LKP_Institution)
                );
            CreateMap<Experience, USLQ_PS_Experience>();
            CreateMap<Project, USLQ_S_Project>();
            CreateMap<LKP_Certificate, USLQC_Certificate>();
            CreateMap<LKP_Institution, USLQ_LKP_Institution>();
            CreateMap<Certificate, USLQC_Certificate>()
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate));
            CreateMap<LKP_Certificate, USLQCC_Certificate>();

            CreateMap<EditDeleteUserSkillCommand, User>()
                .ForMember(dest => dest.LstUserSkills, opt => opt.MapFrom(src =>
                    (src.LstUserSkills ?? new List<EDUSC_LKP_Skill>()).Select(id => new UserSkill
                    {
                        LKP_SkillID = id.LKP_SkillID,
                        EducationID = id.EducationID,
                        ExperienceID = id.ExperienceID,
                        ProjectID = id.ProjectID,
                        CertificateID = id.CertificateID
                    }).ToList()
                ));

            CreateMap<LKP_Skill, LKP_SLQ_Response>();
        }
    }
}
