using Application.Account.Commands;
using Application.Account.Queries;
using Application.Common.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Logout(LogoutCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> ValidateToken(ValidateTokenCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> ResendConfirmEmail([FromQuery] ResendConfirmEmailQuery request)
            => Result.HandleResult(await Mediator.Send(request));
    }
}