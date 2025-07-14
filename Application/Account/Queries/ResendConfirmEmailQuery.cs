using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries
{
    public class ResendConfirmEmailQuery : IRequest<CommandResponse>
    {
        public string Username { get; set; }
    }
}
