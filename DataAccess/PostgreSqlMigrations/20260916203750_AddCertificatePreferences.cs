using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddCertificatePreferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LKP_Preference",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("78664c9d-5e97-4d6b-909f-7a3010497551"), null, "show-certificate-widget", null },
                    { new Guid("8514987f-e721-4241-9f96-334ae5758086"), null, "show-certificate-bar-chart", null },
                    { new Guid("95854f81-5544-47bf-95c2-74776367e09e"), null, "show-certificate-pie-chart", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("78664c9d-5e97-4d6b-909f-7a3010497551"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("8514987f-e721-4241-9f96-334ae5758086"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("95854f81-5544-47bf-95c2-74776367e09e"));
        }
    }
}
