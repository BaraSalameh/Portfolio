using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Admin.Commands.LKP_TechnologyCommands;
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
        public async Task<IActionResult> AddEditLKP_Language(AddEditLKP_LanguageCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_Language(DeleteLKP_LanguageCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLKP_LanguageProficiency(AddEditLKP_LanguageProficiencyCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_LanguageProficiency(DeleteLKP_LanguageProficiencyCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLKP_Technology(AddEditLKP_TechnologyCommand request) => Ok(await Mediator.Send(request));
        
        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_Technology(DeleteLKP_TechnologyCommand request) => Ok(await Mediator.Send(request));


    }
}
