using Application.Common.Entities;
using Application.Owner.Commands.BlogPostCommands;
using Application.Owner.Commands.ContactMessageCommands;
using Application.Owner.Commands.EducationCommands;
using Application.Owner.Commands.ExperienceCommands;
using Application.Owner.Commands.Profile;
using Application.Owner.Commands.ProjectTechnologyCommands;
using Application.Owner.Commands.SkillCommands;
using Application.Owner.Commands.SocialLinkCommands;
using Application.Owner.Commands.UserLanguageCommands;
using Application.Owner.Queries.EducationQueries;
using Application.Owner.Queries.LKP_LanguageQuieries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireOwnerRole")]
    public class OwnerController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> LKP_LanguageList([FromQuery]  ListQuery<LKP_LanguageListQuery> request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> SignMessage(SignMessageCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(DeleteMessageCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditSocialLink(AddEditSocialLinkCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteSocialLink(DeleteSocialLinkCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditSkill(AddEditSkillCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteSkill(DeleteSkillCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditExperience(AddEditExperienceCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteExperience(DeleteExperienceCommand request) => Ok(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> EducationList([FromQuery] EducationListQuery request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditEducation(AddEditEducationCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteEducation(DeleteEducationCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditBlogPost(AddEditBlogPostCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteBlogPost(DeleteBlogPostCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> EditDeleteUserLanguage(EditDeleteUserLanguageCommand request) => Ok(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditDeleteProjectTechnology(AddEditDeleteProjectTechnologyCommand request) => Ok(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(DeleteProjectCommand request) => Ok(await Mediator.Send(request));
    }
}
