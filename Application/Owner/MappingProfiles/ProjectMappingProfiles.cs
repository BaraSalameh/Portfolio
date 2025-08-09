using Application.Owner.Commands.ProjectCommands;
using Application.Owner.Queries.ProjectQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ProjectMappingProfiles : Profile
    {
        public ProjectMappingProfiles()
        {
            CreateMap<AddEditProjectCommand, Project>()
                .ForMember(dest => dest.LstUserSkillProjects, opt => opt.Ignore());

            CreateMap<Project, PLQ_Response>()
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillProjects.Select(usp => usp.UserSkill).Select(us => us.LKP_Skill)))
                .ForMember(dest => dest.Education, opt => opt.MapFrom(src => src.Education))
                .ForMember(dest => dest.Experience, opt => opt.MapFrom(src => src.Experience));
            CreateMap<LKP_Skill, PLQ_Skill>();
            CreateMap<Education, PLQ_PS_Education>()
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution));
            CreateMap<Experience, PLQ_PS_Experience>();
            CreateMap<LKP_Institution, PLQ_LKP_Institution>();
        }
    }
}
