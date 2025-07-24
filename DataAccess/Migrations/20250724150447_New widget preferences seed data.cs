using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Newwidgetpreferencesseeddata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "LKP_Preference",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("9d7a1776-99d6-4206-8d0d-1a22365b8a97"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-skill-pie-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b10f6ef7-35cc-44a3-81b4-d78cc8f5aaf1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-skill-widget", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c14d3b4f-62a2-4db1-897c-f3cb3eae3122"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-skill-bar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fb91d22c-b6cd-4f09-b9a0-7a9633027f49"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-skill-radar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("9d7a1776-99d6-4206-8d0d-1a22365b8a97"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("b10f6ef7-35cc-44a3-81b4-d78cc8f5aaf1"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("c14d3b4f-62a2-4db1-897c-f3cb3eae3122"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("fb91d22c-b6cd-4f09-b9a0-7a9633027f49"));
        }
    }
}
