using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectCommands
{
    public class AddEditProjectCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public List<AEPC_Technology>? LstTechnologies { get; set; }
    }

    public class AEPC_Technology
    {
        public int? ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
