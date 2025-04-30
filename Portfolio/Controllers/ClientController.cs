using Application.Client.Commands;
using Application.Client.Queries;
using Application.Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class ClientController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserList([FromQuery] UserListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserByUsername([FromQuery] UserByUsernameQuery request)
            => Result.HandleResult(await Mediator.Send(request));
    }
}
