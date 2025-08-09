using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Updatedeletingstrategyforuserskill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject",
                column: "UserSkillID",
                principalTable: "UserSkill",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                table: "UserSkillEducation");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkillProject_UserSkill_UserSkillID",
                table: "UserSkillProject");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                table: "UserSkillCertificate",
                column: "UserSkillID",
                principalTable: "UserSkill",
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
                name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                table: "UserSkillExperience",
                column: "UserSkillID",
                principalTable: "UserSkill",
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
    }
}
