using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries
{
    public class ResendConfirmEmailQuery : IRequest<AbstractViewModel>
    {
        public string Email { get; set; }
    }
}
