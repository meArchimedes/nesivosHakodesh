using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class BechatzrPrinted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "BechatzrPrinted",
                table: "Maamarim",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BechatzrPrintedWeek",
                table: "Maamarim",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BechatzrPrintedWeek",
                table: "Maamarim");

            migrationBuilder.AlterColumn<string>(
                name: "BechatzrPrinted",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(bool));
        }
    }
}
