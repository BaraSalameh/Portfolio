using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class RestrictPhysicalCascadeDeletes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                table: "BlogPost");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_User_UserID",
                table: "BlogPost");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostTag_BlogPost_BlogPostID",
                table: "BlogPostTag");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostTag_Tag_TagId",
                table: "BlogPostTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_LKP_Certificate_LKP_CertificateID",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_User_UserID",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_CertificateMedia_Certificate_CertificateID",
                table: "CertificateMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessage_User_UserID",
                table: "ContactMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_Degree_LKP_DegreeID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_User_UserID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_User_UserID",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingEmailConfirmation_User_UserID",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Education_EducationID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Experience_ExperienceID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_UserID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_UserID",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialLink_User_UserID",
                table: "SocialLink");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChartPreference_LKP_ChartType_LKP_ChartTypeID",
                table: "UserChartPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChartPreference_LKP_Widget_LKP_WidgetID",
                table: "UserChartPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChartPreference_User_UserID",
                table: "UserChartPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficienc~",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_LKP_Language_LKP_LanguageID",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_User_UserID",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreference_LKP_Preference_LKP_PreferenceID",
                table: "UserPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreference_User_UserID",
                table: "UserPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_User_UserID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillCertificate_Certificate_CertificateID",
                table: "UserSkillCertificate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEducation_Education_EducationID",
                table: "UserSkillEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillExperience_Experience_ExperienceID",
                table: "UserSkillExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillProject_Project_ProjectID",
                table: "UserSkillProject");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                table: "BlogPost",
                column: "LKP_BlogPostStatusID",
                principalTable: "LKP_BlogPostStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_User_UserID",
                table: "BlogPost",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostTag_BlogPost_BlogPostID",
                table: "BlogPostTag",
                column: "BlogPostID",
                principalTable: "BlogPost",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostTag_Tag_TagId",
                table: "BlogPostTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_LKP_Certificate_LKP_CertificateID",
                table: "Certificate",
                column: "LKP_CertificateID",
                principalTable: "LKP_Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_User_UserID",
                table: "Certificate",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateMedia_Certificate_CertificateID",
                table: "CertificateMedia",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessage_User_UserID",
                table: "ContactMessage",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_Degree_LKP_DegreeID",
                table: "Education",
                column: "LKP_DegreeID",
                principalTable: "LKP_Degree",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                table: "Education",
                column: "LKP_FieldOfStudyID",
                principalTable: "LKP_FieldOfStudy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                table: "Education",
                column: "LKP_InstitutionID",
                principalTable: "LKP_Institution",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_User_UserID",
                table: "Education",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_User_UserID",
                table: "Experience",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingEmailConfirmation_User_UserID",
                table: "PendingEmailConfirmation",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Education_EducationID",
                table: "Project",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Experience_ExperienceID",
                table: "Project",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_UserID",
                table: "Project",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_UserID",
                table: "RefreshToken",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialLink_User_UserID",
                table: "SocialLink",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChartPreference_LKP_ChartType_LKP_ChartTypeID",
                table: "UserChartPreference",
                column: "LKP_ChartTypeID",
                principalTable: "LKP_ChartType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChartPreference_LKP_Widget_LKP_WidgetID",
                table: "UserChartPreference",
                column: "LKP_WidgetID",
                principalTable: "LKP_Widget",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChartPreference_User_UserID",
                table: "UserChartPreference",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficienc~",
                table: "UserLanguage",
                column: "LKP_LanguageProficiencyID",
                principalTable: "LKP_LanguageProficiency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_LKP_Language_LKP_LanguageID",
                table: "UserLanguage",
                column: "LKP_LanguageID",
                principalTable: "LKP_Language",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_User_UserID",
                table: "UserLanguage",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreference_LKP_Preference_LKP_PreferenceID",
                table: "UserPreference",
                column: "LKP_PreferenceID",
                principalTable: "LKP_Preference",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreference_User_UserID",
                table: "UserPreference",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                table: "UserSkill",
                column: "LKP_SkillID",
                principalTable: "LKP_Skill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_User_UserID",
                table: "UserSkill",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillCertificate_Certificate_CertificateID",
                table: "UserSkillCertificate",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEducation_Education_EducationID",
                table: "UserSkillEducation",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillExperience_Experience_ExperienceID",
                table: "UserSkillExperience",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillProject_Project_ProjectID",
                table: "UserSkillProject",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                table: "BlogPost");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_User_UserID",
                table: "BlogPost");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostTag_BlogPost_BlogPostID",
                table: "BlogPostTag");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostTag_Tag_TagId",
                table: "BlogPostTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_LKP_Certificate_LKP_CertificateID",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_Certificate_User_UserID",
                table: "Certificate");

            migrationBuilder.DropForeignKey(
                name: "FK_CertificateMedia_Certificate_CertificateID",
                table: "CertificateMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_ContactMessage_User_UserID",
                table: "ContactMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_Degree_LKP_DegreeID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_User_UserID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Experience_User_UserID",
                table: "Experience");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingEmailConfirmation_User_UserID",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Education_EducationID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Experience_ExperienceID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_User_UserID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_UserID",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_SocialLink_User_UserID",
                table: "SocialLink");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChartPreference_LKP_ChartType_LKP_ChartTypeID",
                table: "UserChartPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChartPreference_LKP_Widget_LKP_WidgetID",
                table: "UserChartPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserChartPreference_User_UserID",
                table: "UserChartPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficienc~",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_LKP_Language_LKP_LanguageID",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_User_UserID",
                table: "UserLanguage");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreference_LKP_Preference_LKP_PreferenceID",
                table: "UserPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserPreference_User_UserID",
                table: "UserPreference");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_User_UserID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillCertificate_Certificate_CertificateID",
                table: "UserSkillCertificate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEducation_Education_EducationID",
                table: "UserSkillEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillExperience_Experience_ExperienceID",
                table: "UserSkillExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillProject_Project_ProjectID",
                table: "UserSkillProject");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                table: "BlogPost",
                column: "LKP_BlogPostStatusID",
                principalTable: "LKP_BlogPostStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_User_UserID",
                table: "BlogPost",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostTag_BlogPost_BlogPostID",
                table: "BlogPostTag",
                column: "BlogPostID",
                principalTable: "BlogPost",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostTag_Tag_TagId",
                table: "BlogPostTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_LKP_Certificate_LKP_CertificateID",
                table: "Certificate",
                column: "LKP_CertificateID",
                principalTable: "LKP_Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Certificate_User_UserID",
                table: "Certificate",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CertificateMedia_Certificate_CertificateID",
                table: "CertificateMedia",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactMessage_User_UserID",
                table: "ContactMessage",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_Degree_LKP_DegreeID",
                table: "Education",
                column: "LKP_DegreeID",
                principalTable: "LKP_Degree",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                table: "Education",
                column: "LKP_FieldOfStudyID",
                principalTable: "LKP_FieldOfStudy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                table: "Education",
                column: "LKP_InstitutionID",
                principalTable: "LKP_Institution",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_User_UserID",
                table: "Education",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Experience_User_UserID",
                table: "Experience",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingEmailConfirmation_User_UserID",
                table: "PendingEmailConfirmation",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Education_EducationID",
                table: "Project",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Experience_ExperienceID",
                table: "Project",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_User_UserID",
                table: "Project",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_UserID",
                table: "RefreshToken",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SocialLink_User_UserID",
                table: "SocialLink",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_User_Role_RoleID",
                table: "User",
                column: "RoleID",
                principalTable: "Role",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChartPreference_LKP_ChartType_LKP_ChartTypeID",
                table: "UserChartPreference",
                column: "LKP_ChartTypeID",
                principalTable: "LKP_ChartType",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChartPreference_LKP_Widget_LKP_WidgetID",
                table: "UserChartPreference",
                column: "LKP_WidgetID",
                principalTable: "LKP_Widget",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserChartPreference_User_UserID",
                table: "UserChartPreference",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficienc~",
                table: "UserLanguage",
                column: "LKP_LanguageProficiencyID",
                principalTable: "LKP_LanguageProficiency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_LKP_Language_LKP_LanguageID",
                table: "UserLanguage",
                column: "LKP_LanguageID",
                principalTable: "LKP_Language",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_User_UserID",
                table: "UserLanguage",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreference_LKP_Preference_LKP_PreferenceID",
                table: "UserPreference",
                column: "LKP_PreferenceID",
                principalTable: "LKP_Preference",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserPreference_User_UserID",
                table: "UserPreference",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                table: "UserSkill",
                column: "LKP_SkillID",
                principalTable: "LKP_Skill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_User_UserID",
                table: "UserSkill",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillCertificate_Certificate_CertificateID",
                table: "UserSkillCertificate",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEducation_Education_EducationID",
                table: "UserSkillEducation",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillExperience_Experience_ExperienceID",
                table: "UserSkillExperience",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillProject_Project_ProjectID",
                table: "UserSkillProject",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");
        }
    }
}
