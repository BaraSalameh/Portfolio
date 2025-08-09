using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.EducationQueries
{
    public class EducationListQuery : IRequest<ListQueryResponse<ELQ_Educations>> { }

    public class ELQ_Educations
    {
        public Guid ID { get; set; }
        public ELQ_LKP_Institution Institution { get; set; }
        public ELQ_LKP_Degree Degree { get; set; }
        public ELQ_LKP_FieldOfStudy FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Description { get; set; }
        public List<ELQ_Skill> LstSkills { get; set; }
    }

    public class ELQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class ELQ_LKP_Degree
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Abbreviation { get; set; }
    }

    public class ELQ_LKP_FieldOfStudy
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class ELQ_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
