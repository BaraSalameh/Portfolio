using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AppendOwnerCollectionOrdering : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                SET LOCAL lock_timeout = '5s';
                SET LOCAL statement_timeout = '5min';

                CREATE TABLE "OwnerCollectionOrder" (
                    "UserID" uuid NOT NULL,
                    "Collection" character varying(32) NOT NULL,
                    "LastOrder" integer NOT NULL,
                    CONSTRAINT "PK_OwnerCollectionOrder" PRIMARY KEY ("UserID", "Collection"),
                    CONSTRAINT "FK_OwnerCollectionOrder_User_UserID"
                        FOREIGN KEY ("UserID") REFERENCES "User" ("ID") ON DELETE RESTRICT,
                    CONSTRAINT "CK_OwnerCollectionOrder_LastOrder" CHECK ("LastOrder" >= 0),
                    CONSTRAINT "CK_OwnerCollectionOrder_Collection"
                        CHECK ("Collection" IN ('Project', 'Education', 'Experience', 'Certificate'))
                );

                INSERT INTO "OwnerCollectionOrder" ("UserID", "Collection", "LastOrder")
                SELECT "UserID", 'Project', GREATEST(COALESCE(MAX("Order"), 0), 0)
                FROM "Project"
                GROUP BY "UserID";

                INSERT INTO "OwnerCollectionOrder" ("UserID", "Collection", "LastOrder")
                SELECT "UserID", 'Education', GREATEST(COALESCE(MAX("Order"), 0), 0)
                FROM "Education"
                GROUP BY "UserID";

                INSERT INTO "OwnerCollectionOrder" ("UserID", "Collection", "LastOrder")
                SELECT "UserID", 'Experience', GREATEST(COALESCE(MAX("Order"), 0), 0)
                FROM "Experience"
                GROUP BY "UserID";

                INSERT INTO "OwnerCollectionOrder" ("UserID", "Collection", "LastOrder")
                SELECT "UserID", 'Certificate', GREATEST(COALESCE(MAX("Order"), 0), 0)
                FROM "Certificate"
                GROUP BY "UserID";

                CREATE FUNCTION "AssignOwnerCollectionOrder"() RETURNS trigger
                LANGUAGE plpgsql AS $$
                BEGIN
                    IF NEW."Order" <= 0 THEN
                        INSERT INTO "OwnerCollectionOrder" ("UserID", "Collection", "LastOrder")
                        VALUES (NEW."UserID", TG_TABLE_NAME, 1)
                        ON CONFLICT ("UserID", "Collection") DO UPDATE
                        SET "LastOrder" = "OwnerCollectionOrder"."LastOrder" + 1
                        RETURNING "LastOrder" INTO NEW."Order";
                    ELSE
                        INSERT INTO "OwnerCollectionOrder" ("UserID", "Collection", "LastOrder")
                        VALUES (NEW."UserID", TG_TABLE_NAME, NEW."Order")
                        ON CONFLICT ("UserID", "Collection") DO UPDATE
                        SET "LastOrder" = GREATEST(
                            "OwnerCollectionOrder"."LastOrder",
                            EXCLUDED."LastOrder");
                    END IF;

                    RETURN NEW;
                END;
                $$;

                CREATE TRIGGER "TR_Project_AssignOwnerCollectionOrder"
                BEFORE INSERT ON "Project"
                FOR EACH ROW EXECUTE FUNCTION "AssignOwnerCollectionOrder"();

                CREATE TRIGGER "TR_Education_AssignOwnerCollectionOrder"
                BEFORE INSERT ON "Education"
                FOR EACH ROW EXECUTE FUNCTION "AssignOwnerCollectionOrder"();

                CREATE TRIGGER "TR_Experience_AssignOwnerCollectionOrder"
                BEFORE INSERT ON "Experience"
                FOR EACH ROW EXECUTE FUNCTION "AssignOwnerCollectionOrder"();

                CREATE TRIGGER "TR_Certificate_AssignOwnerCollectionOrder"
                BEFORE INSERT ON "Certificate"
                FOR EACH ROW EXECUTE FUNCTION "AssignOwnerCollectionOrder"();
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                SET LOCAL lock_timeout = '5s';
                SET LOCAL statement_timeout = '5min';

                DROP TRIGGER IF EXISTS "TR_Project_AssignOwnerCollectionOrder" ON "Project";
                DROP TRIGGER IF EXISTS "TR_Education_AssignOwnerCollectionOrder" ON "Education";
                DROP TRIGGER IF EXISTS "TR_Experience_AssignOwnerCollectionOrder" ON "Experience";
                DROP TRIGGER IF EXISTS "TR_Certificate_AssignOwnerCollectionOrder" ON "Certificate";
                DROP FUNCTION IF EXISTS "AssignOwnerCollectionOrder"();
                DROP TABLE IF EXISTS "OwnerCollectionOrder";
                """);
        }
    }
}
