using Application.Account.Commands;
using Application.Account.Queries;
using Application.Common.Entities;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Authorization;
using Portfolio.Http;

namespace Portfolio.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        public AccountController(IMediator mediator) : base(mediator) { }

        [HttpPost]
        [EnableRateLimiting("authentication")]
        public async Task<IActionResult> Login(LoginCommand request)
            => Result.HandleResult(await Send(request));

        [HttpPost]
        [EnableRateLimiting("authentication")]
        public async Task<IActionResult> Register(RegisterCommand request)
            => Result.HandleResult(await Send(request));

        [HttpPost]
        [EnableRateLimiting("authentication")]
        public async Task<IActionResult> Logout(LogoutCommand request)
            => Result.HandleResult(await Send(request));

        [HttpPost]
        [EnableRateLimiting("authentication")]
        public async Task<IActionResult> ValidateToken(ValidateTokenCommand request)
            => Result.HandleResult(await Send(request));

        [HttpGet]
        [EnableRateLimiting("authentication")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailQuery request)
            => Result.HandleResult(await Send(request));

        [HttpGet]
        [EnableRateLimiting("authentication")]
        [RequireTrustedBrowserOrigin]
        public async Task<IActionResult> ResendConfirmEmail([FromQuery] ResendConfirmEmailQuery request)
            => Result.HandleResult(await Send(request));
    }
}
