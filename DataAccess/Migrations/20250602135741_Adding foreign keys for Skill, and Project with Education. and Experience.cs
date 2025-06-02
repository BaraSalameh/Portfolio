using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingforeignkeysforSkillandProjectwithEducationandExperience : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EducationID",
                table: "Skill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExperienceID",
                table: "Skill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EducationID",
                table: "Project",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExperienceID",
                table: "Project",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skill_EducationID",
                table: "Skill",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ExperienceID",
                table: "Skill",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EducationID",
                table: "Project",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ExperienceID",
                table: "Project",
                column: "ExperienceID");

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Education_EducationID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Experience_ExperienceID",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Education_EducationID",
                table: "Skill");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Experience_ExperienceID",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_EducationID",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_ExperienceID",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Project_EducationID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_ExperienceID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "EducationID",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "ExperienceID",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "EducationID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "ExperienceID",
                table: "Project");
        }
    }
}
