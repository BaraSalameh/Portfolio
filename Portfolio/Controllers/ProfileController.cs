using Application.Profile.Commands;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    public class ProfileController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddEditEducation(AddEditEducationCommand request) => Ok(await Mediator.Send(request));
    }
}
