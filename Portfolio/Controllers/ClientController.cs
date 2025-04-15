using Application.Client.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class ClientController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailCommand request) => Ok(await Mediator.Send(request));
    }
}
