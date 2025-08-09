using Application.Owner.Commands.EducationCommands;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class EducationMappingProfiles : Profile
    {
        public EducationMappingProfiles()
        {
            CreateMap<AddEditEducationCommand, Education>()
                .ForMember(dest => dest.LstUserSkillEducations, opt => opt.Ignore());

            CreateMap<Education, ELQ_Educations>()
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution))
                .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.LKP_Degree))
                .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.LKP_FieldOfStudy))
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillEducations.Select(use => use.UserSkill).Select(us => us.LKP_Skill)));
            CreateMap<LKP_Institution, ELQ_LKP_Institution>();
            CreateMap<LKP_Degree, ELQ_LKP_Degree>();
            CreateMap<LKP_FieldOfStudy, ELQ_LKP_FieldOfStudy>();
            CreateMap<LKP_Skill, ELQ_Skill>();

            CreateMap<LKP_Institution, LKP_ILQ_Response>();

            CreateMap<LKP_Degree, LKP_DLQ_Response>();

            CreateMap<LKP_FieldOfStudy, LKP_FOSLQ_Response>();
        }
    }
}
