using Application.Common.Entities;

namespace Application.Owner.Queries.SkillQueries
{
    public class LKP_SkillListQuery : ListQuery<LKP_SLQ_Response> { }

    public class LKP_SLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
        public LKP_SLQ_SkillCategory SkillCategory { get; set; }
    }

    public class LKP_SLQ_SkillCategory
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
