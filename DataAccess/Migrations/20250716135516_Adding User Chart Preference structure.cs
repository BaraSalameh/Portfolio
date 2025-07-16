using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserChartPreferencestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LKP_ChartType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_ChartType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Widget",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Widget", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserChartPreference",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LKP_WidgetID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LKP_ChartTypeID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChartPreference", x => new { x.UserID, x.LKP_WidgetID, x.LKP_ChartTypeID });
                    table.ForeignKey(
                        name: "FK_UserChartPreference_LKP_ChartType_LKP_ChartTypeID",
                        column: x => x.LKP_ChartTypeID,
                        principalTable: "LKP_ChartType",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChartPreference_LKP_Widget_LKP_WidgetID",
                        column: x => x.LKP_WidgetID,
                        principalTable: "LKP_Widget",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChartPreference_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LKP_ChartType",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10b6e1ab-9d90-45ce-81fd-7258db4fae2c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a5f1f2c3-67bd-41b2-bc0b-f1c7aa4fdab0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Line", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b14a8d12-1e01-4d91-b7ae-85f2219f03aa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Bar", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c92a1e67-f510-49bb-910d-b331d4f04d47"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pie", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("de5d14cf-9731-4ea1-8cf3-5b6bc7167b41"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Donut", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "LKP_Widget",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("194e6b38-5f1d-4b6f-bf6a-5ac4aaad5b94"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "About", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3ae5b0f3-d26c-4d98-b4ec-5c6f4b1e6f8e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Contact", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("55c7dd42-07ec-4c5f-aadc-2ad7f3bdfae4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Language", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a8d0f22e-d1b3-4d1f-83c7-4e67a345f311"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Education", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b69e03a3-2fa5-4cb3-8d36-5607c49fd779"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Skill", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c6d20f43-5ae3-4df3-bf37-e657c26d63aa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Certification", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e79c20c5-92a4-47e5-b167-f028f55a364a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Experience", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f3b2cf11-6ce0-4e06-b798-1826b8bc67f0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LKP_ChartType_Name",
                table: "LKP_ChartType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Widget_Name",
                table: "LKP_Widget",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChartPreference_LKP_ChartTypeID",
                table: "UserChartPreference",
                column: "LKP_ChartTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_UserChartPreference_LKP_WidgetID",
                table: "UserChartPreference",
                column: "LKP_WidgetID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserChartPreference");

            migrationBuilder.DropTable(
                name: "LKP_ChartType");

            migrationBuilder.DropTable(
                name: "LKP_Widget");
        }
    }
}
