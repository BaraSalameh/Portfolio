using Application.Admin.Commands.EducationLevelCommands;
using Application.Admin.Commands.LanguageLevelCommands;
using Application.Admin.Commands.RoleCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class AdminController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddEditRole(AddEditRoleCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(DeleteRoleCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLanguageLevel(AddEditLanguageLevelCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLanguageLevel(DeleteLanguageLevelCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditEducationLevel(AddEditEducationLevelCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteEducationLevel(DeleteEducationLevelCommand request) => Ok(await Mediator.Send(request));


    }
}
