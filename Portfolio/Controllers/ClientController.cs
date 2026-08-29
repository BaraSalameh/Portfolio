using Application.Client.Commands;
using Application.Client.Queries;
using Application.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;

namespace Portfolio.Controllers
{
    [AllowAnonymous]
    public class ClientController : ApiController
    {
        public ClientController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [EnableRateLimiting("contact")]
        public async Task<IActionResult> SendEmail(SendEmailCommand request)
            => Result.HandleResult(await Send(request));

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false, VaryByHeader = "Origin")]
        public async Task<IActionResult> UserList([FromQuery] UserListQuery request)
            => Result.HandleResult(await Send(request));

        [HttpGet]
        [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any, NoStore = false, VaryByHeader = "Origin")]
        public async Task<IActionResult> UserByUsername([FromQuery] UserByUsernameQuery request)
            => Result.HandleResult(await Send(request));
    }
}
