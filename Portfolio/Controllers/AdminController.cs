using Application.Admin.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddEditLanguageLevel(AddEditLanguageLevelCommand request) => Ok(await Mediator.Send(request));
    }
}
