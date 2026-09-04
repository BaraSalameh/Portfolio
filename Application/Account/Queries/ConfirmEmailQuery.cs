using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Account.Queries
{
    public class ConfirmEmailQuery : IRequest<CommandResponse<Application.Account.Commands.LC_Response>>
    {
        [Required, StringLength(256)]
        public string Token { get; set; } = string.Empty;
    }
}
