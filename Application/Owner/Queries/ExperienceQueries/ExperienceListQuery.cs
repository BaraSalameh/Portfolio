using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ExperienceQueries
{
    public class ExperienceListQuery : IRequest<ListQueryResponse<ELQ_Response>> { }

    public class ELQ_Response
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
    }
}
