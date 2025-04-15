using Application.Owner.Commands.ContactMessageCommands;
using Application.Owner.Commands.LanguageCommands;
using Application.Owner.Commands.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireOwnerRole")]
    public class OwnerController : ApiController
    {
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> SignMessage(SignMessageCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(DeleteMessageCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditLanguage(AddEditLanguageCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteLanguage(DeleteLanguageCommand request) => Ok(await Mediator.Send(request));
    }
}
