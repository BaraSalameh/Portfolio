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
        public List<ELQ_LKP_Skill> LstSkills { get; set; }
    }

    public class ELQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
