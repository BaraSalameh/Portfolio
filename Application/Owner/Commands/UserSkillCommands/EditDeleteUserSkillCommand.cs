using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserSkillCommands
{
    public class EditDeleteUserSkillCommand : IRequest<CommandResponse>
    {
        public List<EDUSC_LKP_Skill>? LstUserSkills { get; set; }
    }

    public class EDUSC_LKP_Skill
    {
        public Guid LKP_SkillID { get; set; }
        public Guid? EducationID { get; set; }
        public Guid? ExperienceID { get; set; }
        public Guid? ProjectID { get; set; }
        public int Proficiency { get; set; }
        public string? Description { get; set; }
    }
}
