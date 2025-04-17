using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.RoleCommands
{
    public class DeleteRoleCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
