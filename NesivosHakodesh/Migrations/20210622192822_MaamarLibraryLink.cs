using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class MaamarLibraryLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaamarLibraryLinks",
                columns: table => new
                {
                    MaamarLibraryLinkId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LibraryId = table.Column<int>(nullable: true),
                    MaamarID = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaamarLibraryLinks", x => x.MaamarLibraryLinkId);
                    table.ForeignKey(
                        name: "FK_MaamarLibraryLinks_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarLibraryLinks_Library_LibraryId",
                        column: x => x.LibraryId,
                        principalTable: "Library",
                        principalColumn: "LibraryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarLibraryLinks_Maamarim_MaamarID",
                        column: x => x.MaamarID,
                        principalTable: "Maamarim",
                        principalColumn: "MaamarID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarLibraryLinks_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaamarLibraryLinks_CreatedUserId",
                table: "MaamarLibraryLinks",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarLibraryLinks_LibraryId",
                table: "MaamarLibraryLinks",
                column: "LibraryId");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarLibraryLinks_MaamarID",
                table: "MaamarLibraryLinks",
                column: "MaamarID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarLibraryLinks_UpdatedUserId",
                table: "MaamarLibraryLinks",
                column: "UpdatedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaamarLibraryLinks");
        }
    }
}
