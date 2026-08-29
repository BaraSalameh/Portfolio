using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddGlobalOptimisticConcurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "UserSkill",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "UserPreference",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "UserChartPreference",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "SocialLink",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "Role",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_Widget",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_Skill",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_Preference",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_LanguageProficiency",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_Language",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_ChartType",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "LKP_Certificate",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "ContactMessage",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "CertificateMedia",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<uint>(
                name: "xmin",
                table: "BlogPost",
                type: "xid",
                rowVersion: true,
                nullable: false,
                defaultValue: 0u);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "xmin",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "UserPreference");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "UserChartPreference");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "SocialLink");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "Role");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_Widget");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_Skill");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_Preference");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_LanguageProficiency");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_Language");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_ChartType");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "LKP_Certificate");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "ContactMessage");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "CertificateMedia");

            migrationBuilder.DropColumn(
                name: "xmin",
                table: "BlogPost");
        }
    }
}
