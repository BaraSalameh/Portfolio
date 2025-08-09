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
            CreateMap<AddEditCertificateCommand, Certificate>()
                .ForMember(dest => dest.LstUserSkillCertificates, opt => opt.Ignore())
                .ForMember(dest => dest.LstCertificateMedias, opt => opt.MapFrom(src =>
                    (src.LstCertificateMedias ?? new List<string>()).Select(url => new CertificateMedia
                    {
                        Url = url
                    }).ToList()
                ));

            CreateMap<Certificate, CLQ_Response>()
                .ForMember(dest => dest.Certificate, opt => opt.MapFrom(src => src.LKP_Certificate))
                .ForMember(dest => dest.LstSkills, opt => opt.MapFrom(src => src.LstUserSkillCertificates.Select(usc => usc.UserSkill).Select(us => us.LKP_Skill)))
                .ForMember(dest => dest.LstCertificateMedias, opt => opt.MapFrom(src => src.LstCertificateMedias));
            CreateMap<LKP_Certificate, CLQ_LKP_Certificate>();
            CreateMap<LKP_Skill, CLQ_Skill>();
            CreateMap<CertificateMedia, CLQ_CertificateMedia>();

            CreateMap<LKP_Certificate, LKP_CLQ_Response>();
        }
    }
}
