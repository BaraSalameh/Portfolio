using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChandingtheSkillentitynametoUserSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Education_EducationID",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Experience_ExperienceID",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_LKP_Skill_LKP_SkillID",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Project_ProjectID",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_User_UserID",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "UserSkill");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_ProjectID",
                table: "UserSkill",
                newName: "IX_UserSkill_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_LKP_SkillID",
                table: "UserSkill",
                newName: "IX_UserSkill_LKP_SkillID");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_ExperienceID",
                table: "UserSkill",
                newName: "IX_UserSkill_ExperienceID");

            migrationBuilder.RenameIndex(
                name: "IX_Skill_EducationID",
                table: "UserSkill",
                newName: "IX_UserSkill_EducationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Education_EducationID",
                table: "UserSkill",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Experience_ExperienceID",
                table: "UserSkill",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                table: "UserSkill",
                column: "LKP_SkillID",
                principalTable: "LKP_Skill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Project_ProjectID",
                table: "UserSkill",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_User_UserID",
                table: "UserSkill",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Education_EducationID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Experience_ExperienceID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Project_ProjectID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_User_UserID",
                table: "UserSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.RenameTable(
                name: "UserSkill",
                newName: "Skill");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_ProjectID",
                table: "Skill",
                newName: "IX_Skill_ProjectID");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_LKP_SkillID",
                table: "Skill",
                newName: "IX_Skill_LKP_SkillID");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_ExperienceID",
                table: "Skill",
                newName: "IX_Skill_ExperienceID");

            migrationBuilder.RenameIndex(
                name: "IX_UserSkill_EducationID",
                table: "Skill",
                newName: "IX_Skill_EducationID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                columns: new[] { "UserID", "LKP_SkillID" });

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Education_EducationID",
                table: "Skill",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Experience_ExperienceID",
                table: "Skill",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_LKP_Skill_LKP_SkillID",
                table: "Skill",
                column: "LKP_SkillID",
                principalTable: "LKP_Skill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Project_ProjectID",
                table: "Skill",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_User_UserID",
                table: "Skill",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
