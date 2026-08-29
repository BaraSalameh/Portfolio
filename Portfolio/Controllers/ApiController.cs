using MediatR;
using Microsoft.AspNetCore.Mvc;
using Asp.Versioning;

namespace Portfolio.Controllers
{
    [ApiVersion(1.0)]
    [Route("api/[controller]/[action]")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        private readonly IMediator _mediator;

        protected ApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected Task<TResponse> Send<TResponse>(IRequest<TResponse> request) =>
            _mediator.Send(request, HttpContext.RequestAborted);
    }
}
