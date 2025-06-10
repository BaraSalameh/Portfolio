using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.EducationQueries
{
    public class LKP_InstitutionListQuery : ListQuery<LKP_ILQ_Response> { }

    public class LKP_ILQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }
}
