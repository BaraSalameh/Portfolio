using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class RegisterCommand : IRequest<RC_Response>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }

    public class RC_Response : AbstractViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
