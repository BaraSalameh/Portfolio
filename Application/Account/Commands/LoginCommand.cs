using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class LoginCommand : IRequest<CommandResponse<LC_Response>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LC_Response
    {
        public string Username { get; set; }
        public string Role { get; set; }
        //public bool IsConfirmed { get; set; }
    }
}