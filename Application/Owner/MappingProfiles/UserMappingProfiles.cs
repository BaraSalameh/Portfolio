using Application.Owner.Queries.UserQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
        // UserInfoQuery
            CreateMap<User, UIQ_Response>();

        // UserFullInfoQuery
            CreateMap<User, UFIQ_Response>()
                // -> User
                .ForMember(dest => dest.User,
                    opt => opt.MapFrom(src => src)
                )

                // -> UnreadContactMessageCount
                .ForMember(dest => dest.UnreadContactMessageCount,
                    opt => opt.MapFrom(src => src.LstContactMessages
                        .Count(cm => !cm.IsDeleted && !cm.IsRead)
                    )
                )

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
            CreateMap<User, UFIQ_User>();


            // -> LstProjects
            CreateMap<Project, UFIQ_Project>()
                // -> LstProjects -> Education
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                // -> LstProjects -> Experience
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience))
                // -> LstProjects -> LstSkills
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillProjects.Select(usp => usp.UserSkill).Select(us => us.LKP_Skill)));
            // -> LstProjects -> Education (Already defined)
            // -> LstProjects -> Experience
            CreateMap<Experience, UFIQ_Shared_Experience>();
            // -> LstProjects -> LstSkills
            CreateMap<LKP_Skill, UFIQ_LKP_Skill>();


            // -> LstUserSkills
            CreateMap<UserSkill, UFIQ_UserSkill>()
                // -> LstUserSkills -> Skill
                .ForMember(dest => dest.Skill, opt => opt.MapFrom(src => src.LKP_Skill))
                // -> LstUserSkills -> Education
                .ForMember(dest => dest.LstEducations, opt => opt.MapFrom(src => src.LstEducations.Select(us => us.Education)))
                // -> LstUserSkills -> Experience
                .ForMember(dest => dest.LstExperiences, opt => opt.MapFrom(src => src.LstExperiences.Select(us => us.Experience)))
                // -> LstUserSkills -> Project
                .ForMember(dest => dest.LstProjects, opt => opt.MapFrom(src => src.LstProjects.Select(us => us.Project)))
                // -> LstUserSkills -> Certificate
                .ForMember(dest => dest.LstCertificates, opt => opt.MapFrom(src => src.LstCertificates.Select(us => us.Certificate)));
            // -> LstUserSkills -> Skill (Already defined)
            // -> LstUserSkills -> Education
            CreateMap<Education, UFIQ_Shared_Education>()
                // -> LstUserSkills -> Education -> Institution
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution));
            // -> LstUserSkills -> Experience (Already defined)
            // -> LstUserSkills -> Project
            CreateMap<Project, UFIQ_Shared_Project>();
            // -> LstUserSkills -> Certificate
            CreateMap<Certificate, UFIQ_Shared_Certificate>()
                // -> LstUserSkills -> Certificate -> LKP_Certificate
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate));
            // -> LstUserSkills -> Education -> Institution (Already defined)
            // -> LstUserSkills -> Certificate -> LKP_Certificate
            CreateMap<LKP_Certificate, UFIQ_LKP_Certificate>();


            // -> LstEducations
            CreateMap<Education, UFIQ_Education>()
                // -> LstEducations -> Institution
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution))
                // -> LstEducations -> Degree
                .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.LKP_Degree))
                // -> LstEducations -> FieldOfStudy
                .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.LKP_FieldOfStudy))
                // -> LstEducations -> LstProjects
                .ForMember(dest => dest.LstProjects, opt => opt.MapFrom(src => src.LstProjects))
                // -> LstEducations -> LstSkills
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillEducations.Select(use => use.UserSkill).Select(us => us.LKP_Skill)));
            // -> LstEducations -> Institution
            CreateMap<LKP_Institution, UFIQ_LKP_Institution>();
            // -> LstEducations -> Degree
            CreateMap<LKP_Degree, UFIQ_LKP_Degree>();
            // -> LstEducations -> FieldOfStudy
            CreateMap<LKP_FieldOfStudy, UFIQ_LKP_FieldOfStudy>();
            // -> LstEducations -> LstProjects (Already defined)
            // -> LstEducations -> LstSkills (Already defined)

            // -> LstCertificates
            CreateMap<Certificate, UFIQ_Certificate>()
                // -> LstCertificates -> Certificate
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate))
                // -> LstCertificates -> LstSkills
                .ForMember(dest => dest.LstSkills,  opt => opt.MapFrom(src => src.LstUserSkillCertificates.Select(usc => usc.UserSkill).Select(us => us.LKP_Skill)))
                // -> LstCertificates -> LstCertificateMedias
                .ForMember(dest => dest.LstCertificateMedias, opt => opt.MapFrom(src => src.LstCertificateMedias));
            // -> LstCertificates -> Certificate
            CreateMap<LKP_Certificate, UFIQ_LKP_Certificate>();
            // -> LstCertificates -> LstSkills (Already defined)
            // -> LstCertificates -> LstCertificateMedias
            CreateMap<CertificateMedia, UFIQ_CertificateMedia>();


            // -> LstExperiences
            CreateMap<Experience, UFIQ_Experience>()
                // -> LstExperiences -> LstSkills
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillExperiences.Select(use => use.UserSkill).Select(us => us.LKP_Skill)));
            // -> LstExperiences -> LstSkills (Already defined)

            // -> LstBlogPosts (Not yet built)
            CreateMap<BlogPost, UFIQ_BlogPost>();


            // -> LstSocialLinks (Not yet built)
            CreateMap<SocialLink, UFIQ_SocialLink>();


            // -> LstUserLanguages
            CreateMap<UserLanguage, UFIQ_UserLanguage>()
                // -> LstUserLanguages -> Language
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LKP_Language))
                // -> LstUserLanguages -> LanguageProficiency
                .ForMember(dest => dest.LanguageProficiency, opt => opt.MapFrom(src => src.LKP_LanguageProficiency));
            // -> LstUserLanguages -> Language
            CreateMap<LKP_Language, UFIQ_LKP_Language>();
            // -> LstUserLanguages -> LanguageProficiency
            CreateMap<LKP_LanguageProficiency, UFIQ_LKP_Language_Proficiency>();


            // -> LstUserPreferences
            CreateMap<UserPreference, UFIQ_UserPreference>()
                // -> LstUserPreferences -> Preference
                .ForMember(dest => dest.Preference, opt => opt.MapFrom(src => src.LKP_Preference));
            // -> LstUserPreferences -> Preference
            CreateMap<LKP_Preference, UFIQ_LKP_Preference>();


            // -> LstUserChartPreferences
            CreateMap<UserChartPreference, UFIQ_UserChartPreference>()
                // -> LstUserChartPreferences -> Widget
                .ForMember(dest => dest.Widget, opt => opt.MapFrom(src => src.LKP_Widget))
                // -> LstUserChartPreferences -> ChartType
                .ForMember(dest => dest.ChartType, opt => opt.MapFrom(src => src.LKP_ChartType));
            // -> LstUserChartPreferences -> Widget
            CreateMap<LKP_Widget, UFIQ_LKP_Widget>();
            // -> LstUserChartPreferences -> ChartType
            CreateMap<LKP_ChartType, UFIQ_LKP_ChartType>();
        }
    }
}
