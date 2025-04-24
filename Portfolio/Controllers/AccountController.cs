using Application.Account.Commands;
using Application.Account.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Logout(LogoutCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> ValidateToken(ValidateTokenCommand request) => Ok(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery request) => Ok(await Mediator.Send(request));
    }
}