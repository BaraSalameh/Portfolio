﻿using Application.Admin.Commands;
using AutoMapper;
using Domain.Entities;

namespace Application.Profile.MappingProfiles
{
    public class EducationMappingProfiles
    {
        public IMapper AddEditEducationCommandHandler()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddEditLanguageLevelCommand, Education>();
            });
            return config.CreateMapper();
        }
    }
}