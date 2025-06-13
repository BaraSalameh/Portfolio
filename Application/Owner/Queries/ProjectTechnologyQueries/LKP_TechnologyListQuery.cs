using Application.Common.Entities;

namespace Application.Owner.Queries.ProjectTechnologyQueries
{
    public class LKP_TechnologyListQuery : ListQuery<LKP_TLQ_Response> { }

    public class LKP_TLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
