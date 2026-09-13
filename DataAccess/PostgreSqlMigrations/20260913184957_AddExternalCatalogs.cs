using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddExternalCatalogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalID",
                table: "LKP_Skill",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncedAt",
                table: "LKP_Skill",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "LKP_Skill",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Internal");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "LKP_Language",
                type: "character varying(3)",
                maxLength: 3,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalID",
                table: "LKP_Institution",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastSyncedAt",
                table: "LKP_Institution",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "LKP_Institution",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Internal");

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c001"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c002"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c003"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c004"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.InsertData(
                table: "LKP_Language",
                columns: new[] { "ID", "Code", "CreatedAt", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("01666c83-a33c-ca44-64b2-17f2c475c630"), "mr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Marathi", null },
                    { new Guid("02412a5d-65f8-8579-e6db-b67d42d558b7"), "gv", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Manx", null },
                    { new Guid("0568bf40-c1c5-2053-8065-f2674d0c62cb"), "bm", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bamanankan", null },
                    { new Guid("0716c38c-bd70-0c32-c83f-09376d67ce1e"), "be", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Belarusian", null },
                    { new Guid("09281a60-e044-6985-ac9e-6c8c49d412d1"), "da", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Danish", null },
                    { new Guid("0c0701de-8e84-8da6-7121-88e7cf335302"), "eo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Esperanto", null },
                    { new Guid("0c4ebf6a-01f9-2b0f-0b4c-bcd470272769"), "ce", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chechen", null },
                    { new Guid("0d26d66b-246d-77f6-c12d-091354eb655a"), "kl", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kalaallisut", null },
                    { new Guid("0ea537d6-4f46-4ef1-9cb9-4de053dd64cc"), "ug", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Uyghur", null },
                    { new Guid("13e31945-4c89-736f-c0e6-51f41a0b107b"), "he", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hebrew", null },
                    { new Guid("149388f7-35a9-c705-1256-3aa957916761"), "bg", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bulgarian", null },
                    { new Guid("172f403a-ee41-fb03-ab98-1fdfb536535f"), "co", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Corsican", null },
                    { new Guid("19055f4d-e22b-2559-fcea-4f31596a3885"), "ta", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tamil", null },
                    { new Guid("1ae8d923-4fbf-53b2-17a8-bdc588913c14"), "ks", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kashmiri", null },
                    { new Guid("1b0935e0-fdbe-3c7e-9dbd-7945e4c9aff0"), "ca", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Catalan", null },
                    { new Guid("1db0a514-2022-3bfd-f850-86001c4111ab"), "rn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Rundi", null },
                    { new Guid("232ec748-44d5-1f4d-9982-c2e1ece06d7c"), "oc", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Occitan", null },
                    { new Guid("23739250-229d-f06f-ed1e-4f4f28239ee0"), "id", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Indonesian", null },
                    { new Guid("23d0d988-aa73-428e-f20d-34033add11ba"), "yi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Yiddish", null },
                    { new Guid("27124151-7577-8ffa-34b5-95e80239293f"), "ga", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Irish", null },
                    { new Guid("273d8e42-fdc2-08cf-3245-42f224c28d51"), "mt", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Maltese", null },
                    { new Guid("27c8d43f-1323-ae6e-cc5c-36535e71f152"), "dz", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dzongkha", null },
                    { new Guid("2925205e-f397-0160-56df-9e656d5752e3"), "or", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Odia", null },
                    { new Guid("2b0bcb66-75c1-d742-f736-9e3422aa7554"), "mi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Māori", null },
                    { new Guid("2b239181-bfa2-0b65-0343-5111bb93b34c"), "lu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Luba-Katanga", null },
                    { new Guid("2b2c18f4-2231-eeb2-5ad4-5755015b3eda"), "pt", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Portuguese", null },
                    { new Guid("2bf07763-cf6b-fd83-f6ad-6d1214d6caa9"), "iu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Inuktitut", null },
                    { new Guid("2de6e135-3c73-b000-90db-0efc73640da0"), "ii", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Yi", null },
                    { new Guid("2e495d1f-7f50-6ba7-ca2e-af21c2f279b9"), "en", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "English", null },
                    { new Guid("2e88e4c8-80fb-4ed0-8e3b-a074db195918"), "wo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Wolof", null },
                    { new Guid("3053e241-7840-990b-8388-e424155602ad"), "aa", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Afar", null },
                    { new Guid("32ce95d8-1bc4-588f-5192-d177f272d333"), "dv", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Divehi", null },
                    { new Guid("33b37d55-017c-1b58-c975-f07b36e3231f"), "jv", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Javanese", null },
                    { new Guid("348dfdd3-0966-d27b-758c-da88cba3b0fc"), "te", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Telugu", null },
                    { new Guid("37c69d0c-b00f-a8d4-073f-7adf4ee6705e"), "gl", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Galician", null },
                    { new Guid("38b2be24-52c8-9524-8869-e513cb193390"), "sw", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kiswahili", null },
                    { new Guid("3975d138-31a1-38ba-4d5f-c55d8d1ddc1a"), "tn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Setswana", null },
                    { new Guid("3a1b4c94-fa69-e433-b936-55debf0a746d"), "ka", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Georgian", null },
                    { new Guid("3b78dbb4-f487-672e-7d11-71d7d45d3820"), "lt", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lithuanian", null },
                    { new Guid("3ba3b16e-251d-db15-8194-64c542a638b5"), "sv", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Swedish", null },
                    { new Guid("3d7a23fa-1afb-c136-e0b6-24dd624b5c3e"), "tr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Turkish", null },
                    { new Guid("3db17afc-6a6e-03b6-e0ca-b978601bbf71"), "ru", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Russian", null },
                    { new Guid("41e25bb0-9203-7ba3-a707-b0f7c781c947"), "ki", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kikuyu", null },
                    { new Guid("451edc4d-d488-95e5-1d68-9195206f2e11"), "lb", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Luxembourgish", null },
                    { new Guid("45ce83c3-b261-bce8-a7de-a1130428d212"), "hr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Croatian", null },
                    { new Guid("4925b021-ffdf-6441-3362-cbd22b92a013"), "ml", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Malayalam", null },
                    { new Guid("4c4e7d1f-63b5-6473-ba8e-441a5ec512e4"), "yo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Yoruba", null },
                    { new Guid("4d6c8b3d-ba16-274f-697c-50c004d07edb"), "is", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Icelandic", null },
                    { new Guid("4de02aca-8313-0e8d-68eb-602eb3e47572"), "la", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Latin", null },
                    { new Guid("4e3598dd-ef0f-49fc-1432-54db744b2755"), "kr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kanuri", null },
                    { new Guid("4e981b99-cee1-55ca-664d-770c8c11232c"), "cu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Church Slavic", null },
                    { new Guid("4fefd557-2bd2-8fa7-7eee-afd964b535e5"), "so", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Somali", null },
                    { new Guid("550cde79-74f3-f955-424d-6c51ad19cb40"), "si", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sinhala", null },
                    { new Guid("55c9fdf0-f339-f7ef-3c15-ab135d7794a5"), "tg", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tajik", null },
                    { new Guid("563910ad-ca26-cc42-a45b-1b7366f606a2"), "pa", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Punjabi", null },
                    { new Guid("56f184a5-b468-a035-4d14-43f95017d182"), "as", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Assamese", null },
                    { new Guid("59800b17-949f-7715-fe18-ff94fb4a2518"), "ts", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Xitsonga", null },
                    { new Guid("5b02b5f9-11fc-c38f-f020-5533f9758e05"), "kn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kannada", null },
                    { new Guid("5e5a8c9e-6538-cba8-d6f2-7800890fb536"), "kk", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kazakh", null },
                    { new Guid("5fb1ef4b-e146-046a-e811-87538c471b83"), "gn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Guarani", null },
                    { new Guid("609b773b-b3c7-4dd0-f53e-50a81de4c12d"), "bs", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bosnian", null },
                    { new Guid("65ee024f-8dcf-ed9b-79a7-2fe5492b7126"), "zu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "isiZulu", null },
                    { new Guid("66524554-7a66-d982-557a-911acc8a2589"), "nl", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dutch", null },
                    { new Guid("66efc68f-376f-afa5-cf27-9ba0923d5fde"), "om", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Oromo", null },
                    { new Guid("68090640-f481-b5e0-5c34-fc3eb434dc25"), "vo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Volapük", null },
                    { new Guid("6a8edadc-22e3-5e7b-3ca0-cee13eb98776"), "ee", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ewe", null },
                    { new Guid("6a9b45b9-d0bb-4526-a403-9592adaca8fb"), "to", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tongan", null },
                    { new Guid("6d2a0d08-ef8a-b4b5-86c3-2dd3198facf5"), "fi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Finnish", null },
                    { new Guid("6dfccef7-9c8d-4c00-e3c1-c235c3f53f44"), "ko", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Korean", null },
                    { new Guid("7009a050-0a57-abfb-2e90-a042334d1608"), "my", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Burmese", null },
                    { new Guid("71d41c6c-e1d2-10b3-9b94-ff3daa8f06d6"), "sr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Serbian", null },
                    { new Guid("71e2e60d-dff9-b2bc-2e88-cb506e18b7de"), "pl", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Polish", null },
                    { new Guid("737ecc91-4aec-5dfa-2bbd-9bd423def01d"), "es", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Spanish", null },
                    { new Guid("7439cd29-58e4-57b7-4b76-448d486e8f4e"), "ps", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Pashto", null },
                    { new Guid("74779e03-4aef-27d7-c733-834154886d36"), "nn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Norwegian Nynorsk", null },
                    { new Guid("750fbb6f-c937-d1c4-511f-ba75f71d1735"), "ky", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kyrgyz", null },
                    { new Guid("7511fc0d-4020-d8e1-49dd-7686867d7423"), "ur", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Urdu", null },
                    { new Guid("76a0a90f-73d2-db9c-65c8-82e5cc6a8888"), "nb", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Norwegian Bokmål", null },
                    { new Guid("77a96c09-3901-fb6b-fecb-4e15b4864821"), "se", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Northern Sami", null },
                    { new Guid("7b9e4e6d-0f20-2087-de07-4bd28fa46da4"), "el", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Greek", null },
                    { new Guid("7e3e8d6a-e607-ce2d-3e2c-49ce688b0290"), "qu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Quechua", null },
                    { new Guid("7ea38d41-5ba2-904d-d73c-3cd91c68024a"), "ia", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Interlingua", null },
                    { new Guid("7ebd95f1-0eaa-4320-0c92-0a82b588431b"), "cv", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chuvash", null },
                    { new Guid("7f321844-1438-8bc7-f8cb-6cd5a2a886af"), "ve", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Venda", null },
                    { new Guid("7fbe57e9-aab6-16c4-b47a-0f0dfbd41e97"), "hu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hungarian", null },
                    { new Guid("80228530-2d6d-a850-403d-eeeed31c4663"), "gd", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Scottish Gaelic", null },
                    { new Guid("82cb16af-5e50-902f-f520-a3cc8c14271d"), "br", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Breton", null },
                    { new Guid("83e83ec9-f52c-a40a-c7e5-8494e03ee8ca"), "tk", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Turkmen", null },
                    { new Guid("87305288-53ee-8e2d-117d-68b1442b1c34"), "bn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bangla", null },
                    { new Guid("8ced07a5-aa4f-48bb-c53b-898b1c8630b6"), "zh", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Chinese", null },
                    { new Guid("8d72cc34-7625-95ba-861f-6a2f9d8b6a13"), "hy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Armenian", null },
                    { new Guid("8e6b65a7-94c2-b2e8-feee-fdfb0d3b4ec9"), "tt", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tatar", null },
                    { new Guid("8ec85aa5-1ed8-f48e-5cf2-858db8c1e358"), "sq", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Albanian", null },
                    { new Guid("8f5a6bc3-e166-5603-4b2a-0d43d25fcdd9"), "lg", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ganda", null },
                    { new Guid("916dec0e-cc6c-2fcf-b66e-31243aaf32e4"), "sc", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sardinian", null },
                    { new Guid("955de1ab-4793-143a-b124-3fbec25ba3e8"), "st", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sesotho", null },
                    { new Guid("957016e7-8d4c-8fb4-db2d-3cb4350761ea"), "ar", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Arabic", null },
                    { new Guid("95cab2de-9161-e8c2-118a-32c5dfb0dd17"), "xh", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "isiXhosa", null },
                    { new Guid("99748e02-6a4c-00da-ec9a-b3a0007e3d99"), "fa", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Persian", null },
                    { new Guid("997634ee-724c-66ee-8418-3fd941065324"), "no", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Norwegian", null },
                    { new Guid("9ad2b1af-9e12-e24f-2fe5-6c2b0a78e355"), "nd", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "North Ndebele", null },
                    { new Guid("9c2325e3-7ce4-3af6-2566-71d3b62da064"), "eu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Basque", null },
                    { new Guid("9d5b5d19-dad8-13e9-e74e-1c37392b1cb4"), "km", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Khmer", null },
                    { new Guid("9f7add17-ac09-a50c-f3df-e9483b0cec3c"), "rm", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Romansh", null },
                    { new Guid("a0b2d946-20c1-44d5-7907-3597035d25e7"), "sk", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Slovak", null },
                    { new Guid("a5497136-0963-cd85-d858-7c1517efe9cc"), "gu", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Gujarati", null },
                    { new Guid("a609c7ab-67af-a31d-5c88-69e81de83c61"), "ss", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "siSwati", null },
                    { new Guid("a6f93fe0-cb2e-b7c7-1b79-a64c6cd5ef37"), "th", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Thai", null },
                    { new Guid("ab026035-9ddf-9fdd-321f-71511c179008"), "ne", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Nepali", null },
                    { new Guid("ab3d883a-f6cc-4a6e-a6fd-6ceda2f415df"), "mk", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Macedonian", null },
                    { new Guid("abeb6b2a-752b-aab4-77e9-2803984a68e4"), "ba", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bashkir", null },
                    { new Guid("ac5fe2ae-b2e1-34bc-08ad-248cdf1e15fd"), "ig", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Igbo", null },
                    { new Guid("ae85ae94-d403-a270-9d81-0fb88de0ff62"), "kw", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Cornish", null },
                    { new Guid("af72af52-ebf5-a4f2-9064-92850bdd7b3f"), "mn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mongolian", null },
                    { new Guid("b214f3ca-9f6f-b1fb-d293-5b3e26390af4"), "vi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Vietnamese", null },
                    { new Guid("b2b82ff4-e3f9-5834-a3de-e86509c1eafb"), "su", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sundanese", null },
                    { new Guid("b5ece74a-95fe-3992-b72b-30d5fbaa9f3e"), "ln", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lingala", null },
                    { new Guid("c3899716-4aa7-3aa8-08d9-8a8fa74b9538"), "os", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ossetic", null },
                    { new Guid("c646e7cd-e3ee-c13c-b861-32600cf0f267"), "hi", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hindi", null },
                    { new Guid("c6ea53f1-4863-f5b0-3002-877deedeb949"), "lv", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Latvian", null },
                    { new Guid("c78474d2-7fc4-9fc6-67e4-8743c0b9ba72"), "uk", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Ukrainian", null },
                    { new Guid("c97fff5c-c92c-66fa-3010-2ac0d25f13c3"), "it", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Italian", null },
                    { new Guid("cac33082-f360-71a1-42ef-cba5d53e3b2e"), "sg", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sango", null },
                    { new Guid("cc7a195e-a4c0-65b0-a775-658d2004eba5"), "mg", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Malagasy", null },
                    { new Guid("ccec38f5-7b18-e543-604c-fcf1c33c82a2"), "et", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Estonian", null },
                    { new Guid("ce6b04fe-d579-f225-2263-86c557f9e05c"), "ti", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tigrinya", null },
                    { new Guid("cf5152f6-61f1-6da9-0872-8c9a90fbc416"), "cy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Welsh", null },
                    { new Guid("cf969cf5-951f-2624-7925-eb29ff223787"), "ak", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Akan", null },
                    { new Guid("cff2bc35-cb6e-0ae6-dd6f-443ace97f82b"), "ff", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Fula", null },
                    { new Guid("d11b217f-b281-9b81-3b27-322a5c42773a"), "ha", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Hausa", null },
                    { new Guid("d849268d-4cb8-1321-183e-021a35ac0ca6"), "ms", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Malay", null },
                    { new Guid("da7273a6-09b3-0319-8c0b-739290ba8c82"), "sa", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sanskrit", null },
                    { new Guid("dacc0aa8-cfa1-27fb-d0fe-27d90b4bde7b"), "fy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Western Frisian", null },
                    { new Guid("db085b3d-9f53-898f-6469-e8afdbc0cd03"), "cs", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Czech", null },
                    { new Guid("dd5d8f39-4c6e-22aa-f7ae-8e2d4195828a"), "fr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "French", null },
                    { new Guid("e00ece3c-d84f-cd37-8d3b-e93f8d452427"), "de", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "German", null },
                    { new Guid("e07e7015-96be-754a-aaea-e79c733588a6"), "lo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Lao", null },
                    { new Guid("e0b83068-348a-064e-e4ad-3bc9ac564ab4"), "am", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Amharic", null },
                    { new Guid("e0dfd13b-9c8e-ffbd-f526-882f68acfdff"), "sn", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Shona", null },
                    { new Guid("e0e647a4-c23f-c0ff-2100-0615a70ff6e2"), "uz", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Uzbek", null },
                    { new Guid("e460a47a-c145-07f5-49b7-b645bc0b36b7"), "ja", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Japanese", null },
                    { new Guid("e9772e90-6429-a825-5d2f-116992009031"), "az", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Azerbaijani", null },
                    { new Guid("eec41ad5-5a18-ac1d-bb9c-434655cac594"), "nr", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "South Ndebele", null },
                    { new Guid("ef10e59f-bca5-77d9-d8c2-09fd0b1d2a71"), "sd", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sindhi", null },
                    { new Guid("efdd5b65-8e37-e16b-e135-c4c7b90f432a"), "rw", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Kinyarwanda", null },
                    { new Guid("f3c2b8b8-8f34-63aa-7916-fcd0380bf866"), "sl", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Slovenian", null },
                    { new Guid("f4aa4488-2add-8300-5572-47b9edda0b28"), "ro", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Romanian", null },
                    { new Guid("f75f3fd1-b00f-5989-6bf3-3446ad6e3558"), "bo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Tibetan", null },
                    { new Guid("f8d42243-639e-d8c5-3bdb-a8de83b7de6a"), "af", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Afrikaans", null },
                    { new Guid("fa769284-5d28-c1d6-4c29-2003b633ba6f"), "fo", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Faroese", null }
                });

            migrationBuilder.InsertData(
                table: "LKP_LanguageProficiency",
                columns: new[] { "ID", "CreatedAt", "DeletedAt", "Level", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a1000000-0000-4000-8000-000000000001"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A1 - Beginner", null },
                    { new Guid("a1000000-0000-4000-8000-000000000002"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A2 - Elementary", null },
                    { new Guid("a1000000-0000-4000-8000-000000000003"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "B1 - Intermediate", null },
                    { new Guid("a1000000-0000-4000-8000-000000000004"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "B2 - Upper Intermediate", null },
                    { new Guid("a1000000-0000-4000-8000-000000000005"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "C1 - Advanced", null },
                    { new Guid("a1000000-0000-4000-8000-000000000006"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "C2 - Proficient", null },
                    { new Guid("a1000000-0000-4000-8000-000000000007"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Native or bilingual", null }
                });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("02a3d389-06b7-4be0-a62f-7aa23e8a2de1"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("30e964e3-b1d1-4890-a632-857c33b22803"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("34db2c3b-59be-4b0f-a988-f816b4e2a82e"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("47b844d2-3ee5-4907-92c3-f09f5a92b3f0"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("51d71c55-f93a-4b6d-94b5-5425e9f7c026"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("5476bcee-4d61-4f0a-905f-2fa0f8a5287f"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("69cce10e-9ecf-46e8-a831-b539a1a65149"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("6bfb8a3e-1b9f-4d9d-a58d-36d967bc9c01"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("73f9372e-37bb-4703-9936-8f74109aa3f0"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("76a5c3f9-5b4e-4d3c-b2b2-481c44500cd4"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("7dc2f321-70c7-4a6e-8721-3ecf3ae36745"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("908e1c7e-2de7-44f9-b189-146e4c6784e9"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("9d53f924-48c3-4c86-8ac3-1f8d0d013e50"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("c1b76b91-55ae-47b3-9241-5e6f54b54f4f"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("c9e6e1fc-5f70-453d-8a23-5fa9b69331e0"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("cb84548a-1d9d-47c6-bdb9-01e27c86720d"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("cfcaa188-f289-4c33-82ab-7d2f16d4e60f"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("d87a4b5c-43e6-4762-9f9b-6f7e4dc2c4e0"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("d8cf53c1-0fa2-4f10-9584-6c879e1420bc"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("f02b09a0-c7a5-4f0c-9e6a-08d7c4f8ef24"),
                columns: new[] { "ExternalID", "LastSyncedAt", "Source" },
                values: new object[] { null, null, "Internal" });

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Skill_Source_ExternalID",
                table: "LKP_Skill",
                columns: new[] { "Source", "ExternalID" },
                unique: true,
                filter: "\"ExternalID\" IS NOT NULL AND \"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Language_Code",
                table: "LKP_Language",
                column: "Code",
                unique: true,
                filter: "\"Code\" IS NOT NULL AND \"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Institution_Source_ExternalID",
                table: "LKP_Institution",
                columns: new[] { "Source", "ExternalID" },
                unique: true,
                filter: "\"ExternalID\" IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LKP_Skill_Source_ExternalID",
                table: "LKP_Skill");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Language_Code",
                table: "LKP_Language");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Institution_Source_ExternalID",
                table: "LKP_Institution");

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("01666c83-a33c-ca44-64b2-17f2c475c630"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("02412a5d-65f8-8579-e6db-b67d42d558b7"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("0568bf40-c1c5-2053-8065-f2674d0c62cb"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("0716c38c-bd70-0c32-c83f-09376d67ce1e"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("09281a60-e044-6985-ac9e-6c8c49d412d1"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("0c0701de-8e84-8da6-7121-88e7cf335302"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("0c4ebf6a-01f9-2b0f-0b4c-bcd470272769"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("0d26d66b-246d-77f6-c12d-091354eb655a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("0ea537d6-4f46-4ef1-9cb9-4de053dd64cc"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("13e31945-4c89-736f-c0e6-51f41a0b107b"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("149388f7-35a9-c705-1256-3aa957916761"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("172f403a-ee41-fb03-ab98-1fdfb536535f"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("19055f4d-e22b-2559-fcea-4f31596a3885"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("1ae8d923-4fbf-53b2-17a8-bdc588913c14"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("1b0935e0-fdbe-3c7e-9dbd-7945e4c9aff0"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("1db0a514-2022-3bfd-f850-86001c4111ab"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("232ec748-44d5-1f4d-9982-c2e1ece06d7c"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("23739250-229d-f06f-ed1e-4f4f28239ee0"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("23d0d988-aa73-428e-f20d-34033add11ba"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("27124151-7577-8ffa-34b5-95e80239293f"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("273d8e42-fdc2-08cf-3245-42f224c28d51"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("27c8d43f-1323-ae6e-cc5c-36535e71f152"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2925205e-f397-0160-56df-9e656d5752e3"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2b0bcb66-75c1-d742-f736-9e3422aa7554"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2b239181-bfa2-0b65-0343-5111bb93b34c"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2b2c18f4-2231-eeb2-5ad4-5755015b3eda"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2bf07763-cf6b-fd83-f6ad-6d1214d6caa9"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2de6e135-3c73-b000-90db-0efc73640da0"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2e495d1f-7f50-6ba7-ca2e-af21c2f279b9"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("2e88e4c8-80fb-4ed0-8e3b-a074db195918"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3053e241-7840-990b-8388-e424155602ad"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("32ce95d8-1bc4-588f-5192-d177f272d333"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("33b37d55-017c-1b58-c975-f07b36e3231f"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("348dfdd3-0966-d27b-758c-da88cba3b0fc"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("37c69d0c-b00f-a8d4-073f-7adf4ee6705e"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("38b2be24-52c8-9524-8869-e513cb193390"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3975d138-31a1-38ba-4d5f-c55d8d1ddc1a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3a1b4c94-fa69-e433-b936-55debf0a746d"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3b78dbb4-f487-672e-7d11-71d7d45d3820"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3ba3b16e-251d-db15-8194-64c542a638b5"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3d7a23fa-1afb-c136-e0b6-24dd624b5c3e"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("3db17afc-6a6e-03b6-e0ca-b978601bbf71"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("41e25bb0-9203-7ba3-a707-b0f7c781c947"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("451edc4d-d488-95e5-1d68-9195206f2e11"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("45ce83c3-b261-bce8-a7de-a1130428d212"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4925b021-ffdf-6441-3362-cbd22b92a013"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4c4e7d1f-63b5-6473-ba8e-441a5ec512e4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4d6c8b3d-ba16-274f-697c-50c004d07edb"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4de02aca-8313-0e8d-68eb-602eb3e47572"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4e3598dd-ef0f-49fc-1432-54db744b2755"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4e981b99-cee1-55ca-664d-770c8c11232c"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("4fefd557-2bd2-8fa7-7eee-afd964b535e5"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("550cde79-74f3-f955-424d-6c51ad19cb40"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("55c9fdf0-f339-f7ef-3c15-ab135d7794a5"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("563910ad-ca26-cc42-a45b-1b7366f606a2"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("56f184a5-b468-a035-4d14-43f95017d182"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("59800b17-949f-7715-fe18-ff94fb4a2518"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("5b02b5f9-11fc-c38f-f020-5533f9758e05"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("5e5a8c9e-6538-cba8-d6f2-7800890fb536"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("5fb1ef4b-e146-046a-e811-87538c471b83"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("609b773b-b3c7-4dd0-f53e-50a81de4c12d"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("65ee024f-8dcf-ed9b-79a7-2fe5492b7126"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("66524554-7a66-d982-557a-911acc8a2589"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("66efc68f-376f-afa5-cf27-9ba0923d5fde"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("68090640-f481-b5e0-5c34-fc3eb434dc25"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("6a8edadc-22e3-5e7b-3ca0-cee13eb98776"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("6a9b45b9-d0bb-4526-a403-9592adaca8fb"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("6d2a0d08-ef8a-b4b5-86c3-2dd3198facf5"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("6dfccef7-9c8d-4c00-e3c1-c235c3f53f44"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7009a050-0a57-abfb-2e90-a042334d1608"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("71d41c6c-e1d2-10b3-9b94-ff3daa8f06d6"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("71e2e60d-dff9-b2bc-2e88-cb506e18b7de"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("737ecc91-4aec-5dfa-2bbd-9bd423def01d"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7439cd29-58e4-57b7-4b76-448d486e8f4e"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("74779e03-4aef-27d7-c733-834154886d36"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("750fbb6f-c937-d1c4-511f-ba75f71d1735"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7511fc0d-4020-d8e1-49dd-7686867d7423"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("76a0a90f-73d2-db9c-65c8-82e5cc6a8888"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("77a96c09-3901-fb6b-fecb-4e15b4864821"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7b9e4e6d-0f20-2087-de07-4bd28fa46da4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7e3e8d6a-e607-ce2d-3e2c-49ce688b0290"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7ea38d41-5ba2-904d-d73c-3cd91c68024a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7ebd95f1-0eaa-4320-0c92-0a82b588431b"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7f321844-1438-8bc7-f8cb-6cd5a2a886af"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("7fbe57e9-aab6-16c4-b47a-0f0dfbd41e97"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("80228530-2d6d-a850-403d-eeeed31c4663"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("82cb16af-5e50-902f-f520-a3cc8c14271d"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("83e83ec9-f52c-a40a-c7e5-8494e03ee8ca"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("87305288-53ee-8e2d-117d-68b1442b1c34"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("8ced07a5-aa4f-48bb-c53b-898b1c8630b6"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("8d72cc34-7625-95ba-861f-6a2f9d8b6a13"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("8e6b65a7-94c2-b2e8-feee-fdfb0d3b4ec9"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("8ec85aa5-1ed8-f48e-5cf2-858db8c1e358"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("8f5a6bc3-e166-5603-4b2a-0d43d25fcdd9"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("916dec0e-cc6c-2fcf-b66e-31243aaf32e4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("955de1ab-4793-143a-b124-3fbec25ba3e8"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("957016e7-8d4c-8fb4-db2d-3cb4350761ea"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("95cab2de-9161-e8c2-118a-32c5dfb0dd17"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("99748e02-6a4c-00da-ec9a-b3a0007e3d99"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("997634ee-724c-66ee-8418-3fd941065324"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("9ad2b1af-9e12-e24f-2fe5-6c2b0a78e355"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("9c2325e3-7ce4-3af6-2566-71d3b62da064"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("9d5b5d19-dad8-13e9-e74e-1c37392b1cb4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("9f7add17-ac09-a50c-f3df-e9483b0cec3c"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("a0b2d946-20c1-44d5-7907-3597035d25e7"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("a5497136-0963-cd85-d858-7c1517efe9cc"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("a609c7ab-67af-a31d-5c88-69e81de83c61"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("a6f93fe0-cb2e-b7c7-1b79-a64c6cd5ef37"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ab026035-9ddf-9fdd-321f-71511c179008"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ab3d883a-f6cc-4a6e-a6fd-6ceda2f415df"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("abeb6b2a-752b-aab4-77e9-2803984a68e4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ac5fe2ae-b2e1-34bc-08ad-248cdf1e15fd"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ae85ae94-d403-a270-9d81-0fb88de0ff62"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("af72af52-ebf5-a4f2-9064-92850bdd7b3f"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("b214f3ca-9f6f-b1fb-d293-5b3e26390af4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("b2b82ff4-e3f9-5834-a3de-e86509c1eafb"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("b5ece74a-95fe-3992-b72b-30d5fbaa9f3e"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("c3899716-4aa7-3aa8-08d9-8a8fa74b9538"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("c646e7cd-e3ee-c13c-b861-32600cf0f267"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("c6ea53f1-4863-f5b0-3002-877deedeb949"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("c78474d2-7fc4-9fc6-67e4-8743c0b9ba72"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("c97fff5c-c92c-66fa-3010-2ac0d25f13c3"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("cac33082-f360-71a1-42ef-cba5d53e3b2e"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("cc7a195e-a4c0-65b0-a775-658d2004eba5"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ccec38f5-7b18-e543-604c-fcf1c33c82a2"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ce6b04fe-d579-f225-2263-86c557f9e05c"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("cf5152f6-61f1-6da9-0872-8c9a90fbc416"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("cf969cf5-951f-2624-7925-eb29ff223787"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("cff2bc35-cb6e-0ae6-dd6f-443ace97f82b"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("d11b217f-b281-9b81-3b27-322a5c42773a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("d849268d-4cb8-1321-183e-021a35ac0ca6"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("da7273a6-09b3-0319-8c0b-739290ba8c82"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("dacc0aa8-cfa1-27fb-d0fe-27d90b4bde7b"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("db085b3d-9f53-898f-6469-e8afdbc0cd03"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("dd5d8f39-4c6e-22aa-f7ae-8e2d4195828a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e00ece3c-d84f-cd37-8d3b-e93f8d452427"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e07e7015-96be-754a-aaea-e79c733588a6"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e0b83068-348a-064e-e4ad-3bc9ac564ab4"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e0dfd13b-9c8e-ffbd-f526-882f68acfdff"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e0e647a4-c23f-c0ff-2100-0615a70ff6e2"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e460a47a-c145-07f5-49b7-b645bc0b36b7"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("e9772e90-6429-a825-5d2f-116992009031"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("eec41ad5-5a18-ac1d-bb9c-434655cac594"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("ef10e59f-bca5-77d9-d8c2-09fd0b1d2a71"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("efdd5b65-8e37-e16b-e135-c4c7b90f432a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("f3c2b8b8-8f34-63aa-7916-fcd0380bf866"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("f4aa4488-2add-8300-5572-47b9edda0b28"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("f75f3fd1-b00f-5989-6bf3-3446ad6e3558"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("f8d42243-639e-d8c5-3bdb-a8de83b7de6a"));

            migrationBuilder.DeleteData(
                table: "LKP_Language",
                keyColumn: "ID",
                keyValue: new Guid("fa769284-5d28-c1d6-4c29-2003b633ba6f"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000001"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000002"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000003"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000004"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000005"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000006"));

            migrationBuilder.DeleteData(
                table: "LKP_LanguageProficiency",
                keyColumn: "ID",
                keyValue: new Guid("a1000000-0000-4000-8000-000000000007"));

            migrationBuilder.DropColumn(
                name: "ExternalID",
                table: "LKP_Skill");

            migrationBuilder.DropColumn(
                name: "LastSyncedAt",
                table: "LKP_Skill");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "LKP_Skill");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "LKP_Language");

            migrationBuilder.DropColumn(
                name: "ExternalID",
                table: "LKP_Institution");

            migrationBuilder.DropColumn(
                name: "LastSyncedAt",
                table: "LKP_Institution");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "LKP_Institution");
        }
    }
}
