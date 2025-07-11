using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Admin.Commands.LKP_PreferenceCommands;
using Application.Admin.Commands.LKP_TechnologyCommands;
using Application.Admin.Commands.RoleCommands;
using Application.Common.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfolio.Filters;

namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    [NormalizeStrings()]
    public class AdminController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddEditRole(AddEditRoleCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(DeleteRoleCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLKP_Language(AddEditLKP_LanguageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_Language(DeleteLKP_LanguageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLKP_LanguageProficiency(AddEditLKP_LanguageProficiencyCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_LanguageProficiency(DeleteLKP_LanguageProficiencyCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLKP_Technology(AddEditLKP_TechnologyCommand request)
            => Result.HandleResult(await Mediator.Send(request));
        
        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_Technology(DeleteLKP_TechnologyCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLKP_Preference(AddEditLKP_PreferenceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLKP_Preference(DeleteLKP_PreferenceCommand request)
            => Result.HandleResult(await Mediator.Send(request));
    }
}
