using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.RoleCommands
{
    public class DeleteRoleCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
