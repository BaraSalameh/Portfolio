using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.EducationQueries
{
    public class LKP_DegreeListQuery : IRequest<ListQueryResponse<LKP_DLQ_Response>> { }

    public class LKP_DLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
    }
}
