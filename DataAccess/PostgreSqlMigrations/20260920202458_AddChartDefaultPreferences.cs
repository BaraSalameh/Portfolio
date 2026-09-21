using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddChartDefaultPreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LKP_Preference",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("d1a7c001-8618-4a18-9714-1a489963d801"), null, "default-overview-chart", null },
                    { new Guid("d1a7c002-8618-4a18-9714-1a489963d802"), null, "default-education-chart", null },
                    { new Guid("d1a7c003-8618-4a18-9714-1a489963d803"), null, "default-experience-chart", null },
                    { new Guid("d1a7c004-8618-4a18-9714-1a489963d804"), null, "default-project-chart", null },
                    { new Guid("d1a7c005-8618-4a18-9714-1a489963d805"), null, "default-skill-chart", null },
                    { new Guid("d1a7c006-8618-4a18-9714-1a489963d806"), null, "default-language-chart", null },
                    { new Guid("d1a7c007-8618-4a18-9714-1a489963d807"), null, "default-certificate-chart", null }
                });

            migrationBuilder.UpdateData(
                table: "LKP_Widget",
                keyColumn: "ID",
                keyValue: new Guid("c6d20f43-5ae3-4df3-bf37-e657c26d63aa"),
                column: "Name",
                value: "Certificate");

            migrationBuilder.InsertData(
                table: "LKP_Widget",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[] { new Guid("7c9e2f10-aa27-41ba-99b2-7a70e7218d21"), null, "Overview", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c001-8618-4a18-9714-1a489963d801"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c002-8618-4a18-9714-1a489963d802"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c003-8618-4a18-9714-1a489963d803"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c004-8618-4a18-9714-1a489963d804"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c005-8618-4a18-9714-1a489963d805"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c006-8618-4a18-9714-1a489963d806"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d1a7c007-8618-4a18-9714-1a489963d807"));

            migrationBuilder.DeleteData(
                table: "LKP_Widget",
                keyColumn: "ID",
                keyValue: new Guid("7c9e2f10-aa27-41ba-99b2-7a70e7218d21"));

            migrationBuilder.UpdateData(
                table: "LKP_Widget",
                keyColumn: "ID",
                keyValue: new Guid("c6d20f43-5ae3-4df3-bf37-e657c26d63aa"),
                column: "Name",
                value: "Certification");
        }
    }
}
