using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class _20230810_1617 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TorahSeferLinks",
                columns: table => new
                {
                    TorahSeferLinkId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                    TorahID = table.Column<int>(nullable: false),
                    SeferID = table.Column<int>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TorahSeferLinks", x => x.TorahSeferLinkId);
                    table.ForeignKey(
                        name: "FK_TorahSeferLinks_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TorahSeferLinks_Sefurim_SeferID",
                        column: x => x.SeferID,
                        principalTable: "Sefurim",
                        principalColumn: "SeferID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TorahSeferLinks_Torahs_TorahID",
                        column: x => x.TorahID,
                        principalTable: "Torahs",
                        principalColumn: "TorahID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TorahSeferLinks_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });
       
            migrationBuilder.CreateIndex(
                name: "IX_TorahSeferLinks_SeferID",
                table: "TorahSeferLinks",
                column: "SeferID");

            migrationBuilder.CreateIndex(
                name: "IX_TorahSeferLinks_TorahID",
                table: "TorahSeferLinks",
                column: "TorahID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TorahSeferLinks");
        }
    }
}
