using Application.Common.Entities;
using Domain.Entities;
using Application.Owner.Queries.EducationQueries;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.EducationCommands
{
    public class AddEditEducationCommand : IRequest<CommandResponse<ELQ_Educations>>
    {
        public Guid? ID { get; set; }
        public Guid LKP_InstitutionID { get; set; }
        public Guid LKP_DegreeID { get; set; }
        public Guid LKP_FieldOfStudyID { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        [StringLength(5000)]
        public string? Description { get; set; }
        [MaxLength(100)]
        public List<Guid>? LstSkills { get; set; }
    }
}
