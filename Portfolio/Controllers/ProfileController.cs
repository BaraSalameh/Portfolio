using Application.CV.Commands.EducationCommands;
using Application.CV.Commands.HeaderCommands;
using Application.CV.Commands.LanguageCommands;
using Application.CV.Commands.LinkCommands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireOwnerRole")]
    public class ProfileController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> AddEditHeader(AddEditHeaderCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteHeader(DeleteHeaderCommand request) => Ok(await Mediator.Send(request));


        [HttpPost]
        public async Task<IActionResult> AddEditEducation(AddEditEducationCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteEducation(DeleteEducationCommand request) => Ok(await Mediator.Send(request));


        [HttpPost]
        public async Task<IActionResult> AddEditLanguage(AddEditLanguageCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLanguage(DeleteLanguageCommand request) => Ok(await Mediator.Send(request));


        [HttpPost]
        public async Task<IActionResult> AddEditLink(AddEditLinkCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLink(DeleteLinkCommand request) => Ok(await Mediator.Send(request));
    }
}
