using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class index : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Parsha",
                table: "Maamarim",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_Parsha",
                table: "Maamarim",
                column: "Parsha");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_Status",
                table: "Maamarim",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_Type",
                table: "Maamarim",
                column: "Type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Maamarim_Parsha",
                table: "Maamarim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_Status",
                table: "Maamarim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_Type",
                table: "Maamarim");

            migrationBuilder.AlterColumn<string>(
                name: "Parsha",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
