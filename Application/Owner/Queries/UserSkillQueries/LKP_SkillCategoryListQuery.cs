using Application.Common.Entities;

namespace Application.Owner.Queries.UserSkillQueries
{
    public class LKP_SkillCategoryListQuery : ListQuery<LKP_SCLQ_Response> { }

    public class LKP_SCLQ_Response
    {
        public Guid? ID { get; set; }
        public string name { get; set; }
    }
}
