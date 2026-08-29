using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ProjectQueries
{
    public class ProjectListQuery : OwnerCollectionQuery<PLQ_Response> { }

    public class PLQ_Response
    {
        public Guid ID { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string LiveLink { get; set; } = string.Empty;
        public string SourceCode { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsFeatured { get; set; }
        public PLQ_PS_Education Education { get; set; } = null!;
        public PLQ_PS_Experience Experience { get; set; } = null!;
        public List<PLQ_Skill> LstSkills { get; set; } = [];
    }

    public class PLQ_PS_Education
    {
        public Guid ID { get; set; }
        public PLQ_LKP_Institution Institution { get; set; } = null!;
    }

    public class PLQ_PS_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; } = string.Empty;
    }

    public class PLQ_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    public class PLQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Logo { get; set; }
    }
}
