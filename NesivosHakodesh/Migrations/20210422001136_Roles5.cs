using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class Roles5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /*
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 26);
            */
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            /*migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { 1, "1434ab56-060f-44d2-9d75-525624ae13e8", "ADMIN", null },
                    { 24, "7a8cb5f0-26f6-49a4-9998-588f43354470", "MAAMARIM_3_PRINT", null },
                    { 23, "448b7f87-c5d9-4194-be55-e59996d6a683", "MAAMARIM_3_EDIT", null },
                    { 22, "616169e8-c1e7-4dcb-8ff5-4de49c0b6238", "MAAMARIM_3_VIEW", null },
                    { 21, "71fc65ff-67e6-418e-9494-6cad02b71b16", "MAAMARIM_2_DOWNLOAD", null },
                    { 20, "96b9d9f3-d3a1-47e9-ac81-294185e7d71f", "MAAMARIM_2_PRINT", null },
                    { 19, "29f971f5-a5de-438c-9916-eff9f45a6609", "MAAMARIM_2_EDIT", null },
                    { 18, "187a7d42-67dd-4944-ab7d-c90929f22c26", "MAAMARIM_2_VIEW", null },
                    { 17, "58915745-02d9-48f6-a775-dc79f96bd90b", "MAAMARIM_1_DOWNLOAD", null },
                    { 16, "dbd10946-8568-48e3-987c-d311fb111c32", "MAAMARIM_1_PRINT", null },
                    { 15, "c5d233b2-c3d9-484e-b278-cc2477344c1d", "MAAMARIM_1_EDIT", null },
                    { 14, "c4ef2569-ca24-4f4e-ba87-9a7500a23451", "MAAMARIM_1_VIEW", null },
                    { 13, "490f5777-3f47-4101-b392-94258a73c397", "MAAMARIM_0_DOWNLOAD", null },
                    { 12, "2bb0f23b-1499-4426-8002-8bad2ea4dee2", "MAAMARIM_0_PRINT", null },
                    { 11, "73001f2d-328f-42e0-822c-316f3b882db3", "MAAMARIM_0_EDIT", null },
                    { 10, "871c9145-f2fe-4854-8de1-062d4710269f", "MAAMARIM_0_VIEW", null },
                    { 9, "bf49ea85-9df1-46e1-8301-c9857631a60e", "TORHAS_EDIT", null },
                    { 8, "44b3b668-8035-4194-881d-d32c7eb30e1f", "TORHAS_VIEW", null },
                    { 7, "0d48623a-fdbf-4865-a379-310aadf648e6", "SOURCES_PRS_EDIT", null },
                    { 6, "14501589-aca2-40b6-8347-f0c40409344a", "SOURCES_PRS_VIEW", null },
                    { 5, "755d5c21-cafa-4fb0-aaa5-ad8f3591ec63", "SOURCES_EDIT", null },
                    { 4, "b77e7050-644b-4bf3-b34f-ed9b66519f54", "SOURCES_VIEW", null },
                    { 3, "690051c3-bef6-40b4-aecc-ae26c6b037bc", "TOPICS_EDIT", null },
                    { 2, "76c7e481-4790-4c14-849c-49afb41d0b32", "TOPICS_VIEW", null },
                    { 25, "dc6679bd-3a62-407b-add3-151de1555ade", "MAAMARIM_3_DOWNLOAD", null },
                    { 26, "e5858795-e5da-440c-9bdc-51174bcacfd1", "MAAMARIM_VIEW", null }
                });*/
        }
    }
}
