using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.EducationQueries
{
    public class EducationListQuery : IRequest<ListQueryResponse<ELQ_Educations>> { }

    public class ELQ_Educations
    {
        public Guid ID { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Description { get; set; }
    }
}
