using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.EducationCommands
{
    public class AddEditEducationCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public int EducationLevelID { get; set; }
        public string? Topic { get; set; }
        public string? Place { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? Describtion { get; set; }
        public int ProfileID { get; set; }
    }
}
