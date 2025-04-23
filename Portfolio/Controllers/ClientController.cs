using Application.Client.Commands;
using Application.Client.Queries;
using Application.Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class ClientController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailCommand request) => Ok(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UsersList([FromQuery] ListQuery<ULQ_Response> request) => Ok(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserByUsername([FromQuery] UserByUsernameQuery request) => Ok(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> ValidateToken([FromQuery] ValidateTokenQuery request) => Ok(await Mediator.Send(request));
    }
}
