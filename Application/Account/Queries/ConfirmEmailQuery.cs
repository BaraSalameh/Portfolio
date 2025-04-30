using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries
{
    public class ConfirmEmailQuery : IRequest<CommandResponse>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}
