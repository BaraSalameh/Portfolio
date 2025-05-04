using Application.Owner.Queries.UserQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserMappingProfiles : Profile
    {
        public UserMappingProfiles()
        {
            CreateMap<User, UQ_Response>()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.LstEducations,
                    opt => opt.MapFrom(src => src.LstEducations.Where(e => e.IsDeleted == false)))
                .ForMember(dest => dest.LstExperiences,
                    opt => opt.MapFrom(src => src.LstExperiences.Where(e => e.IsDeleted == false)))
                .ForMember(dest => dest.LstSkills,
                    opt => opt.MapFrom(src => src.LstSkills.Where(s => s.IsDeleted == false)))
                .ForMember(dest => dest.LstProjects,
                    opt => opt.MapFrom(src => src.LstProjects.Where(p => p.IsDeleted == false)))
                .ForMember(dest => dest.LstBlogPosts,
                    opt => opt.MapFrom(src => src.LstBlogPosts.Where(p => p.IsDeleted == false)))
                .ForMember(dest => dest.LstSocialLinks,
                    opt => opt.MapFrom(src => src.LstSocialLinks.Where(l => l.IsDeleted == false)));
            CreateMap<User, UQ_User>();
            CreateMap<Project, UQ_Project>();
            CreateMap<ProjectTechnology, UQ_ProjectTechnology>();
            CreateMap<LKP_Technology, UQ_LKP_Technology>();
            CreateMap<Skill, UQ_Skill>();
            CreateMap<Education, UQ_Education>();
            CreateMap<Experience, UQ_Experience>();
            CreateMap<BlogPost, UQ_BlogPost>();
            CreateMap<SocialLink, UQ_SocialLink>();
            CreateMap<UserLanguage, UQ_UserLanguage>();
            CreateMap<LKP_Language, UQ_LKP_Language>();
            CreateMap<LKP_LanguageProficiency, UQ_LKP_Language_Proficiency>();

        }
    }
}
