using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class ValidateTokenCommand : IRequest<VTC_Response>
    {
    }

    public class VTC_Response : AbstractViewModel
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
