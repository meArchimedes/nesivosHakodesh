using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class Roles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Role",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "240ec39e-b70f-4d8f-8f0a-2ce72dabb800", "ADMIN", null },
                    { 23, "4a4ab5f2-200b-4355-8e6b-de7d075c6c8d", "MAAMARIM_3_EDIT", null },
                    { 22, "a837a96c-686d-4360-991b-15231d35d83b", "MAAMARIM_3_VIEW", null },
                    { 21, "167dd656-d606-4f26-a8dc-4c3b704aec90", "MAAMARIM_2_DOWNLOAD", null },
                    { 20, "63048700-9170-460c-997e-594442f5eca9", "MAAMARIM_2_PRINT", null },
                    { 19, "ef21134f-53eb-49c2-a1b2-baa845e66291", "MAAMARIM_2_EDIT", null },
                    { 18, "a8c6e314-c498-4d3a-afd6-5440515abb7a", "MAAMARIM_2_VIEW", null },
                    { 17, "3794d193-1327-41a2-a208-507dde9468d6", "MAAMARIM_1_DOWNLOAD", null },
                    { 16, "fc519a30-3d98-41b4-a5eb-fc588fc26210", "MAAMARIM_1_PRINT", null },
                    { 15, "b359a64a-f806-45ad-9997-03677dfc7302", "MAAMARIM_1_EDIT", null },
                    { 14, "9396d909-b5a9-40bd-91e1-8ccde16d8257", "MAAMARIM_1_VIEW", null },
                    { 24, "9301722a-cd76-43e0-a186-fe25eb103a94", "MAAMARIM_3_PRINT", null },
                    { 13, "41aed1aa-7446-4189-8c40-bedf593dad99", "MAAMARIM_0_DOWNLOAD", null },
                    { 11, "a72fe343-b12b-4c15-b439-86cb08c6d128", "MAAMARIM_0_EDIT", null },
                    { 10, "e78d338d-3098-41a3-bd1b-13df3ac61649", "MAAMARIM_0_VIEW", null },
                    { 9, "8f67ca29-474f-4957-bfd1-707c13c8166a", "TORHAS_EDIT", null },
                    { 8, "b8204480-d1d9-4831-927c-8a3a0150b27c", "TORHAS_VIEW", null },
                    { 7, "f1d00568-5e4f-4aee-984d-6450f32dc156", "SOURCES_PRS_EDIT", null },
                    { 6, "00ec56be-7e3c-4d6b-931a-4bba3dc241ac", "SOURCES_PRS_VIEW", null },
                    { 5, "d84fb213-86e5-439c-af9c-a038efdce096", "SOURCES_REG_EDIT", null },
                    { 4, "1927be09-b15f-4656-ba1c-ec3fcfc80197", "SOURCES_REG_VIEW", null },
                    { 3, "6d3e98d8-abb7-489a-ab86-bd46e4a20fc4", "TOPICS_EDIT", null },
                    { 2, "bb26e05f-8ab3-491a-8a68-08a1d6f2e881", "TOPICS_VIEW", null },
                    { 12, "28d922fc-6cb8-47fe-9497-852fc1054064", "MAAMARIM_0_PRINT", null },
                    { 25, "99b62bb3-83f7-4807-9aef-af715b3c7d86", "MAAMARIM_3_DOWNLOAD", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Role",
                keyColumn: "Id",
                keyValue: 25);
        }
    }
}
