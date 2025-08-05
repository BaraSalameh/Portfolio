using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ProjectQueries
{
    public class ProjectListQuery : IRequest<ListQueryResponse<PLQ_Response>> { }

    public class PLQ_Response
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public PLQ_PS_Education Education { get; set; }
        public PLQ_PS_Experience Experience { get; set; }
        public List<PLQ_Skill> LstSkills { get; set; }
    }

    public class PLQ_PS_Education
    {
        public Guid ID { get; set; }
        public PLQ_LKP_Institution Institution { get; set; }
    }

    public class PLQ_PS_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
    }

    public class PLQ_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class PLQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }
}
