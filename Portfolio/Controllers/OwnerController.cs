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
using Application.Owner.Queries.UserQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireOwnerRole")]
    public class OwnerController : ApiController
    {
        [HttpGet]
        public async Task<IActionResult> UserFullInfo([FromQuery] UserQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_LanguageList([FromQuery] LKP_LanguageListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> SignMessage(SignMessageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(DeleteMessageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditSocialLink(AddEditSocialLinkCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteSocialLink(DeleteSocialLinkCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditSkill(AddEditSkillCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteSkill(DeleteSkillCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditExperience(AddEditExperienceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteExperience(DeleteExperienceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> EducationList([FromQuery] EducationListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_InstitutionList([FromQuery] LKP_InstitutionListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_DegreeList([FromQuery] LKP_DegreeListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_FieldOfStudyList([FromQuery] LKP_FieldOfStudyListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditEducation(AddEditEducationCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> ReOrderEducation(ReOrderEducationCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteEducation(DeleteEducationCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditBlogPost(AddEditBlogPostCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteBlogPost(DeleteBlogPostCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> EditDeleteUserLanguage(EditDeleteUserLanguageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditDeleteProjectTechnology(AddEditDeleteProjectTechnologyCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(DeleteProjectCommand request)
            => Result.HandleResult(await Mediator.Send(request));
    }
}
