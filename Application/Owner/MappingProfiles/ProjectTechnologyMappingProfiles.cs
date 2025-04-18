﻿using Application.Owner.Commands.ProjectTechnologyCommands;
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
        }
    }
}
