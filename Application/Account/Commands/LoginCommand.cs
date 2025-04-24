using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class LoginCommand : IRequest<LC_Response>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }

    public class LC_Response : AbstractViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}