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


            CreateMap<User, UBUQ_Response>();
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
