using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ExperienceCommands
{
    public class AddEditExperienceCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Location { get; set; }
        public string? Description { get; set; }
    }
}
