using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries
{
    public class ConfirmEmailQuery : IRequest<CEQ_Response>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

    public class CEQ_Response : AbstractViewModel
    {
        public bool IsConfirmed { get; set; }
    }
}
