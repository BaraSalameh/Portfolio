using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingUserSkillindexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.AddColumn<Guid>(
                name: "ID",
                table: "UserSkill",
                type: "uniqueidentifier",
                nullable: false,
                defaultValueSql: "NEWID()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_CertificateID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "CertificateID" },
                unique: true,
                filter: "[CertificateID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_EducationID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "EducationID" },
                unique: true,
                filter: "[EducationID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ExperienceID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "ExperienceID" },
                unique: true,
                filter: "[ExperienceID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ProjectID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "ProjectID" },
                unique: true,
                filter: "[ProjectID] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_CertificateID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_EducationID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ExperienceID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ProjectID",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "UserSkill");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserSkill",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID" });
        }
    }
}
