using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_PreferenceCommands
{
    public class DeleteLKP_PreferenceCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
