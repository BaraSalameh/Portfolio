using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserPreferenceCommands
{
    public class EditUserPreferenceCommand : IRequest<CommandResponse>
    {
        public Guid LKP_PreferenceID { get; set; }
        public string Value { get; set; }
    }
}
