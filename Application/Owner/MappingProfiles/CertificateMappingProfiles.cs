using Application.Owner.Commands.CertificaeCommands;
using Application.Owner.Queries.CertificateQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class CertificateMappingProfiles : Profile
    {
        public CertificateMappingProfiles()
        {
            CreateMap<AddEditDeleteCertificateCommand, Certificate>()
                .ForMember(dest => dest.LstUserSkills, opt => opt.MapFrom(src =>
                    (src.LstSkills ?? new List<Guid>()).Select(id => new UserSkill
                    {
                        CertificateID = id
                    }).ToList()
                ))
                .ForMember(dest => dest.LstCertificateMedias, opt => opt.MapFrom(src =>
                    (src.LstCertificateMedias ?? new List<string>()).Select(url => new CertificateMedia
                    {
                        Url = url
                    }).ToList()
                ));

            CreateMap<Certificate, CLQ_Response>()
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate));
            CreateMap<LKP_Certificate, CLQ_LKP_Certificate>();

            CreateMap<LKP_Certificate, LKP_CLQ_Response>();
        }
    }
}
