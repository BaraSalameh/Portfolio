using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Admin.Commands.LKP_LanguageProficiencyCommands
{
    public class AddEditLKP_LanguageProficiencyCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(100)]
        public string Level { get; set; } = string.Empty;
    }
}
