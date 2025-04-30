using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.RoleCommands
{
    public class DeleteRoleCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
