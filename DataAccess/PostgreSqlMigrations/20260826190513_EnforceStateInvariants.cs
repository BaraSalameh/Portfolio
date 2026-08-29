using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class EnforceStateInvariants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                SET LOCAL lock_timeout = '5s';
                SET LOCAL statement_timeout = '5min';

                ALTER TABLE "User" ADD CONSTRAINT "CK_User_Gender"
                    CHECK ("Gender" IS NULL OR "Gender" BETWEEN 0 AND 2) NOT VALID;
                ALTER TABLE "Project" ADD CONSTRAINT "CK_Project_Order"
                    CHECK ("Order" >= 0) NOT VALID;
                ALTER TABLE "Experience" ADD CONSTRAINT "CK_Experience_DateRange"
                    CHECK ("EndDate" IS NULL OR "EndDate" >= "StartDate") NOT VALID;
                ALTER TABLE "Experience" ADD CONSTRAINT "CK_Experience_Order"
                    CHECK ("Order" >= 0) NOT VALID;
                ALTER TABLE "EmailOutboxMessage" ADD CONSTRAINT "CK_EmailOutboxMessage_AttemptCount"
                    CHECK ("AttemptCount" BETWEEN 0 AND 5) NOT VALID;
                ALTER TABLE "EmailOutboxMessage" ADD CONSTRAINT "CK_EmailOutboxMessage_Kind"
                    CHECK ("Kind" IN (1, 2)) NOT VALID;
                ALTER TABLE "EmailOutboxMessage" ADD CONSTRAINT "CK_EmailOutboxMessage_LeasePair"
                    CHECK (("LockID" IS NULL) = ("LockedUntil" IS NULL)) NOT VALID;
                ALTER TABLE "EmailOutboxMessage" ADD CONSTRAINT "CK_EmailOutboxMessage_ProcessedLease"
                    CHECK ("ProcessedAt" IS NULL OR "LockID" IS NULL) NOT VALID;
                ALTER TABLE "Education" ADD CONSTRAINT "CK_Education_DateRange"
                    CHECK ("EndDate" IS NULL OR "EndDate" >= "StartDate") NOT VALID;
                ALTER TABLE "Education" ADD CONSTRAINT "CK_Education_Order"
                    CHECK ("Order" >= 0) NOT VALID;
                ALTER TABLE "Certificate" ADD CONSTRAINT "CK_Certificate_DateRange"
                    CHECK ("IssueDate" IS NULL OR "ExpirationDate" IS NULL OR "ExpirationDate" >= "IssueDate") NOT VALID;
                ALTER TABLE "Certificate" ADD CONSTRAINT "CK_Certificate_Order"
                    CHECK ("Order" >= 0) NOT VALID;

                ALTER TABLE "User" VALIDATE CONSTRAINT "CK_User_Gender";
                ALTER TABLE "Project" VALIDATE CONSTRAINT "CK_Project_Order";
                ALTER TABLE "Experience" VALIDATE CONSTRAINT "CK_Experience_DateRange";
                ALTER TABLE "Experience" VALIDATE CONSTRAINT "CK_Experience_Order";
                ALTER TABLE "EmailOutboxMessage" VALIDATE CONSTRAINT "CK_EmailOutboxMessage_AttemptCount";
                ALTER TABLE "EmailOutboxMessage" VALIDATE CONSTRAINT "CK_EmailOutboxMessage_Kind";
                ALTER TABLE "EmailOutboxMessage" VALIDATE CONSTRAINT "CK_EmailOutboxMessage_LeasePair";
                ALTER TABLE "EmailOutboxMessage" VALIDATE CONSTRAINT "CK_EmailOutboxMessage_ProcessedLease";
                ALTER TABLE "Education" VALIDATE CONSTRAINT "CK_Education_DateRange";
                ALTER TABLE "Education" VALIDATE CONSTRAINT "CK_Education_Order";
                ALTER TABLE "Certificate" VALIDATE CONSTRAINT "CK_Certificate_DateRange";
                ALTER TABLE "Certificate" VALIDATE CONSTRAINT "CK_Certificate_Order";
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_User_Gender",
                table: "User");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Project_Order",
                table: "Project");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Experience_DateRange",
                table: "Experience");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Experience_Order",
                table: "Experience");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmailOutboxMessage_AttemptCount",
                table: "EmailOutboxMessage");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmailOutboxMessage_Kind",
                table: "EmailOutboxMessage");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmailOutboxMessage_LeasePair",
                table: "EmailOutboxMessage");

            migrationBuilder.DropCheckConstraint(
                name: "CK_EmailOutboxMessage_ProcessedLease",
                table: "EmailOutboxMessage");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Education_DateRange",
                table: "Education");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Education_Order",
                table: "Education");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Certificate_DateRange",
                table: "Certificate");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Certificate_Order",
                table: "Certificate");
        }
    }
}
