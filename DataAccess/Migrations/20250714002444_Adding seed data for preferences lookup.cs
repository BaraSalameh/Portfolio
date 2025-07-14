using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Addingseeddataforpreferenceslookup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_Preference",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "LKP_Preference",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01278046-dcf6-4c39-a256-32f52e0b6eeb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-experience-radar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("0ddf9055-fd64-4c9a-84f0-5d8db3db17a0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-overview-bar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3004c55b-16b9-4fa2-bbe5-fbd26aa31497"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-project-radar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3055cce6-4022-4c2c-87cf-2ea06b9e7d2d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-overview-widget", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3f87d7a5-5ab8-4ea6-85c7-eed6bb83dcb0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-education-radar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4073e1e3-3d59-4f12-ae90-31f2d20cf68b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "profile-width", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("435a47b5-43c4-4c0f-91ed-7d6a32ae5398"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-overview-pie-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("491b6c0a-7f16-4c01-b3fd-5010ff4b6072"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "birthdate-format", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6d83cb36-fd8e-4fd2-87d2-4d4d9b9e4f27"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-experience-bar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6f1f71a6-74b1-4ed3-b2ae-4d1137dbcb8d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-language-bar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8e4d5b5f-3f44-49a8-83c2-d4c3c5155e63"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-birthdate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8f9e7e6b-6f49-420e-8fd2-3ea35aa9d5b0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "profile-picture-position", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("95b5f7ec-e1c2-446f-8401-e0a982a6172e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-experience-pie-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a68cd4c7-b0fd-4d25-a32f-d7772082ae9c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-language-pie-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("acef54ff-49b5-45bb-a84f-0eafce08730c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-overview-radar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b65b26c1-d9c7-4089-9ae2-31a2353cf434"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-project-widget", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ca2375d4-d3e4-4dc3-b25c-9dc6fcb03c4e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-education-bar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cb4c589b-cb07-4414-92f5-98d5c08867a7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-education-pie-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d05c7c4e-c3bb-4422-8ad2-3d10ec961a49"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-project-pie-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ec47f4b3-2852-4067-a2e9-0e43b2e7b91b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-email-address", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f1a529dc-99a1-41d1-86bb-bd9d661a9435"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-language-radar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f45d65cf-2f6e-4a42-b25a-11eb326c8f38"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-phone-number", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f9ef68e1-f315-4a3c-b3d5-9a53646e75aa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-gender", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fe5d6427-2ae3-49c5-b94e-8b0e1c361471"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "show-project-bar-chart", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference");

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("01278046-dcf6-4c39-a256-32f52e0b6eeb"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("0ddf9055-fd64-4c9a-84f0-5d8db3db17a0"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("3004c55b-16b9-4fa2-bbe5-fbd26aa31497"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("3055cce6-4022-4c2c-87cf-2ea06b9e7d2d"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("3f87d7a5-5ab8-4ea6-85c7-eed6bb83dcb0"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("4073e1e3-3d59-4f12-ae90-31f2d20cf68b"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("435a47b5-43c4-4c0f-91ed-7d6a32ae5398"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("491b6c0a-7f16-4c01-b3fd-5010ff4b6072"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("6d83cb36-fd8e-4fd2-87d2-4d4d9b9e4f27"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("6f1f71a6-74b1-4ed3-b2ae-4d1137dbcb8d"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("8e4d5b5f-3f44-49a8-83c2-d4c3c5155e63"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("8f9e7e6b-6f49-420e-8fd2-3ea35aa9d5b0"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("95b5f7ec-e1c2-446f-8401-e0a982a6172e"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("a68cd4c7-b0fd-4d25-a32f-d7772082ae9c"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("acef54ff-49b5-45bb-a84f-0eafce08730c"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("b65b26c1-d9c7-4089-9ae2-31a2353cf434"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("ca2375d4-d3e4-4dc3-b25c-9dc6fcb03c4e"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("cb4c589b-cb07-4414-92f5-98d5c08867a7"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("d05c7c4e-c3bb-4422-8ad2-3d10ec961a49"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("ec47f4b3-2852-4067-a2e9-0e43b2e7b91b"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("f1a529dc-99a1-41d1-86bb-bd9d661a9435"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("f45d65cf-2f6e-4a42-b25a-11eb326c8f38"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("f9ef68e1-f315-4a3c-b3d5-9a53646e75aa"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("fe5d6427-2ae3-49c5-b94e-8b0e1c361471"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_Preference",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
