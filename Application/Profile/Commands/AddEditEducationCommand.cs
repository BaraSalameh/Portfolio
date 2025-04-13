using Application.Common.Entities;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Profile.Commands
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
