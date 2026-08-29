namespace Domain.Entities
{
    public class UserSkillEducation
    {
        public Guid UserSkillID { get; set; }
        public UserSkill UserSkill { get; set; } = null!;

        public Guid EducationID { get; set; }
        public Education Education { get; set; } = null!;
    }
}
