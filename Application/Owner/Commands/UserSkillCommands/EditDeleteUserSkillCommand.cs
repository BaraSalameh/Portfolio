using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserSkillCommands
{
    public class EditDeleteUserSkillCommand : IRequest<CommandResponse>
    {
        public List<EDUSC_UserSkill>? LstUserSkills { get; set; }
    }

    public class EDUSC_UserSkill
    {
        public Guid LKP_SkillID { get; set; }
        public List<Guid>? EducationIDs { get; set; }
        public List<Guid>? ExperienceIDs { get; set; }
        public List<Guid>? ProjectIDs { get; set; }
        public List<Guid>? CertificateIDs { get; set; }
    }
}
