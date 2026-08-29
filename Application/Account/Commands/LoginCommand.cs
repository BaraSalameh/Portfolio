using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Account.Commands
{
    public class LoginCommand : IRequest<CommandResponse<LC_Response>>
    {
        [Required, EmailAddress, StringLength(320)]
        public string Email { get; set; } = string.Empty;
        [Required, StringLength(256, MinimumLength = 8)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }

    public class LC_Response
    {
        public string Username { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
