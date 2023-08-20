using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class MaamarTopic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Maamarim_MaamarID",
                table: "Topics");

            migrationBuilder.DropIndex(
                name: "IX_Topics_MaamarID",
                table: "Topics");

            migrationBuilder.DropColumn(
                name: "MaamarID",
                table: "Topics");

            migrationBuilder.CreateTable(
                name: "MaamarTopic",
                columns: table => new
                {
                    MaamarTopicID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaamarID = table.Column<int>(nullable: true),
                    TopicID = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaamarTopic", x => x.MaamarTopicID);
                    table.ForeignKey(
                        name: "FK_MaamarTopic_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTopic_Maamarim_MaamarID",
                        column: x => x.MaamarID,
                        principalTable: "Maamarim",
                        principalColumn: "MaamarID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTopic_Topics_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topics",
                        principalColumn: "TopicID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTopic_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTopic_CreatedUserId",
                table: "MaamarTopic",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTopic_MaamarID",
                table: "MaamarTopic",
                column: "MaamarID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTopic_TopicID",
                table: "MaamarTopic",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTopic_UpdatedUserId",
                table: "MaamarTopic",
                column: "UpdatedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaamarTopic");

            migrationBuilder.AddColumn<int>(
                name: "MaamarID",
                table: "Topics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Topics_MaamarID",
                table: "Topics",
                column: "MaamarID");

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Maamarim_MaamarID",
                table: "Topics",
                column: "MaamarID",
                principalTable: "Maamarim",
                principalColumn: "MaamarID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
