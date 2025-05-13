using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.EducationQueries
{
    public class LKP_FieldOfStudyListQuery : IRequest<ListQueryResponse<LKP_FOSLQ_Response>> { }

    public class LKP_FOSLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
