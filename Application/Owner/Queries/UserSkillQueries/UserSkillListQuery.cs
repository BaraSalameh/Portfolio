using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserSkillQueries
{
    public class UserSkillListQuery : IRequest<ListQueryResponse<USLQ_Response>> { }

    public class USLQ_Response
    {
        public USLQ_LKP_Skill Skill { get; set; }
        public int Proficiency { get; set; }
        public string? Description { get; set; }
        public USLQ_PS_Education Education { get; set; }
        public USLQ_PS_Experience Experience { get; set; }
        public USLQ_S_Project Project { get; set; }
    }

    public class USLQ_S_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class USLQ_PS_Education
    {
        public Guid ID { get; set; }
        public USLQ_LKP_Institution Institution { get; set; }
    }

    public class USLQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class USLQ_PS_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
    }

    public class USLQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public USLQ_LKP_SkillCategory SkillCategory { get; set; }
    }

    public class USLQ_LKP_SkillCategory
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
