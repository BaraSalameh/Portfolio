using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectTechnologyCommands
{
    public class AddEditDeleteProjectTechnologyCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public Guid? EducationID { get; set; }
        public Guid? ExperienceID { get; set; }
        public List<Guid>? LstTechnologies { get; set; }
    }
}
