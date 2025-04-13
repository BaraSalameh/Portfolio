using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries.LoginQueries
{
    public class LoginQuery : IRequest<AbstractViewModel>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}