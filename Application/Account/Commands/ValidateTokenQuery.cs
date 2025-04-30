using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class ValidateTokenCommand : IRequest<CommandResponse<VTC_Response>>{}

    public class VTC_Response
    {
        public string Username { get; set; }
        public string Role { get; set; }
    }
}
