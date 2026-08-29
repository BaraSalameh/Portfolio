using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.UserPreferenceCommands
{
    public class EditUserPreferenceCommand : IRequest<CommandResponse>
    {
        public Guid LKP_PreferenceID { get; set; }
        [Required, StringLength(1000)]
        public string Value { get; set; } = string.Empty;
    }
}
