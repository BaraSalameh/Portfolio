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
                .ForMember(dest => dest.LstEducations,
                    opt => opt.MapFrom(src => src.LstEducations.Select(use => use.Education))
                )
                .ForMember(dest => dest.LstExperiences,
                    opt => opt.MapFrom(src => src.LstExperiences.Select(use => use.Experience))
                )
                .ForMember(dest => dest.LstProjects,
                    opt => opt.MapFrom(src => src.LstProjects.Select(usp => usp.Project))
                )
                .ForMember(dest => dest.LstCertificates,
                    opt => opt.MapFrom(src => src.LstCertificates.Select(usc => usc.Certificate))
                );
            CreateMap<LKP_Skill, USLQ_LKP_Skill>();
            CreateMap<Education, USLQ_Education>()
                .ForMember(dest => dest.Institution,
                    opt => opt.MapFrom(src => src.LKP_Institution)
                );
            CreateMap<Experience, USLQ_Experience>();
            CreateMap<Project, USLQ_Project>();
            CreateMap<LKP_Certificate, USLQ_Certificate>();
            CreateMap<LKP_Institution, USLQ_LKP_Institution>();
            CreateMap<Certificate, USLQ_Certificate>()
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate));
            CreateMap<LKP_Certificate, USLQ_LKP_Certificate>();

            CreateMap<LKP_Skill, LKP_SLQ_Response>();
        }
    }
}
