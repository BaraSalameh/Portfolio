using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingLKP_LanguageLevelByAddingIsDeletedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_Profile_ProfileID",
                table: "Education");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LKP_LanguageLevel",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ProfileID",
                table: "Education",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Profile_ProfileID",
                table: "Education",
                column: "ProfileID",
                principalTable: "Profile",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_Profile_ProfileID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LKP_LanguageLevel");

            migrationBuilder.AlterColumn<int>(
                name: "ProfileID",
                table: "Education",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_Profile_ProfileID",
                table: "Education",
                column: "ProfileID",
                principalTable: "Profile",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
