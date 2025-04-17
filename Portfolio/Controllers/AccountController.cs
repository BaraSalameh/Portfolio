using Application.Account.Commands;
using Application.Account.Queries;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class AccountController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> Login(LoginQuery request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> Register(RegisterCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> ActiviateDeactivateUser(ActivateDeactivateUserCommand request) => Ok(await Mediator.Send(request));
    }
}