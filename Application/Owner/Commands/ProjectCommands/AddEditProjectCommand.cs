using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validation;

namespace Application.Owner.Commands.ProjectCommands
{
    public class AddEditProjectCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [StringLength(5000)]
        public string? Description { get; set; }
        [StringLength(2048), Url, HttpUrl]
        public string? LiveLink { get; set; }
        [StringLength(2048), Url, HttpUrl]
        public string? SourceCode { get; set; }
        [StringLength(2048), Url, HttpUrl]
        public string? ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public Guid? EducationID { get; set; }
        public Guid? ExperienceID { get; set; }
        [MaxLength(100)]
        public List<Guid>? LstSkills { get; set; }
    }
}
