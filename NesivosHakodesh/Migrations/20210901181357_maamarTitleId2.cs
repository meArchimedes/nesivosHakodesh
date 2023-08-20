using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class maamarTitleId2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiberyTitle",
                table: "Maamarim");

            migrationBuilder.AddColumn<string>(
                name: "LiberyTitleId",
                table: "Maamarim",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiberyTitleId",
                table: "Maamarim");

            migrationBuilder.AddColumn<string>(
                name: "LiberyTitle",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
