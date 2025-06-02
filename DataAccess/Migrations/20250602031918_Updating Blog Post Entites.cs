using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingBlogPostEntites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "BlogPost");

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "BlogPost",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "LKP_BlogPostStatusID",
                table: "BlogPost",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LKP_BlogPostStatus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_BlogPostStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTag",
                columns: table => new
                {
                    BlogPostID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TagId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPostTag", x => new { x.BlogPostID, x.TagId });
                    table.ForeignKey(
                        name: "FK_BlogPostTag_BlogPost_BlogPostID",
                        column: x => x.BlogPostID,
                        principalTable: "BlogPost",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPostTag_Tag_TagId",
                        column: x => x.TagId,
                        principalTable: "Tag",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LKP_BlogPostStatus",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("4c9e2d6a-6d8b-4a2e-9f2d-32f4a7d290c3"), "Published" },
                    { new Guid("8b1f2e0c-5b7e-4f6d-98e4-cfb230fe4f99"), "Scheduled" },
                    { new Guid("a7f5d9b3-9c7d-47a9-8c2e-13d43f26a6f2"), "PendingReview" },
                    { new Guid("b8d6f4a0-1e97-4f39-80c9-3f1e7216b45e"), "Deleted" },
                    { new Guid("d3a7b6f1-8f2a-4d93-9bfc-1e8a4b6f0a11"), "Draft" },
                    { new Guid("ee4a3c1d-7f42-4f1a-b4d9-2d84f8a72954"), "Archived" },
                    { new Guid("f12c7b8e-3a49-4b9f-9e13-6dbd85d24870"), "Rejected" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_LKP_BlogPostStatusID",
                table: "BlogPost",
                column: "LKP_BlogPostStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTag_TagId",
                table: "BlogPostTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_BlogPostStatus_Name",
                table: "LKP_BlogPostStatus",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                table: "BlogPost",
                column: "LKP_BlogPostStatusID",
                principalTable: "LKP_BlogPostStatus",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                table: "BlogPost");

            migrationBuilder.DropTable(
                name: "BlogPostTag");

            migrationBuilder.DropTable(
                name: "LKP_BlogPostStatus");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_BlogPost_LKP_BlogPostStatusID",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "LKP_BlogPostStatusID",
                table: "BlogPost");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "BlogPost",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
