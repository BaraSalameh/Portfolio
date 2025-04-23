using Application.Common.Entities;
using MediatR;

namespace Application.Client.Queries
{
    public class ValidateTokenQuery : IRequest<VTQ_Response>
    {
    }

    public class VTQ_Response : AbstractViewModel
    {
        public string Username { get; set; }
    }
}
