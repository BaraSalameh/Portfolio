using Application.Account.Commands;
using Application.Account.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class AccountController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] LoginQuery request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> ActiviateDeactivateUser(ActivateDeactivateUserCommand request) => Ok(await Mediator.Send(request));
    }
}