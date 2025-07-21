using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.SkillQueries
{
    public class SkillListQuery : IRequest<ListQueryResponse<SLQ_Response>> { }

    public class SLQ_Response
    {
        public SLQ_LKP_Skill Skill { get; set; }
        public int Proficiency { get; set; }
        public string? Description { get; set; }
        public SLQ_PS_Education Education { get; set; }
        public SLQ_PS_Experience Experience { get; set; }
        public SLQ_Project Project { get; set; }
    }

    public class SLQ_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class SLQ_PS_Education
    {
        public Guid ID { get; set; }
        public SLQ_LKP_Institution Institution { get; set; }
    }

    public class SLQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class SLQ_PS_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
    }

    public class SLQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public SLQ_LKP_SkillCategory SkillCategory { get; set; }
    }

    public class SLQ_LKP_SkillCategory
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
