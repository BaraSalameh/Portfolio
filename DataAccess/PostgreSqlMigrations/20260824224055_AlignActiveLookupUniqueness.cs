using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AlignActiveLookupUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference");

            migrationBuilder.DropIndex(
                name: "IX_LKP_LanguageProficiency_Level",
                table: "LKP_LanguageProficiency");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Language_Name",
                table: "LKP_Language");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_LanguageProficiency_Level",
                table: "LKP_LanguageProficiency",
                column: "Level",
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Language_Name",
                table: "LKP_Language",
                column: "Name",
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Role_Name",
                table: "Role");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference");

            migrationBuilder.DropIndex(
                name: "IX_LKP_LanguageProficiency_Level",
                table: "LKP_LanguageProficiency");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Language_Name",
                table: "LKP_Language");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_LanguageProficiency_Level",
                table: "LKP_LanguageProficiency",
                column: "Level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Language_Name",
                table: "LKP_Language",
                column: "Name",
                unique: true);
        }
    }
}
