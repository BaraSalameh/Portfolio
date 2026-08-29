using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.ExperienceCommands
{
    public class AddEditExperienceCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(200)]
        public string JobTitle { get; set; } = string.Empty;
        [Required, StringLength(200)]
        public string CompanyName { get; set; } = string.Empty;
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        [Required, StringLength(300)]
        public string Location { get; set; } = string.Empty;
        [StringLength(5000)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public List<Guid>? LstSkills { get; set; }
    }
}
