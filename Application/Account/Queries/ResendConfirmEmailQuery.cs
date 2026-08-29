using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Account.Queries
{
    public class ResendConfirmEmailQuery : IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Username { get; set; } = string.Empty;
    }
}
