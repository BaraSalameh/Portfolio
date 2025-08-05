using Application.Common.Entities;
using Application.Owner.Commands.BlogPostCommands;
using Application.Owner.Commands.CertificaeCommands;
using Application.Owner.Commands.ContactMessageCommands;
using Application.Owner.Commands.EducationCommands;
using Application.Owner.Commands.ExperienceCommands;
using Application.Owner.Commands.Profile;
using Application.Owner.Commands.ProjectCommands;
using Application.Owner.Commands.SocialLinkCommands;
using Application.Owner.Commands.UserChartPreferenceCommands;
using Application.Owner.Commands.UserLanguageCommands;
using Application.Owner.Commands.UserPreferenceCommands;
using Application.Owner.Commands.UserSkillCommands;
using Application.Owner.Queries.CertificateQueries;
using Application.Owner.Queries.ContactMessageQueries;
using Application.Owner.Queries.EducationQueries;
using Application.Owner.Queries.ExperienceQueries;
using Application.Owner.Queries.LKP_LanguageQuieries;
using Application.Owner.Queries.ProjectQueries;
using Application.Owner.Queries.UserChartPreferenceQueries;
using Application.Owner.Queries.UserLanguageQueries;
using Application.Owner.Queries.UserPreferenceQueries;
using Application.Owner.Queries.UserQueries;
using Application.Owner.Queries.UserSkillQueries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Portfolio.Controllers
{
    [Authorize(Policy = "RequireOwnerRole")]
    public class OwnerController : ApiController
    {
        // User
        [HttpGet]
        public async Task<IActionResult> UserFullInfo([FromQuery] UserFullInfoQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserInfo([FromQuery] UserInfoQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        // UserLanguage
        [HttpGet]
        public async Task<IActionResult> UserLanguageList([FromQuery] UserLanguageListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_LanguageList([FromQuery] LKP_LanguageListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_LanguageProficiencyList([FromQuery] LKP_LanguageProficiencyListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> EditDeleteUserLanguage(EditDeleteUserLanguageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // Profile
        [HttpPost]
        public async Task<IActionResult> EditProfile(EditProfileCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // Message
        [HttpGet]
        public async Task<IActionResult> ContactMessageList([FromQuery] ContactMessageListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> SignMessage(SignMessageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteMessage(DeleteMessageCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // SocialLink
        [HttpPost]
        public async Task<IActionResult> AddEditSocialLink(AddEditSocialLinkCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteSocialLink(DeleteSocialLinkCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // Skill
        [HttpPost]
        public async Task<IActionResult> EditDeleteUserSkill(EditDeleteUserSkillCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserSkillList([FromQuery] UserSkillListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_SkillList([FromQuery] LKP_SkillListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        // Experience
        [HttpGet]
        public async Task<IActionResult> ExperienceList([FromQuery] ExperienceListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditExperience(AddEditExperienceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteExperience(DeleteExperienceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> SortExperience(SortExperienceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // Education
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

        [HttpDelete]
        public async Task<IActionResult> DeleteEducation(DeleteEducationCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> ReOrderEducation(ReOrderEducationCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // Certificate
        [HttpGet]
        public async Task<IActionResult> CertificateList([FromQuery] CertificateListQuery request)
            => Result.HandleResult(await Mediator.Send(request));
        [HttpGet]
        public async Task<IActionResult> LKP_CertificateList([FromQuery] LKP_CertificateListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditCertificate(AddEditCertificateCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteCertificate(DeleteCertificateCommand request)
            => Result.HandleResult(await Mediator.Send(request));
        [HttpPost]
        public async Task<IActionResult> SortCertificate(SortCertificateCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // BlogPost
        [HttpPost]
        public async Task<IActionResult> AddEditBlogPost(AddEditBlogPostCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteBlogPost(DeleteBlogPostCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // Project
        [HttpGet]
        public async Task<IActionResult> ProjectList([FromQuery] ProjectListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpPost]
        public async Task<IActionResult> AddEditProject(AddEditProjectCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpDelete]
        public async Task<IActionResult> DeleteProject(DeleteProjectCommand request)
            => Result.HandleResult(await Mediator.Send(request));
        [HttpPost]
        public async Task<IActionResult> SortProject(SortProjectCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        // UserPreferences
        [HttpPost]
        public async Task<IActionResult> EditUserPreference(EditUserPreferenceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserPreferenceList([FromQuery] UserPreferenceListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_PreferenceList([FromQuery] LKP_PreferenceListQurery request)
            => Result.HandleResult(await Mediator.Send(request));

        // UserChartPreferences
        [HttpPost]
        public async Task<IActionResult> EditUserChartPreference(EditUserChartPreferenceCommand request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> UserChartPreferenceList([FromQuery] UserChartPreferenceListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_WidgetList([FromQuery] LKP_WidgetListQuery request)
            => Result.HandleResult(await Mediator.Send(request));

        [HttpGet]
        public async Task<IActionResult> LKP_ChartTypeList([FromQuery] LKP_ChartTypeListQuery request)
            => Result.HandleResult(await Mediator.Send(request));
    }
}
