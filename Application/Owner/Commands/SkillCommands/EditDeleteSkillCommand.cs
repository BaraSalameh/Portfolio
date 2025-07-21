using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SkillCommands
{
    public class EditDeleteSkillCommand : IRequest<CommandResponse>
    {
        public List<EDSC_LKP_Skill>? LstSkills { get; set; }
    }

    public class EDSC_LKP_Skill
    {
        public Guid LKP_SkillID { get; set; }
        public int Proficiency { get; set; }
        public string? Description { get; set; }
        public Guid? EducationID { get; set; }
        public Guid? ExperienceID { get; set; }
        public Guid? ProjectID { get; set; }
    }
}
