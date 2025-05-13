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
            CreateMap<Education, ELQ_Educations>();
            CreateMap<LKP_Institution, LKP_ILQ_Response>();
            CreateMap<LKP_Degree, LKP_DLQ_Response>();
            CreateMap<LKP_FieldOfStudy, LKP_FOSLQ_Response>();
        }
    }
}
