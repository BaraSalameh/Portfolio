using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSql : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LKP_BlogPostStatus",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_BlogPostStatus", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Certificate",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Certificate", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_ChartType",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_ChartType", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Degree",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Abbreviation = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Degree", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_FieldOfStudy",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_FieldOfStudy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Institution",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Logo = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Institution", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Language",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Language", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_LanguageProficiency",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Level = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_LanguageProficiency", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Preference",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Preference", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Skill",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconUrl = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Skill", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Widget",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Widget", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Firstname = table.Column<string>(type: "text", nullable: false),
                    Lastname = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Bio = table.Column<string>(type: "text", nullable: true),
                    Phone = table.Column<string>(type: "text", nullable: true),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    CoverPhoto = table.Column<string>(type: "text", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Gender = table.Column<int>(type: "integer", nullable: true),
                    RoleID = table.Column<Guid>(type: "uuid", nullable: false),
                    IsConfirmed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.ID);
                    table.ForeignKey(
                        name: "FK_User_Role_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Role",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPost",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Thumbnail = table.Column<string>(type: "text", nullable: false),
                    LKP_BlogPostStatusID = table.Column<Guid>(type: "uuid", nullable: false),
                    Excerpt = table.Column<string>(type: "text", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPost", x => x.ID);
                    table.ForeignKey(
                        name: "FK_BlogPost_LKP_BlogPostStatus_LKP_BlogPostStatusID",
                        column: x => x.LKP_BlogPostStatusID,
                        principalTable: "LKP_BlogPostStatus",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogPost_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    LKP_CertificateID = table.Column<Guid>(type: "uuid", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CredintialID = table.Column<string>(type: "text", nullable: true),
                    CredintialUrl = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Certificate_LKP_Certificate_LKP_CertificateID",
                        column: x => x.LKP_CertificateID,
                        principalTable: "LKP_Certificate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificate_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessage",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessage", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ContactMessage_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    LKP_InstitutionID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_DegreeID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_FieldOfStudyID = table.Column<Guid>(type: "uuid", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Education_LKP_Degree_LKP_DegreeID",
                        column: x => x.LKP_DegreeID,
                        principalTable: "LKP_Degree",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                        column: x => x.LKP_FieldOfStudyID,
                        principalTable: "LKP_FieldOfStudy",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                        column: x => x.LKP_InstitutionID,
                        principalTable: "LKP_Institution",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Education_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    JobTitle = table.Column<string>(type: "text", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Location = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Experience_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingEmailConfirmation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    TokenHash = table.Column<string>(type: "text", nullable: false),
                    RememberMe = table.Column<bool>(type: "boolean", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingEmailConfirmation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PendingEmailConfirmation_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Token = table.Column<string>(type: "text", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedByIp = table.Column<string>(type: "text", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    RevokedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    RememberMe = table.Column<bool>(type: "boolean", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialLink",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Platform = table.Column<string>(type: "text", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: false),
                    Icon = table.Column<string>(type: "text", nullable: true),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialLink", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SocialLink_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChartPreference",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_WidgetID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_ChartTypeID = table.Column<Guid>(type: "uuid", nullable: false),
                    GroupBy = table.Column<string>(type: "text", nullable: false),
                    ValueSource = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
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

            migrationBuilder.CreateTable(
                name: "UserLanguage",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_LanguageID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_LanguageProficiencyID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLanguage", x => new { x.UserID, x.LKP_LanguageID });
                    table.ForeignKey(
                        name: "FK_UserLanguage_LKP_LanguageProficiency_LKP_LanguageProficienc~",
                        column: x => x.LKP_LanguageProficiencyID,
                        principalTable: "LKP_LanguageProficiency",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguage_LKP_Language_LKP_LanguageID",
                        column: x => x.LKP_LanguageID,
                        principalTable: "LKP_Language",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserLanguage_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPreference",
                columns: table => new
                {
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_PreferenceID = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPreference", x => new { x.UserID, x.LKP_PreferenceID });
                    table.ForeignKey(
                        name: "FK_UserPreference_LKP_Preference_LKP_PreferenceID",
                        column: x => x.LKP_PreferenceID,
                        principalTable: "LKP_Preference",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPreference_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkill",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    LKP_SkillID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkill", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserSkill_LKP_Skill_LKP_SkillID",
                        column: x => x.LKP_SkillID,
                        principalTable: "LKP_Skill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkill_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BlogPostTag",
                columns: table => new
                {
                    BlogPostID = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
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

            migrationBuilder.CreateTable(
                name: "CertificateMedia",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Url = table.Column<string>(type: "text", nullable: false),
                    CertificateID = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateMedia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CertificateMedia_Certificate_CertificateID",
                        column: x => x.CertificateID,
                        principalTable: "Certificate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    LiveLink = table.Column<string>(type: "text", nullable: true),
                    SourceCode = table.Column<string>(type: "text", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    IsFeatured = table.Column<bool>(type: "boolean", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    UserID = table.Column<Guid>(type: "uuid", nullable: false),
                    EducationID = table.Column<Guid>(type: "uuid", nullable: true),
                    ExperienceID = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Project_Education_EducationID",
                        column: x => x.EducationID,
                        principalTable: "Education",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Project_Experience_ExperienceID",
                        column: x => x.ExperienceID,
                        principalTable: "Experience",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Project_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillCertificate",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uuid", nullable: false),
                    CertificateID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillCertificate", x => new { x.UserSkillID, x.CertificateID });
                    table.ForeignKey(
                        name: "FK_UserSkillCertificate_Certificate_CertificateID",
                        column: x => x.CertificateID,
                        principalTable: "Certificate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserSkillEducation",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uuid", nullable: false),
                    EducationID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillEducation", x => new { x.UserSkillID, x.EducationID });
                    table.ForeignKey(
                        name: "FK_UserSkillEducation_Education_EducationID",
                        column: x => x.EducationID,
                        principalTable: "Education",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserSkillExperience",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uuid", nullable: false),
                    ExperienceID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillExperience", x => new { x.UserSkillID, x.ExperienceID });
                    table.ForeignKey(
                        name: "FK_UserSkillExperience_Experience_ExperienceID",
                        column: x => x.ExperienceID,
                        principalTable: "Experience",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "UserSkillProject",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillProject", x => new { x.UserSkillID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_UserSkillProject_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillProject_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID");
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

            migrationBuilder.InsertData(
                table: "LKP_Certificate",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("04274ed2-cdc7-4d3b-99ac-b18cf7f78a35"), null, "CompTIA Database Administrator", null },
                    { new Guid("10284bc8-7380-4fef-8098-9e8682831787"), null, "Oracle System Administrator", null },
                    { new Guid("1059acd9-bc5b-481a-8c2a-25fbf3684636"), null, "ISACA Security Specialist", null },
                    { new Guid("111434a0-a785-476b-a402-3b0c6cbc9d0b"), null, "IBM Certified Developer", null },
                    { new Guid("11643d44-35d5-4bef-8956-78ed7d93c81a"), null, "PMI Network Associate", null },
                    { new Guid("15acc4ef-0770-4f34-9878-7e66c321f4b6"), null, "ISACA Network Associate", null },
                    { new Guid("16760e04-59bf-4712-acc1-aee450ae9058"), null, "Red Hat Project Manager", null },
                    { new Guid("1901ff1b-7724-405c-8908-48de51138df2"), null, "Adobe Data Engineer", null },
                    { new Guid("1b217b4c-e382-41ac-b353-6e8c25e85675"), null, "AWS Penetration Tester", null },
                    { new Guid("1d0a2fb3-abce-46be-8996-40df1a2ea454"), null, "Red Hat IT Technician", null },
                    { new Guid("232b4254-96c1-4196-a21c-5a8920c11795"), null, "CompTIA IT Technician", null },
                    { new Guid("237d5521-b406-4c0f-9471-2d67804b03c3"), null, "ISACA IT Technician", null },
                    { new Guid("24052529-fced-4ad4-900e-0e5f555fe9ad"), null, "Cisco Security Specialist", null },
                    { new Guid("246656fc-f27d-4f81-a508-27c2ddad3caa"), null, "Oracle Penetration Tester", null },
                    { new Guid("26846893-3967-43ac-b2dd-024d96a333f6"), null, "EC-Council Certified Solutions Architect", null },
                    { new Guid("292ef240-d26c-4d5d-881d-a9d2f3af68fe"), null, "AWS Certified Developer", null },
                    { new Guid("2ae2dced-eb42-4c80-911b-2c4685f56c84"), null, "EC-Council Data Engineer", null },
                    { new Guid("30f03a14-000b-4b1d-9a51-73d24b35060f"), null, "EC-Council Penetration Tester", null },
                    { new Guid("33c51761-e3b0-483d-9be9-68ca0676c602"), null, "Cisco Certified Solutions Architect", null },
                    { new Guid("341cace1-59e2-439a-bab3-bbe4ebd9034c"), null, "PMI Database Administrator", null },
                    { new Guid("36bf89ff-0b90-43d3-91b8-1e741850545d"), null, "Adobe System Administrator", null },
                    { new Guid("3c37a2aa-72a2-4bf2-901d-a77351964c57"), null, "CompTIA Security Specialist", null },
                    { new Guid("3cba1b91-894f-4462-bd7d-e1136fe99111"), null, "Google Network Associate", null },
                    { new Guid("3eda576a-4dd8-4f50-8467-e395235915e5"), null, "CompTIA Cloud Practitioner", null },
                    { new Guid("415af969-6968-4d94-b7a3-03decf369eaf"), null, "Oracle Data Engineer", null },
                    { new Guid("440bcc23-5883-440b-b891-1e8671e9c64e"), null, "Microsoft Certified Solutions Architect", null },
                    { new Guid("44f1378e-336c-40c8-97a0-819249dc372b"), null, "EC-Council Database Administrator", null },
                    { new Guid("498a001c-11c7-4fcb-953d-7d7dd914d1ab"), null, "CompTIA System Administrator", null },
                    { new Guid("49a55547-a821-4f3c-b7af-7e9d76aba00f"), null, "AWS Data Engineer", null },
                    { new Guid("4a67dae5-502d-450c-8020-5779577f18b2"), null, "PMI Data Engineer", null },
                    { new Guid("4bf8184b-3ccc-4ab9-9da5-c58c35ccbe8e"), null, "Adobe Database Administrator", null },
                    { new Guid("4cfd44eb-6392-4e21-87b3-c197409d8bcb"), null, "Google IT Technician", null },
                    { new Guid("55a5a15c-6837-4683-9d02-f1462f34a3a5"), null, "Adobe IT Technician", null },
                    { new Guid("58851cf8-4b6c-42d7-8a15-f1a1e94cce73"), null, "CompTIA Penetration Tester", null },
                    { new Guid("5c18acc3-b668-4711-a8b0-a4f0d78dd6a5"), null, "Google Data Engineer", null },
                    { new Guid("5e68d80a-8e65-4f66-9d8a-ea8fbb5b9760"), null, "ISACA Certified Solutions Architect", null },
                    { new Guid("60141ea8-3e51-4d67-a5a1-c17d46489d0f"), null, "Cisco Project Manager", null },
                    { new Guid("6295282b-468d-471c-badc-8f04031f3c5b"), null, "Cisco Cloud Practitioner", null },
                    { new Guid("69cd4e57-b114-4185-9d5a-8e0c964f83cb"), null, "IBM IT Technician", null },
                    { new Guid("6be5fc5d-2cd4-4c5e-863c-df56fddc55c4"), null, "Microsoft Security Specialist", null },
                    { new Guid("6c755da9-3e17-4382-95b9-3b16156f203a"), null, "Microsoft Project Manager", null },
                    { new Guid("6dfbf4e0-f70c-4837-b715-ab94b56e6e9c"), null, "Google Cloud Practitioner", null },
                    { new Guid("6e9b2a22-2244-4756-a2aa-b8fb02543822"), null, "ISACA Database Administrator", null },
                    { new Guid("7091bdca-a368-4534-8e67-b6c1c26304c5"), null, "Red Hat Security Specialist", null },
                    { new Guid("73b04b6e-8d49-4e37-80a4-92f117c5f56f"), null, "AWS Cloud Practitioner", null },
                    { new Guid("740ee348-3430-4fdd-a30c-0362a13c2c6d"), null, "PMI Cybersecurity Analyst", null },
                    { new Guid("7a70019c-c537-4980-b96f-780ca9df3701"), null, "PMI Penetration Tester", null },
                    { new Guid("7a817f5f-9ad0-4364-b322-95668711e76f"), null, "AWS Network Associate", null },
                    { new Guid("7b2bcbf8-e5d9-4d9c-83d4-a4e24cbd36c7"), null, "Adobe Network Associate", null },
                    { new Guid("7d488bd7-6e81-4db8-be6a-e49f9871f14d"), null, "Oracle Cybersecurity Analyst", null },
                    { new Guid("7e9f63e8-9950-4311-af67-9918f30bb52a"), null, "Oracle Network Associate", null },
                    { new Guid("81f63198-c51d-4a47-9d1b-0793f6fb9c57"), null, "Microsoft Cybersecurity Analyst", null },
                    { new Guid("8e48d852-1a3f-496a-b88f-a58512d6207d"), null, "Cisco Data Engineer", null },
                    { new Guid("9131cb00-0f96-4532-8981-3c628c5d7ce2"), null, "Adobe Penetration Tester", null },
                    { new Guid("91c4d43b-012d-44fa-a0eb-aa7d6c2e1222"), null, "Adobe Certified Solutions Architect", null },
                    { new Guid("945249b9-3551-43a9-a321-351f241fc920"), null, "Red Hat Cloud Practitioner", null },
                    { new Guid("9617c867-4031-4883-8889-2c2ef747edf1"), null, "Microsoft IT Technician", null },
                    { new Guid("96e0af6a-bebc-44c4-9f3c-47782d56e041"), null, "PMI Project Manager", null },
                    { new Guid("9b3516b0-76db-42c0-9575-0f81b47a0ecc"), null, "Microsoft Network Associate", null },
                    { new Guid("9cca7b2a-4260-4363-a547-e14589113b7e"), null, "EC-Council System Administrator", null },
                    { new Guid("9e6f23c7-141c-4433-8b99-140b6fe9afad"), null, "ISACA System Administrator", null },
                    { new Guid("a401cc5b-3895-47cd-ac74-2a260566094c"), null, "PMI Security Specialist", null },
                    { new Guid("a452b0dd-926e-4b23-be8f-02fb48481245"), null, "IBM Certified Solutions Architect", null },
                    { new Guid("a5cb53c5-adf6-4615-b022-466a253cae65"), null, "Red Hat Data Engineer", null },
                    { new Guid("a5e65fb4-bf2a-4c18-95a3-9f77e797b7ac"), null, "IBM Network Associate", null },
                    { new Guid("a89c258f-2030-42bf-bb22-8ee64fc8f1ee"), null, "Adobe Cybersecurity Analyst", null },
                    { new Guid("ac3bec3d-845b-49ad-ab84-6f19c9e19b13"), null, "Google Certified Solutions Architect", null },
                    { new Guid("b087e907-50a5-4cd5-af6d-4eefa5b8c486"), null, "IBM System Administrator", null },
                    { new Guid("b1643bea-2313-4f9a-84e9-ab9602c3bb93"), null, "Microsoft System Administrator", null },
                    { new Guid("b413d0a0-9fa6-4a2f-a809-24d2fe0f204a"), null, "ISACA Project Manager", null },
                    { new Guid("b5d340af-31c1-4e2d-b7ba-da1c0358597a"), null, "PMI Certified Developer", null },
                    { new Guid("b60d84c0-597c-4b4e-be13-a943cb162e1f"), null, "Adobe Cloud Practitioner", null },
                    { new Guid("b74c7439-5722-4e14-8f78-3c48c784fa7d"), null, "Cisco Database Administrator", null },
                    { new Guid("b7ad0428-1bd4-4170-b808-70fd5f28f512"), null, "CompTIA Cybersecurity Analyst", null },
                    { new Guid("b7df6ea0-5dd1-4d1c-9f0b-6e4cd050f591"), null, "Microsoft Database Administrator", null },
                    { new Guid("b9cb0d6f-b758-4870-9c7e-16365b356fdf"), null, "Red Hat Network Associate", null },
                    { new Guid("baae285e-b03b-4652-b540-1b57144dec7b"), null, "Red Hat System Administrator", null },
                    { new Guid("bc9b4afa-c305-45e5-b363-f4be48dd8997"), null, "EC-Council Certified Developer", null },
                    { new Guid("bfcc0b63-7b19-4f1e-878e-d8598d632809"), null, "IBM Data Engineer", null },
                    { new Guid("c33c294a-0e97-4607-950d-a40a2a51b825"), null, "IBM Cybersecurity Analyst", null },
                    { new Guid("c6b3ef23-1969-409a-8e04-d911b58df8f5"), null, "Google Cybersecurity Analyst", null },
                    { new Guid("c7c62473-59b7-4b4e-b659-aac885b0b6e6"), null, "PMI Certified Solutions Architect", null },
                    { new Guid("d0f35ee4-9794-43e1-86bb-a37a4dfb4877"), null, "Oracle Certified Solutions Architect", null },
                    { new Guid("d217e4d0-665e-42d8-880f-8990f737a8a5"), null, "PMI IT Technician", null },
                    { new Guid("d38e9b13-7c44-431c-bb35-5134b1ad9e73"), null, "Cisco Certified Developer", null },
                    { new Guid("d7bbe8ae-3790-4d3c-aef2-cfb9a3017b43"), null, "EC-Council Cybersecurity Analyst", null },
                    { new Guid("dd539ebd-92e0-42b5-beae-e5c8c0b08460"), null, "IBM Project Manager", null },
                    { new Guid("e08c0011-87d3-4849-8379-dd9558a13f85"), null, "Red Hat Certified Solutions Architect", null },
                    { new Guid("e542fbc3-51d9-467d-a707-82f24e4ef3c8"), null, "IBM Security Specialist", null },
                    { new Guid("f0371c53-3777-4434-9f83-8f06cd68fa3d"), null, "Microsoft Data Engineer", null },
                    { new Guid("f17f5a7d-7e8d-4fb1-a7bf-951d7b62306c"), null, "Cisco Penetration Tester", null },
                    { new Guid("f224afc1-ce50-4704-8834-844fa0c9bc9d"), null, "AWS Project Manager", null },
                    { new Guid("f3d21817-8f72-426c-9247-c610839ec17d"), null, "ISACA Data Engineer", null },
                    { new Guid("f6705fec-5fa8-4bbe-ad0c-f4bb61810e0c"), null, "EC-Council Security Specialist", null },
                    { new Guid("f6aa0074-c9f4-4a65-a014-ae9465511e3a"), null, "Google Project Manager", null },
                    { new Guid("f6c1de1c-4e34-4f36-857c-4becf536b4e1"), null, "CompTIA Certified Developer", null },
                    { new Guid("f8a12b8b-b90b-4bda-864b-a122c3bde9f9"), null, "AWS Cybersecurity Analyst", null },
                    { new Guid("fbed12c8-0c1f-462a-abc8-a51277703789"), null, "Red Hat Certified Developer", null },
                    { new Guid("fd8d97bd-d305-4167-a734-7a791754dba1"), null, "Adobe Certified Developer", null },
                    { new Guid("fff5827a-221c-48d5-b7da-b338a5c1cadc"), null, "AWS Database Administrator", null }
                });

            migrationBuilder.InsertData(
                table: "LKP_ChartType",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("10b6e1ab-9d90-45ce-81fd-7258db4fae2c"), null, "Radar", null },
                    { new Guid("a5f1f2c3-67bd-41b2-bc0b-f1c7aa4fdab0"), null, "Line", null },
                    { new Guid("b14a8d12-1e01-4d91-b7ae-85f2219f03aa"), null, "Bar", null },
                    { new Guid("c92a1e67-f510-49bb-910d-b331d4f04d47"), null, "Pie", null },
                    { new Guid("de5d14cf-9731-4ea1-8cf3-5b6bc7167b41"), null, "Donut", null }
                });

            migrationBuilder.InsertData(
                table: "LKP_Degree",
                columns: new[] { "ID", "Abbreviation", "Name" },
                values: new object[,]
                {
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d001"), "BSc", "Bachelor of Science" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d002"), "BA", "Bachelor of Arts" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d003"), "MSc", "Master of Science" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d004"), "MBA", "Master of Business Administration" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d005"), "PhD", "Doctor of Philosophy" }
                });

            migrationBuilder.InsertData(
                table: "LKP_FieldOfStudy",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10001"), "Computer Science" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10002"), "Business Administration" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10003"), "Electrical Engineering" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10004"), "Mechanical Engineering" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10005"), "Economics" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10006"), "Cyber Security" }
                });

            migrationBuilder.InsertData(
                table: "LKP_Institution",
                columns: new[] { "ID", "Logo", "Name" },
                values: new object[,]
                {
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c001"), null, "Arab American University" },
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c002"), null, "Bir Zeit University" },
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c003"), null, "University of Oxford" },
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c004"), null, "Üsküdar Üniversitesi" }
                });

            migrationBuilder.InsertData(
                table: "LKP_Preference",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01278046-dcf6-4c39-a256-32f52e0b6eeb"), null, "show-experience-radar-chart", null },
                    { new Guid("0ddf9055-fd64-4c9a-84f0-5d8db3db17a0"), null, "show-overview-bar-chart", null },
                    { new Guid("3004c55b-16b9-4fa2-bbe5-fbd26aa31497"), null, "show-project-radar-chart", null },
                    { new Guid("3055cce6-4022-4c2c-87cf-2ea06b9e7d2d"), null, "show-overview-widget", null },
                    { new Guid("3f87d7a5-5ab8-4ea6-85c7-eed6bb83dcb0"), null, "show-education-radar-chart", null },
                    { new Guid("4073e1e3-3d59-4f12-ae90-31f2d20cf68b"), null, "profile-width", null },
                    { new Guid("435a47b5-43c4-4c0f-91ed-7d6a32ae5398"), null, "show-overview-pie-chart", null },
                    { new Guid("491b6c0a-7f16-4c01-b3fd-5010ff4b6072"), null, "birthdate-format", null },
                    { new Guid("6d83cb36-fd8e-4fd2-87d2-4d4d9b9e4f27"), null, "show-experience-bar-chart", null },
                    { new Guid("6f1f71a6-74b1-4ed3-b2ae-4d1137dbcb8d"), null, "show-language-bar-chart", null },
                    { new Guid("8e4d5b5f-3f44-49a8-83c2-d4c3c5155e63"), null, "show-birthdate", null },
                    { new Guid("8f9e7e6b-6f49-420e-8fd2-3ea35aa9d5b0"), null, "profile-picture-position", null },
                    { new Guid("95b5f7ec-e1c2-446f-8401-e0a982a6172e"), null, "show-experience-pie-chart", null },
                    { new Guid("9d7a1776-99d6-4206-8d0d-1a22365b8a97"), null, "show-skill-pie-chart", null },
                    { new Guid("a68cd4c7-b0fd-4d25-a32f-d7772082ae9c"), null, "show-language-pie-chart", null },
                    { new Guid("acef54ff-49b5-45bb-a84f-0eafce08730c"), null, "show-overview-radar-chart", null },
                    { new Guid("b10f6ef7-35cc-44a3-81b4-d78cc8f5aaf1"), null, "show-skill-widget", null },
                    { new Guid("b65b26c1-d9c7-4089-9ae2-31a2353cf434"), null, "show-project-widget", null },
                    { new Guid("c14d3b4f-62a2-4db1-897c-f3cb3eae3122"), null, "show-skill-bar-chart", null },
                    { new Guid("ca2375d4-d3e4-4dc3-b25c-9dc6fcb03c4e"), null, "show-education-bar-chart", null },
                    { new Guid("cb4c589b-cb07-4414-92f5-98d5c08867a7"), null, "show-education-pie-chart", null },
                    { new Guid("d05c7c4e-c3bb-4422-8ad2-3d10ec961a49"), null, "show-project-pie-chart", null },
                    { new Guid("ec47f4b3-2852-4067-a2e9-0e43b2e7b91b"), null, "show-email-address", null },
                    { new Guid("f1a529dc-99a1-41d1-86bb-bd9d661a9435"), null, "show-language-radar-chart", null },
                    { new Guid("f45d65cf-2f6e-4a42-b25a-11eb326c8f38"), null, "show-phone-number", null },
                    { new Guid("f9ef68e1-f315-4a3c-b3d5-9a53646e75aa"), null, "show-gender", null },
                    { new Guid("fb91d22c-b6cd-4f09-b9a0-7a9633027f49"), null, "show-skill-radar-chart", null },
                    { new Guid("fe5d6427-2ae3-49c5-b94e-8b0e1c361471"), null, "show-project-bar-chart", null }
                });

            migrationBuilder.InsertData(
                table: "LKP_Skill",
                columns: new[] { "ID", "DeletedAt", "IconUrl", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("02a3d389-06b7-4be0-a62f-7aa23e8a2de1"), null, "https://cdn.example.com/icons/css.svg", "CSS", null },
                    { new Guid("30e964e3-b1d1-4890-a632-857c33b22803"), null, "https://cdn.example.com/icons/surgery.svg", "Surgery", null },
                    { new Guid("34db2c3b-59be-4b0f-a988-f816b4e2a82e"), null, "https://cdn.example.com/icons/node-js.svg", "Node.js", null },
                    { new Guid("47b844d2-3ee5-4907-92c3-f09f5a92b3f0"), null, "https://cdn.example.com/icons/sales.svg", "Sales", null },
                    { new Guid("51d71c55-f93a-4b6d-94b5-5425e9f7c026"), null, "https://cdn.example.com/icons/translation.svg", "Translation", null },
                    { new Guid("5476bcee-4d61-4f0a-905f-2fa0f8a5287f"), null, "https://cdn.example.com/icons/html.svg", "HTML", null },
                    { new Guid("69cce10e-9ecf-46e8-a831-b539a1a65149"), null, "https://cdn.example.com/icons/python.svg", "Python", null },
                    { new Guid("6bfb8a3e-1b9f-4d9d-a58d-36d967bc9c01"), null, "https://cdn.example.com/icons/c-sharp.svg", "C#", null },
                    { new Guid("73f9372e-37bb-4703-9936-8f74109aa3f0"), null, "https://cdn.example.com/icons/content-creation.svg", "Content Creation", null },
                    { new Guid("76a5c3f9-5b4e-4d3c-b2b2-481c44500cd4"), null, "https://cdn.example.com/icons/react.svg", "React", null },
                    { new Guid("7dc2f321-70c7-4a6e-8721-3ecf3ae36745"), null, "https://cdn.example.com/icons/law-enforcement.svg", "Law Enforcement", null },
                    { new Guid("908e1c7e-2de7-44f9-b189-146e4c6784e9"), null, "https://cdn.example.com/icons/teaching.svg", "Teaching", null },
                    { new Guid("9d53f924-48c3-4c86-8ac3-1f8d0d013e50"), null, "https://cdn.example.com/icons/graphic-design.svg", "Graphic Design", null },
                    { new Guid("c1b76b91-55ae-47b3-9241-5e6f54b54f4f"), null, "https://cdn.example.com/icons/finance.svg", "Finance", null },
                    { new Guid("c9e6e1fc-5f70-453d-8a23-5fa9b69331e0"), null, "https://cdn.example.com/icons/mongodb.svg", "MongoDB", null },
                    { new Guid("cb84548a-1d9d-47c6-bdb9-01e27c86720d"), null, "https://cdn.example.com/icons/customer-service.svg", "Customer Service", null },
                    { new Guid("cfcaa188-f289-4c33-82ab-7d2f16d4e60f"), null, "https://cdn.example.com/icons/sql.svg", "SQL", null },
                    { new Guid("d87a4b5c-43e6-4762-9f9b-6f7e4dc2c4e0"), null, "https://cdn.example.com/icons/javascript.svg", "JavaScript", null },
                    { new Guid("d8cf53c1-0fa2-4f10-9584-6c879e1420bc"), null, "https://cdn.example.com/icons/radiology.svg", "Radiology", null },
                    { new Guid("f02b09a0-c7a5-4f0c-9e6a-08d7c4f8ef24"), null, "https://cdn.example.com/icons/java.svg", "Java", null }
                });

            migrationBuilder.InsertData(
                table: "LKP_Widget",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("194e6b38-5f1d-4b6f-bf6a-5ac4aaad5b94"), null, "About", null },
                    { new Guid("3ae5b0f3-d26c-4d98-b4ec-5c6f4b1e6f8e"), null, "Contact", null },
                    { new Guid("55c7dd42-07ec-4c5f-aadc-2ad7f3bdfae4"), null, "Language", null },
                    { new Guid("a8d0f22e-d1b3-4d1f-83c7-4e67a345f311"), null, "Education", null },
                    { new Guid("b69e03a3-2fa5-4cb3-8d36-5607c49fd779"), null, "Skill", null },
                    { new Guid("c6d20f43-5ae3-4df3-bf37-e657c26d63aa"), null, "Certification", null },
                    { new Guid("e79c20c5-92a4-47e5-b167-f028f55a364a"), null, "Experience", null },
                    { new Guid("f3b2cf11-6ce0-4e06-b798-1826b8bc67f0"), null, "Project", null }
                });

            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1111111-1111-1111-1111-111111111111"), null, "Admin", null },
                    { new Guid("b2222222-2222-2222-2222-222222222222"), null, "Owner", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_LKP_BlogPostStatusID",
                table: "BlogPost",
                column: "LKP_BlogPostStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_UserID",
                table: "BlogPost",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostTag_TagId",
                table: "BlogPostTag",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_LKP_CertificateID",
                table: "Certificate",
                column: "LKP_CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_UserID",
                table: "Certificate",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateMedia_CertificateID",
                table: "CertificateMedia",
                column: "CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessage_UserID",
                table: "ContactMessage",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_LKP_DegreeID",
                table: "Education",
                column: "LKP_DegreeID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_LKP_FieldOfStudyID",
                table: "Education",
                column: "LKP_FieldOfStudyID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_LKP_InstitutionID",
                table: "Education",
                column: "LKP_InstitutionID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_UserID",
                table: "Education",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_UserID",
                table: "Experience",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_BlogPostStatus_Name",
                table: "LKP_BlogPostStatus",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Certificate_Name",
                table: "LKP_Certificate",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_ChartType_Name",
                table: "LKP_ChartType",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Degree_Name",
                table: "LKP_Degree",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_FieldOfStudy_Name",
                table: "LKP_FieldOfStudy",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Institution_Name",
                table: "LKP_Institution",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Language_Name",
                table: "LKP_Language",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_LanguageProficiency_Level",
                table: "LKP_LanguageProficiency",
                column: "Level",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Preference_Name",
                table: "LKP_Preference",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Widget_Name",
                table: "LKP_Widget",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_UserID",
                table: "PendingEmailConfirmation",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_EducationID",
                table: "Project",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_ExperienceID",
                table: "Project",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserID",
                table: "Project",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Role_Name",
                table: "Role",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialLink_UserID",
                table: "SocialLink",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_RoleID",
                table: "User",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_User_Username",
                table: "User",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserChartPreference_LKP_ChartTypeID",
                table: "UserChartPreference",
                column: "LKP_ChartTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_UserChartPreference_LKP_WidgetID",
                table: "UserChartPreference",
                column: "LKP_WidgetID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_LKP_LanguageID",
                table: "UserLanguage",
                column: "LKP_LanguageID");

            migrationBuilder.CreateIndex(
                name: "IX_UserLanguage_LKP_LanguageProficiencyID",
                table: "UserLanguage",
                column: "LKP_LanguageProficiencyID");

            migrationBuilder.CreateIndex(
                name: "IX_UserPreference_LKP_PreferenceID",
                table: "UserPreference",
                column: "LKP_PreferenceID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_LKP_SkillID",
                table: "UserSkill",
                column: "LKP_SkillID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID",
                table: "UserSkill",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillCertificate_CertificateID",
                table: "UserSkillCertificate",
                column: "CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillEducation_EducationID",
                table: "UserSkillEducation",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillExperience_ExperienceID",
                table: "UserSkillExperience",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillProject_ProjectID",
                table: "UserSkillProject",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPostTag");

            migrationBuilder.DropTable(
                name: "CertificateMedia");

            migrationBuilder.DropTable(
                name: "ContactMessage");

            migrationBuilder.DropTable(
                name: "PendingEmailConfirmation");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "SocialLink");

            migrationBuilder.DropTable(
                name: "UserChartPreference");

            migrationBuilder.DropTable(
                name: "UserLanguage");

            migrationBuilder.DropTable(
                name: "UserPreference");

            migrationBuilder.DropTable(
                name: "UserSkillCertificate");

            migrationBuilder.DropTable(
                name: "UserSkillEducation");

            migrationBuilder.DropTable(
                name: "UserSkillExperience");

            migrationBuilder.DropTable(
                name: "UserSkillProject");

            migrationBuilder.DropTable(
                name: "BlogPost");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "LKP_ChartType");

            migrationBuilder.DropTable(
                name: "LKP_Widget");

            migrationBuilder.DropTable(
                name: "LKP_LanguageProficiency");

            migrationBuilder.DropTable(
                name: "LKP_Language");

            migrationBuilder.DropTable(
                name: "LKP_Preference");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "UserSkill");

            migrationBuilder.DropTable(
                name: "LKP_BlogPostStatus");

            migrationBuilder.DropTable(
                name: "LKP_Certificate");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "LKP_Skill");

            migrationBuilder.DropTable(
                name: "LKP_Degree");

            migrationBuilder.DropTable(
                name: "LKP_FieldOfStudy");

            migrationBuilder.DropTable(
                name: "LKP_Institution");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Role");
        }
    }
}
