using Application.Client.Queries;
using Application.Owner.Queries.UserQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Client.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<User, ULQ_Response>();

            CreateMap<User, UBUQ_Response>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.LstEducations,
                    opt => opt.MapFrom(src => src.LstEducations
                        .Where(e => e.IsDeleted == false)
                        .OrderBy(e => e.Order)
                    ))
                .ForMember(dest => dest.LstExperiences,
                    opt => opt.MapFrom(src => src.LstExperiences
                        .Where(e => e.IsDeleted == false)
                        .OrderBy(e => e.Order)
                    ))
                .ForMember(dest => dest.LstSkills,
                    opt => opt.MapFrom(src => src.LstSkills.Where(s => s.IsDeleted == false)))
                .ForMember(dest => dest.LstProjects,
                    opt => opt.MapFrom(src => src.LstProjects.Where(p => p.IsDeleted == false)))
                .ForMember(dest => dest.LstBlogPosts,
                    opt => opt.MapFrom(src => src.LstBlogPosts.Where(p => p.IsDeleted == false)))
                .ForMember(dest => dest.LstSocialLinks,
                    opt => opt.MapFrom(src => src.LstSocialLinks.Where(l => l.IsDeleted == false)));
            CreateMap<User, UBUQ_User>();
            CreateMap<Project, UBUQ_Project>()
                .ForMember(dest => dest.LstTechnologies,
                    opt => opt.MapFrom(src => src.LstProjectTechnologies.Select(pt => pt.LKP_Technology))
                )
                .ForMember(dest => dest.Education,
                    opt => opt.MapFrom(src => src.Education)
                )
                .ForMember(dest => dest.Experience,
                    opt => opt.MapFrom(src => src.Experience)
                );
            CreateMap<LKP_Technology, UBUQ_LKP_Technology>();
            CreateMap<Skill, UBUQ_Skill>()
                .ForMember(dest => dest.Education,
                    opt => opt.MapFrom(src => src.Education)
                )
                .ForMember(dest => dest.Experience,
                    opt => opt.MapFrom(src => src.Experience)
                );
            CreateMap<Education, UBUQ_PS_Education>()
                .ForMember(dest => dest.Institution,
                    opt => opt.MapFrom(src => src.LKP_Institution)
                );
            CreateMap<Experience, UBUQ_PS_Experience>();
            CreateMap<Education, UBUQ_Education>()
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution))
                .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.LKP_Degree))
                .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.LKP_FieldOfStudy));
            CreateMap<LKP_Institution, UBUQ_LKP_Institution>();
            CreateMap<LKP_Degree, UBUQ_LKP_Degree>();
            CreateMap<LKP_FieldOfStudy, UBUQ_LKP_FieldOfStudy>();
            CreateMap<Experience, UBUQ_Experience>();
            CreateMap<BlogPost, UBUQ_BlogPost>();
            CreateMap<SocialLink, UBUQ_SocialLink>();
            CreateMap<UserLanguage, UBUQ_UserLanguage>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LKP_Language))
                .ForMember(dest => dest.LanguageProficiency, opt => opt.MapFrom(src => src.LKP_LanguageProficiency));
            CreateMap<LKP_Language, UBUQ_LKP_Language>();
            CreateMap<LKP_LanguageProficiency, UBUQ_LKP_Language_Proficiency>();

        }
    }
}
