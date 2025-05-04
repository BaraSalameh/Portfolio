using Application.Client.Queries;
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
            CreateMap<Project, UBUQ_Project>();
            CreateMap<ProjectTechnology, UBUQ_ProjectTechnology>();
            CreateMap<LKP_Technology, UBUQ_LKP_Technology>();
            CreateMap<Skill, UBUQ_Skill>();
            CreateMap<Education, UBUQ_Education>();
            CreateMap<Experience, UBUQ_Experience>();
            CreateMap<BlogPost, UBUQ_BlogPost>();
            CreateMap<SocialLink, UBUQ_SocialLink>();
            CreateMap<UserLanguage, UBUQ_UserLanguage>();
            CreateMap<LKP_Language, UBUQ_LKP_Language>();
            CreateMap<LKP_LanguageProficiency, UBUQ_LKP_Language_Proficiency>();

        }
    }
}
