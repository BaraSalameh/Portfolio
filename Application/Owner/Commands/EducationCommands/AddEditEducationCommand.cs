using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.EducationCommands
{
    public class AddEditEducationCommand : IRequest<AbstractViewModel>
    {
        public Guid? ID { get; set; }
        public string Institution { get; set; }
        public string Degree { get; set; }
        public string FieldOfStudy { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Description { get; set; }
    }
}
