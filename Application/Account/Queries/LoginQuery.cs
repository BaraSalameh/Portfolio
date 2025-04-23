using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries
{
    public class LoginQuery : IRequest<LQ_Response>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LQ_Response : AbstractViewModel
    {
        public string Username { get; set; }
    }
}