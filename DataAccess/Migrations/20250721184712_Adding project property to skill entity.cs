using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addingprojectpropertytoskillentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectID",
                table: "Skill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skill_ProjectID",
                table: "Skill",
                column: "ProjectID");

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Project_ProjectID",
                table: "Skill",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Project_ProjectID",
                table: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_Skill_ProjectID",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "Skill");
        }
    }
}
