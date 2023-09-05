using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class _20230816_1801 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsTitle",
                table: "MaamarLibraryLinks");

            migrationBuilder.RenameColumn(
                name: "LiberyTitleIdLibraryId",
                newName: "TitleLibraryId",
                table: "Maamarim");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsTitle",
                table: "MaamarLibraryLinks",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.RenameColumn(
                name: "TitleLibraryId",
                newName: "LiberyTitleIdLibraryId",
                table: "Maamarim");
        }
    }
}
