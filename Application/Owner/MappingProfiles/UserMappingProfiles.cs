using Application.Owner.Queries.UserQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<User, UIQ_Response>();

            CreateMap<User, UFIQ_Response>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.UnreadContactMessageCount,
                    opt => opt.MapFrom(src => src.LstContactMessages
                        .Count(cm => !cm.IsDeleted && !cm.IsRead)
                    ))
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
            CreateMap<User, UFIQ_User>();
            CreateMap<Project, UFIQ_Project>()
                .ForMember(dest => dest.LstTechnologies,
                    opt => opt.MapFrom(src => src.LstProjectTechnologies.Select(pt => pt.LKP_Technology))
                );
            CreateMap<LKP_Technology, UFIQ_LKP_Technology>();
            CreateMap<Skill, UFIQ_Skill>();
            CreateMap<Education, UFIQ_Education>()
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution))
                .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.LKP_Degree))
                .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.LKP_FieldOfStudy));
            CreateMap<LKP_Institution, UFIQ_LKP_Institution>();
            CreateMap<LKP_Degree, UFIQ_LKP_Degree>();
            CreateMap<LKP_FieldOfStudy, UFIQ_LKP_FieldOfStudy>();
            CreateMap<Experience, UFIQ_Experience>();
            CreateMap<BlogPost, UFIQ_BlogPost>();
            CreateMap<SocialLink, UFIQ_SocialLink>();
            CreateMap<UserLanguage, UFIQ_UserLanguage>()
                .ForMember(dest => dest.Language, opt => opt.MapFrom(src => src.LKP_Language))
                .ForMember(dest => dest.LanguageProficiency, opt => opt.MapFrom(src => src.LKP_LanguageProficiency));
            CreateMap<LKP_Language, UFIQ_LKP_Language>();
            CreateMap<LKP_LanguageProficiency, UFIQ_LKP_Language_Proficiency>();

        }
    }
}
