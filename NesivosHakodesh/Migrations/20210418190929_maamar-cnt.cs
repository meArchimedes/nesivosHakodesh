using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class maamarcnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maamarim_Torahs_TorahID",
                table: "Maamarim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_TorahID",
                table: "Maamarim");

            migrationBuilder.DropColumn(
                name: "MaarahMakoim",
                table: "Maamarim");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Maamarim");

            migrationBuilder.DropColumn(
                name: "TorahID",
                table: "Maamarim");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Maamarim",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "Maamarim");

            migrationBuilder.AddColumn<string>(
                name: "MaarahMakoim",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TorahID",
                table: "Maamarim",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_TorahID",
                table: "Maamarim",
                column: "TorahID");

            migrationBuilder.AddForeignKey(
                name: "FK_Maamarim_Torahs_TorahID",
                table: "Maamarim",
                column: "TorahID",
                principalTable: "Torahs",
                principalColumn: "TorahID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
