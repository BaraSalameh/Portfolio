using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ProjectTechnologyQueries
{
    public class ProjectTechnologyListQuery : IRequest<ListQueryResponse<PTLQ_Response>> { }

    public class PTLQ_Response
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public List<PTLQ_Technology> LstTechnologies { get; set; }
    }

    public class PTLQ_Technology
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
