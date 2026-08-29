using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Admin.Commands.LKP_PreferenceCommands
{
    public class AddEditLKP_PreferenceCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(100), RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
        public string Name { get; set; } = string.Empty;
    }
}
