using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class maamarLiberylink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LiberyTitleId",
                table: "Maamarim");

            migrationBuilder.AddColumn<int>(
                name: "LiberyTitleIdLibraryId",
                table: "Maamarim",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_LiberyTitleIdLibraryId",
                table: "Maamarim",
                column: "LiberyTitleIdLibraryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Maamarim_Library_LiberyTitleIdLibraryId",
                table: "Maamarim",
                column: "LiberyTitleIdLibraryId",
                principalTable: "Library",
                principalColumn: "LibraryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maamarim_Library_LiberyTitleIdLibraryId",
                table: "Maamarim");

            migrationBuilder.DropIndex(
                name: "IX_Maamarim_LiberyTitleIdLibraryId",
                table: "Maamarim");

            migrationBuilder.DropColumn(
                name: "LiberyTitleIdLibraryId",
                table: "Maamarim");

            migrationBuilder.AddColumn<string>(
                name: "LiberyTitleId",
                table: "Maamarim",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
