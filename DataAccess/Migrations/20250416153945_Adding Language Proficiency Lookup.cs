using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingLanguageProficiencyLookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_ProficiencyID",
                table: "UserLanguage");

            migrationBuilder.RenameColumn(
                name: "ProficiencyID",
                table: "UserLanguage",
                newName: "LKP_LanguageProficiencyID");

            migrationBuilder.RenameIndex(
                name: "IX_UserLanguage_ProficiencyID",
                table: "UserLanguage",
                newName: "IX_UserLanguage_LKP_LanguageProficiencyID");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LKP_LanguageProficiency",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LKP_LanguageProficiency",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LKP_LanguageProficiency",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LKP_LanguageProficiency",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficiencyID",
                table: "UserLanguage",
                column: "LKP_LanguageProficiencyID",
                principalTable: "LKP_LanguageProficiency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficiencyID",
                table: "UserLanguage");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LKP_LanguageProficiency");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LKP_LanguageProficiency");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LKP_LanguageProficiency");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LKP_LanguageProficiency");

            migrationBuilder.RenameColumn(
                name: "LKP_LanguageProficiencyID",
                table: "UserLanguage",
                newName: "ProficiencyID");

            migrationBuilder.RenameIndex(
                name: "IX_UserLanguage_LKP_LanguageProficiencyID",
                table: "UserLanguage",
                newName: "IX_UserLanguage_ProficiencyID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserLanguage_LKP_LanguageProficiency_ProficiencyID",
                table: "UserLanguage",
                column: "ProficiencyID",
                principalTable: "LKP_LanguageProficiency",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
