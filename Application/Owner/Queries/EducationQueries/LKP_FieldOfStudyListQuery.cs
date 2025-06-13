using Application.Common.Entities;

namespace Application.Owner.Queries.EducationQueries
{
    public class LKP_FieldOfStudyListQuery : ListQuery<LKP_FOSLQ_Response> { }

    public class LKP_FOSLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
