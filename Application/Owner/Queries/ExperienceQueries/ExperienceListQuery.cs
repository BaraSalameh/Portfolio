using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ExperienceQueries
{
    public class ExperienceListQuery : OwnerCollectionQuery<ELQ_Response> { }

    public class ELQ_Response
    {
        public Guid ID { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? Description { get; set; }
        public List<ELQ_LKP_Skill> LstSkills { get; set; } = [];
    }

    public class ELQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }
}
