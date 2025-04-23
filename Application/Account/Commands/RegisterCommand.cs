using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class RegisterCommand : IRequest<AbstractViewModel>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
