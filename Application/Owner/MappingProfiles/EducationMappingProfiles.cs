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
            CreateMap<AddEditEducationCommand, Education>();

            CreateMap<Education, ELQ_Educations>()
                .ForMember(dest => dest.Institution, opt => opt.MapFrom(src => src.LKP_Institution))
                .ForMember(dest => dest.Degree, opt => opt.MapFrom(src => src.LKP_Degree))
                .ForMember(dest => dest.FieldOfStudy, opt => opt.MapFrom(src => src.LKP_FieldOfStudy));
            CreateMap<LKP_Institution, ELQ_LKP_Institution>();
            CreateMap<LKP_Degree, ELQ_LKP_Degree>();
            CreateMap<LKP_FieldOfStudy, ELQ_LKP_FieldOfStudy>();

            CreateMap<LKP_Institution, LKP_ILQ_Response>();

            CreateMap<LKP_Degree, LKP_DLQ_Response>();

            CreateMap<LKP_FieldOfStudy, LKP_FOSLQ_Response>();
        }
    }
}
