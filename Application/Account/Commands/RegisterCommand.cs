using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Account.Commands
{
    public class RegisterCommand : IRequest<CommandResponse>
    {
        [Required, StringLength(100)]
        public string Firstname { get; set; } = string.Empty;
        [Required, StringLength(100)]
        public string Lastname { get; set; } = string.Empty;
        [Required, EmailAddress, StringLength(320)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(256, MinimumLength = 12)]
        public string Password { get; set; } = string.Empty;
        public bool? RememberMe { get; set; } = false;
    }
}
