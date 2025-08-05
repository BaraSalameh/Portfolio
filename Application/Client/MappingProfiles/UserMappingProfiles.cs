using Application.Client.Queries;
using AutoMapper;
using Domain.Entities;

namespace Application.Client.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
        //UserListQuery
            CreateMap<User, ULQ_Response>();

        //UserByUserNameQuery
            CreateMap<User, UBUQ_Response>()
                // -> User
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))

                // -> LstProjects
                .ForMember(dest => dest.LstProjects,
                    opt => opt.MapFrom(src => src.LstProjects
                        .Where(p => p.IsDeleted == false)
                        .OrderBy(p => p.Order)
                    )
                )

                // -> LstUserSkills
                .ForMember(dest => dest.LstUserSkills,
                    opt => opt.MapFrom(src => src.LstUserSkills
                        .Where(s => s.IsDeleted == false)
                    )
                )

                // -> LstEducations
                .ForMember(dest => dest.LstEducations,
                    opt => opt.MapFrom(src => src.LstEducations
                        .Where(e => e.IsDeleted == false)
                        .OrderBy(e => e.Order)
                    )
                )

                // -> LstCertificates
                .ForMember(dest => dest.LstCertificates,
                    opt => opt.MapFrom(src => src.LstCertificates
                        .Where(e => e.IsDeleted == false)
                        .OrderBy(e => e.Order)
                    )
                )

                // -> LstExperiences
                .ForMember(dest => dest.LstExperiences,
                    opt => opt.MapFrom(src => src.LstExperiences
                        .Where(e => e.IsDeleted == false)
                        .OrderBy(e => e.Order)
                    )
                )

                // -> LstBlogPosts
                .ForMember(dest => dest.LstBlogPosts,
                    opt => opt.MapFrom(src => src.LstBlogPosts
                        .Where(p => p.IsDeleted == false)
                    )
                )

                // -> LstSocialLinks
                .ForMember(dest => dest.LstSocialLinks,
                    opt => opt.MapFrom(src => src.LstSocialLinks
                        .Where(l => l.IsDeleted == false)
                    )
                )

                // -> LstUserPreferences
                .ForMember(dest => dest.LstUserPreferences,
                    opt => opt.MapFrom(src => src.LstUserPreferences
                        .Where(up => up.IsDeleted == false)
                    )
                )

                // -> LstUserChartPreferences
                .ForMember(dest => dest.LstUserChartPreferences,
                    opt => opt.MapFrom(src => src.LstUserChartPreferences
                        .Where(ucp => ucp.IsDeleted == false)
                    )
                );

            // -> User
            CreateMap<User, UBUQ_User>();


            // -> LstProjects
            CreateMap<Project, UBUQ_Project>()
                // -> LstProjects -> Education
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                // -> LstProjects -> Experience
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                // -> LstProjects -> LstSkills
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkills.Select(pt => pt.LKP_Skill)));
            // -> LstProjects -> Education (Already defined)
            // -> LstProjects -> Experience
            CreateMap<Experience, UBUQ_PS_Experience>();
            // -> LstProjects -> LstSkills (Already defined)



            // -> LstUserSkills
            CreateMap<UserSkill, UBUQ_UserSkill>()
                // -> LstUserSkills -> Skill
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.LKP_Skill))
                // -> LstUserSkills -> Education
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                // -> LstUserSkills -> Experience
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                // -> LstUserSkills -> Project
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project))
                // -> LstUserSkills -> Certificate
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.Certificate.LKP_Certificate));
            // -> LstUserSkills -> Skill (Already defined)
            // -> LstUserSkills -> Education
            CreateMap<Education, UBUQ_PS_Education>()
                // -> LstUserSkills -> Education -> Institution
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution));
            // -> LstUserSkills -> Experience (Already defined)
            // -> LstUserSkills -> Project
            CreateMap<Project, UBUQ_S_Project>();
            // -> LstUserSkills -> Certificate
            CreateMap<Certificate, UBUQC_Certificate>()
                // ->LstUserSkills->Certificate->LKP_Certificate
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate));
            // -> LstUserSkills -> Education -> Institution (Already defined)
            // -> LstUserSkills -> Certificate -> LKP_Certificate
            CreateMap<LKP_Certificate, UBUQ_LKP_Certificate>();


            // -> LstEducations
            CreateMap<Education, UBUQ_Education>()
                // -> LstEducations -> Institution
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution))
                // -> LstEducations -> Degree
                .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.LKP_Degree))
                // -> LstEducations -> FieldOfStudy
                .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.LKP_FieldOfStudy));
            // -> LstEducations -> Institution
            CreateMap<LKP_Institution, UBUQ_LKP_Institution>();
            // -> LstEducations -> Degree
            CreateMap<LKP_Degree, UBUQ_LKP_Degree>();
            // -> LstEducations -> FieldOfStudy
            CreateMap<LKP_FieldOfStudy, UBUQ_LKP_FieldOfStudy>();


            // -> LstCertificates
            CreateMap<Certificate, UBUQ_Certificate>()
                // -> LstCertificates -> Certificate
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate))
                // -> LstCertificates -> LstSkills
                .ForMember(dest => dest.LstSkills,
                    opt => opt.MapFrom(src => src.LstUserSkills
                        .Select(pt => pt.LKP_Skill)
                    )
                )
                // -> LstCertificates -> LstCertificateMedias
                .ForMember(dest => dest.LstCertificateMedias,
                    opt => opt.MapFrom(src => src.LstCertificateMedias
                        .Select(pt => pt.Certificate)
                    )
                );
            // -> LstCertificates -> Certificate
            CreateMap<LKP_Certificate, UBUQ_LKP_Certificate>();
            // -> LstCertificates -> LstSkills
            CreateMap<LKP_Skill, UBUQ_Skill>();
            // -> LstCertificates -> LstCertificateMedias
            CreateMap<CertificateMedia, UBUQ_CertificateMedia>();


            // -> LstExperiences
            CreateMap<Experience, UBUQ_Experience>();


            // -> LstBlogPosts (Not yet built)
            CreateMap<BlogPost, UBUQ_BlogPost>();


            // -> LstSocialLinks (Not yet built)
            CreateMap<SocialLink, UBUQ_SocialLink>();


            // -> LstUserLanguages
            CreateMap<UserLanguage, UBUQ_UserLanguage>()
                // -> LstUserLanguages -> Language
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LKP_Language))
                // -> LstUserLanguages -> LanguageProficiency
                .ForMember(dest => dest.LanguageProficiency, opt => opt.MapFrom(src => src.LKP_LanguageProficiency));
            // -> LstUserLanguages -> Language
            CreateMap<LKP_Language, UBUQ_LKP_Language>();
            // -> LstUserLanguages->LanguageProficiency
            CreateMap<LKP_LanguageProficiency, UBUQ_LKP_Language_Proficiency>();


            // -> LstUserPreferences
            CreateMap<UserPreference, UBUQ_UserPreference>()
                // -> LstUserPreferences -> Preference
                .ForMember(dest => dest.Preference, opt => opt.MapFrom(src => src.LKP_Preference));
            // -> LstUserPreferences -> Preference
            CreateMap<LKP_Preference, UBUQ_LKP_Preference>();


            // -> LstUserChartPreferences
            CreateMap<UserChartPreference, UBUQ_UserChartPreference>()
                // -> LstUserChartPreferences -> Widget
                .ForMember(dest => dest.Widget, opt => opt.MapFrom(src => src.LKP_Widget))
                // -> LstUserChartPreferences -> ChartType
                .ForMember(dest => dest.ChartType, opt => opt.MapFrom(src => src.LKP_ChartType));
            // -> LstUserChartPreferences -> Widget
            CreateMap<LKP_Widget, UBUQ_LKP_Widget>();
            // -> LstUserChartPreferences -> ChartType
            CreateMap<LKP_ChartType, UBUQ_LKP_ChartType>();
        }
    }
}
