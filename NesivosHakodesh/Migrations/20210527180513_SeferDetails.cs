using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class SeferDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Sources_LastName",
                table: "Sources");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Sources");

            migrationBuilder.AddColumn<string>(
                name: "AuthorSefer",
                table: "Sefurim",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrintYear",
                table: "Sefurim",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeferDetails",
                table: "Sefurim",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorSefer",
                table: "Sefurim");

            migrationBuilder.DropColumn(
                name: "PrintYear",
                table: "Sefurim");

            migrationBuilder.DropColumn(
                name: "SeferDetails",
                table: "Sefurim");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Sources",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sources_LastName",
                table: "Sources",
                column: "LastName");
        }
    }
}
