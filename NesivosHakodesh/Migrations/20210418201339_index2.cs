using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class index2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Torahs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parsha",
                table: "Torahs",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Sources",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Sources",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sefurim",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Maamarim",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Maamarim",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Torahs_Parsha",
                table: "Torahs",
                column: "Parsha");

            migrationBuilder.CreateIndex(
                name: "IX_Torahs_Status",
                table: "Torahs",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Torahs_Title",
                table: "Torahs",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_FirstName",
                table: "Sources",
                column: "FirstName");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_LastName",
                table: "Sources",
                column: "LastName");

            migrationBuilder.CreateIndex(
                name: "IX_Sefurim_Name",
                table: "Sefurim",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_Date",
                table: "Maamarim",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_Title",
                table: "Maamarim",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_Year",
                table: "Maamarim",
                column: "Year");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Torahs_Parsha",
                table: "Torahs");

            migrationBuilder.DropIndex(
                name: "IX_Torahs_Status",
                table: "Torahs");

            migrationBuilder.DropIndex(
                name: "IX_Torahs_Title",
                table: "Torahs");

            migrationBuilder.DropIndex(
                name: "IX_Sources_FirstName",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Sources_LastName",
                table: "Sources");

            migrationBuilder.DropIndex(
                name: "IX_Sefurim_Name",
                table: "Sefurim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_Date",
                table: "Maamarim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_Title",
                table: "Maamarim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_Year",
                table: "Maamarim");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Torahs",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Parsha",
                table: "Torahs",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "Sources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "Sources",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Sefurim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Year",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
