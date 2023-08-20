using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class Torahs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Torahs");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Torahs");

            migrationBuilder.AddColumn<string>(
                name: "Index",
                table: "Torahs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Parsha",
                table: "Torahs",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "ProjectAssignments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Maamarim",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    NormalizedName = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TorahParagraphs",
                columns: table => new
                {
                    TorahParagraphID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TorahID = table.Column<int>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    SortIndex = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TorahParagraphs", x => x.TorahParagraphID);
                    table.ForeignKey(
                        name: "FK_TorahParagraphs_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TorahParagraphs_Torahs_TorahID",
                        column: x => x.TorahID,
                        principalTable: "Torahs",
                        principalColumn: "TorahID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TorahParagraphs_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MaamarTorahLinks",
                columns: table => new
                {
                    MaamarTorahLinkID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    MaamarID = table.Column<int>(nullable: true),
                    TorahID = table.Column<int>(nullable: true),
                    TorahParagraphID = table.Column<int>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaamarTorahLinks", x => x.MaamarTorahLinkID);
                    table.ForeignKey(
                        name: "FK_MaamarTorahLinks_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTorahLinks_Maamarim_MaamarID",
                        column: x => x.MaamarID,
                        principalTable: "Maamarim",
                        principalColumn: "MaamarID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTorahLinks_Torahs_TorahID",
                        column: x => x.TorahID,
                        principalTable: "Torahs",
                        principalColumn: "TorahID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTorahLinks_TorahParagraphs_TorahParagraphID",
                        column: x => x.TorahParagraphID,
                        principalTable: "TorahParagraphs",
                        principalColumn: "TorahParagraphID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarTorahLinks_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTorahLinks_CreatedUserId",
                table: "MaamarTorahLinks",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTorahLinks_MaamarID",
                table: "MaamarTorahLinks",
                column: "MaamarID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTorahLinks_TorahID",
                table: "MaamarTorahLinks",
                column: "TorahID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTorahLinks_TorahParagraphID",
                table: "MaamarTorahLinks",
                column: "TorahParagraphID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarTorahLinks_UpdatedUserId",
                table: "MaamarTorahLinks",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TorahParagraphs_CreatedUserId",
                table: "TorahParagraphs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TorahParagraphs_TorahID",
                table: "TorahParagraphs",
                column: "TorahID");

            migrationBuilder.CreateIndex(
                name: "IX_TorahParagraphs_UpdatedUserId",
                table: "TorahParagraphs",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaamarTorahLinks");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "TorahParagraphs");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropColumn(
                name: "Index",
                table: "Torahs");

            migrationBuilder.DropColumn(
                name: "Parsha",
                table: "Torahs");

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Users",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Torahs",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Torahs",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "ProjectAssignments",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Maamarim",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
