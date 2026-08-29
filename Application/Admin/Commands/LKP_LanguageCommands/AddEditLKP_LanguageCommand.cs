using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Admin.Commands.LKP_LanguageCommands
{
    public class AddEditLKP_LanguageCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(100), RegularExpression("^[A-Za-z][A-Za-z '-]*$")]
        public string Name { get; set; } = string.Empty;
    }
}
