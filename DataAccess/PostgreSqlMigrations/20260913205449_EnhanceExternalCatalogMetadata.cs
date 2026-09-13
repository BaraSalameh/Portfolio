using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class EnhanceExternalCatalogMetadata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LKP_Skill",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "LKP_Institution",
                type: "character varying(2)",
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryName",
                table: "LKP_Institution",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LKP_Institution",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c001"),
                columns: new[] { "CountryCode", "CountryName", "IsActive" },
                values: new object[] { null, null, true });

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c002"),
                columns: new[] { "CountryCode", "CountryName", "IsActive" },
                values: new object[] { null, null, true });

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c003"),
                columns: new[] { "CountryCode", "CountryName", "IsActive" },
                values: new object[] { null, null, true });

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c004"),
                columns: new[] { "CountryCode", "CountryName", "IsActive" },
                values: new object[] { null, null, true });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("02a3d389-06b7-4be0-a62f-7aa23e8a2de1"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("30e964e3-b1d1-4890-a632-857c33b22803"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("34db2c3b-59be-4b0f-a988-f816b4e2a82e"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("47b844d2-3ee5-4907-92c3-f09f5a92b3f0"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("51d71c55-f93a-4b6d-94b5-5425e9f7c026"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("5476bcee-4d61-4f0a-905f-2fa0f8a5287f"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("69cce10e-9ecf-46e8-a831-b539a1a65149"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("6bfb8a3e-1b9f-4d9d-a58d-36d967bc9c01"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("73f9372e-37bb-4703-9936-8f74109aa3f0"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("76a5c3f9-5b4e-4d3c-b2b2-481c44500cd4"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("7dc2f321-70c7-4a6e-8721-3ecf3ae36745"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("908e1c7e-2de7-44f9-b189-146e4c6784e9"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("9d53f924-48c3-4c86-8ac3-1f8d0d013e50"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("c1b76b91-55ae-47b3-9241-5e6f54b54f4f"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("c9e6e1fc-5f70-453d-8a23-5fa9b69331e0"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("cb84548a-1d9d-47c6-bdb9-01e27c86720d"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("cfcaa188-f289-4c33-82ab-7d2f16d4e60f"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("d87a4b5c-43e6-4762-9f9b-6f7e4dc2c4e0"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("d8cf53c1-0fa2-4f10-9584-6c879e1420bc"),
                column: "IsActive",
                value: true);

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("f02b09a0-c7a5-4f0c-9e6a-08d7c4f8ef24"),
                column: "IsActive",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LKP_Skill");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "LKP_Institution");

            migrationBuilder.DropColumn(
                name: "CountryName",
                table: "LKP_Institution");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LKP_Institution");
        }
    }
}
