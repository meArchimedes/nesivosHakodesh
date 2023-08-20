using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class torahann : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AnnHeight",
                table: "Torahs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnPageNumber",
                table: "Torahs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnWidth",
                table: "Torahs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnX",
                table: "Torahs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AnnY",
                table: "Torahs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AnnHeight",
                table: "Torahs");

            migrationBuilder.DropColumn(
                name: "AnnPageNumber",
                table: "Torahs");

            migrationBuilder.DropColumn(
                name: "AnnWidth",
                table: "Torahs");

            migrationBuilder.DropColumn(
                name: "AnnX",
                table: "Torahs");

            migrationBuilder.DropColumn(
                name: "AnnY",
                table: "Torahs");
        }
    }
}
