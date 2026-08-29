using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Admin.Commands.RoleCommands
{
    public class AddEditRoleCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(100), RegularExpression("^[A-Za-z][A-Za-z0-9]*$")]
        public string Name { get; set; } = string.Empty;
    }
}
