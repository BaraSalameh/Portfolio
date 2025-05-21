using Application.Owner.Commands.ProjectTechnologyCommands;
using Application.Owner.Queries.ProjectTechnologyQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class ProjectTechnologyMappingProfiles : Profile
    {
        public ProjectTechnologyMappingProfiles()
        {
            CreateMap<AddEditDeleteProjectTechnologyCommand, Project>()
                .ForMember(dest => dest.LstProjectTechnologies, opt => opt.MapFrom(src =>
                    (src.LstProjectTechnologies ?? new List<Guid>()).Select(id => new ProjectTechnology
                    {
                        LKP_TechnologyID = id
                    }).ToList()
                ));

            CreateMap<Project, PTLQ_Response>()
                .ForMember(dest => dest.LstProjectTechnologies,
                    opt => opt.MapFrom(src => src.LstProjectTechnologies)
                );
            CreateMap<ProjectTechnology, PTLQ_ProjectTechnology>();
            CreateMap<LKP_Technology, PTLQ_LKP_Technology>();

            CreateMap<LKP_Technology, LKP_TLQ_Response>();
        }
    }
}
