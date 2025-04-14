using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.RoleCommands
{
    public class AddEditRoleCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
    }
}
