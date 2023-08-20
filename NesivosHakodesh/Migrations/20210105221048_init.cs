using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NesivosHakodesh.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserName = table.Column<string>(nullable: false),
                    NormalizedUserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: false),
                    NormalizedEmail = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    UserID = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Phone = table.Column<string>(nullable: true),
                    Cell = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    ProjectID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: true),
                    ExpectedDate = table.Column<DateTime>(nullable: true),
                    FinishDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    ProjectManagerId = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.ProjectID);
                    table.ForeignKey(
                        name: "FK_Projects_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_ProjectManagerId",
                        column: x => x.ProjectManagerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sefurim",
                columns: table => new
                {
                    SeferID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sefurim", x => x.SeferID);
                    table.ForeignKey(
                        name: "FK_Sefurim_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sefurim_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    SourceID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: false),
                    SourceDetails = table.Column<string>(nullable: true),
                    AssingedUserId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.SourceID);
                    table.ForeignKey(
                        name: "FK_Sources_Users_AssingedUserId",
                        column: x => x.AssingedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sources_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sources_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectChapters",
                columns: table => new
                {
                    ProjectChapterID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectID = table.Column<int>(nullable: true),
                    ParentChapterProjectChapterID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    SubTitle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectChapters", x => x.ProjectChapterID);
                    table.ForeignKey(
                        name: "FK_ProjectChapters_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectChapters_ProjectChapters_ParentChapterProjectChapterID",
                        column: x => x.ParentChapterProjectChapterID,
                        principalTable: "ProjectChapters",
                        principalColumn: "ProjectChapterID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectChapters_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectChapters_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectUsers",
                columns: table => new
                {
                    ProjectUserID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectID = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectUsers", x => x.ProjectUserID);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Projects_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Projects",
                        principalColumn: "ProjectID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Torahs",
                columns: table => new
                {
                    TorahID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SeferID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    MaarahMakoim = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    OriginalFileName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Torahs", x => x.TorahID);
                    table.ForeignKey(
                        name: "FK_Torahs_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Torahs_Sefurim_SeferID",
                        column: x => x.SeferID,
                        principalTable: "Sefurim",
                        principalColumn: "SeferID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Torahs_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProjectAssignments",
                columns: table => new
                {
                    ProjectAssignmentID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectUserID = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAssignments", x => x.ProjectAssignmentID);
                    table.ForeignKey(
                        name: "FK_ProjectAssignments_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAssignments_ProjectUsers_ProjectUserID",
                        column: x => x.ProjectUserID,
                        principalTable: "ProjectUsers",
                        principalColumn: "ProjectUserID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProjectAssignments_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentResults",
                columns: table => new
                {
                    AssignmentResultID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ProjectAssignmentID = table.Column<int>(nullable: true),
                    MaamarID = table.Column<int>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentResults", x => x.AssignmentResultID);
                    table.ForeignKey(
                        name: "FK_AssignmentResults_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignmentResults_ProjectAssignments_ProjectAssignmentID",
                        column: x => x.ProjectAssignmentID,
                        principalTable: "ProjectAssignments",
                        principalColumn: "ProjectAssignmentID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AssignmentResults_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaamarimParagraphs",
                columns: table => new
                {
                    MaamarParagraphID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Text = table.Column<string>(nullable: true),
                    ParagraphType = table.Column<int>(nullable: false),
                    Sort = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    MaamarID = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaamarimParagraphs", x => x.MaamarParagraphID);
                    table.ForeignKey(
                        name: "FK_MaamarimParagraphs_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaamarimParagraphs_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Topics",
                columns: table => new
                {
                    TopicID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentTopicTopicID = table.Column<int>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    MaamarID = table.Column<int>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.TopicID);
                    table.ForeignKey(
                        name: "FK_Topics_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Topics_ParentTopicTopicID",
                        column: x => x.ParentTopicTopicID,
                        principalTable: "Topics",
                        principalColumn: "TopicID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Topics_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Maamarim",
                columns: table => new
                {
                    MaamarID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TorahID = table.Column<int>(nullable: true),
                    SourceID = table.Column<int>(nullable: true),
                    TopicID = table.Column<int>(nullable: true),
                    Type = table.Column<int>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    OriginalFileName = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Parsha = table.Column<string>(nullable: true),
                    Year = table.Column<string>(nullable: true),
                    WeeklyIndex = table.Column<int>(nullable: false),
                    MaarahMakoim = table.Column<string>(nullable: true),
                    LocationDetails = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    BechatzrPrinted = table.Column<string>(nullable: true),
                    AccuracyRate = table.Column<int>(nullable: false),
                    AccuracyDescriptin = table.Column<string>(nullable: true),
                    CreatedTime = table.Column<DateTime>(nullable: false),
                    CreatedUserId = table.Column<int>(nullable: true),
                    UpdatedTime = table.Column<DateTime>(nullable: true),
                    UpdatedUserId = table.Column<int>(nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maamarim", x => x.MaamarID);
                    table.ForeignKey(
                        name: "FK_Maamarim_Users_CreatedUserId",
                        column: x => x.CreatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maamarim_Sources_SourceID",
                        column: x => x.SourceID,
                        principalTable: "Sources",
                        principalColumn: "SourceID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maamarim_Topics_TopicID",
                        column: x => x.TopicID,
                        principalTable: "Topics",
                        principalColumn: "TopicID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maamarim_Torahs_TorahID",
                        column: x => x.TorahID,
                        principalTable: "Torahs",
                        principalColumn: "TorahID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maamarim_Users_UpdatedUserId",
                        column: x => x.UpdatedUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_CreatedUserId",
                table: "AssignmentResults",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_MaamarID",
                table: "AssignmentResults",
                column: "MaamarID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_ProjectAssignmentID",
                table: "AssignmentResults",
                column: "ProjectAssignmentID");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentResults_UpdatedUserId",
                table: "AssignmentResults",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_CreatedUserId",
                table: "Maamarim",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_SourceID",
                table: "Maamarim",
                column: "SourceID");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_TopicID",
                table: "Maamarim",
                column: "TopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_TorahID",
                table: "Maamarim",
                column: "TorahID");

            migrationBuilder.CreateIndex(
                name: "IX_Maamarim_UpdatedUserId",
                table: "Maamarim",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarimParagraphs_CreatedUserId",
                table: "MaamarimParagraphs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarimParagraphs_MaamarID",
                table: "MaamarimParagraphs",
                column: "MaamarID");

            migrationBuilder.CreateIndex(
                name: "IX_MaamarimParagraphs_UpdatedUserId",
                table: "MaamarimParagraphs",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssignments_CreatedUserId",
                table: "ProjectAssignments",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssignments_ProjectUserID",
                table: "ProjectAssignments",
                column: "ProjectUserID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAssignments_UpdatedUserId",
                table: "ProjectAssignments",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectChapters_CreatedUserId",
                table: "ProjectChapters",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectChapters_ParentChapterProjectChapterID",
                table: "ProjectChapters",
                column: "ParentChapterProjectChapterID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectChapters_ProjectID",
                table: "ProjectChapters",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectChapters_UpdatedUserId",
                table: "ProjectChapters",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_CreatedUserId",
                table: "Projects",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_UpdatedUserId",
                table: "Projects",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_CreatedUserId",
                table: "ProjectUsers",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_ProjectID",
                table: "ProjectUsers",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UpdatedUserId",
                table: "ProjectUsers",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectUsers_UserId",
                table: "ProjectUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sefurim_CreatedUserId",
                table: "Sefurim",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sefurim_UpdatedUserId",
                table: "Sefurim",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_AssingedUserId",
                table: "Sources",
                column: "AssingedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_CreatedUserId",
                table: "Sources",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sources_UpdatedUserId",
                table: "Sources",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_CreatedUserId",
                table: "Topics",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_MaamarID",
                table: "Topics",
                column: "MaamarID");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ParentTopicTopicID",
                table: "Topics",
                column: "ParentTopicTopicID");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_UpdatedUserId",
                table: "Topics",
                column: "UpdatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Torahs_CreatedUserId",
                table: "Torahs",
                column: "CreatedUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Torahs_SeferID",
                table: "Torahs",
                column: "SeferID");

            migrationBuilder.CreateIndex(
                name: "IX_Torahs_UpdatedUserId",
                table: "Torahs",
                column: "UpdatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssignmentResults_Maamarim_MaamarID",
                table: "AssignmentResults",
                column: "MaamarID",
                principalTable: "Maamarim",
                principalColumn: "MaamarID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaamarimParagraphs_Maamarim_MaamarID",
                table: "MaamarimParagraphs",
                column: "MaamarID",
                principalTable: "Maamarim",
                principalColumn: "MaamarID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Topics_Maamarim_MaamarID",
                table: "Topics",
                column: "MaamarID",
                principalTable: "Maamarim",
                principalColumn: "MaamarID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Maamarim_Users_CreatedUserId",
                table: "Maamarim");

            migrationBuilder.DropForeignKey(
                name: "FK_Maamarim_Users_UpdatedUserId",
                table: "Maamarim");

            migrationBuilder.DropForeignKey(
                name: "FK_Sefurim_Users_CreatedUserId",
                table: "Sefurim");

            migrationBuilder.DropForeignKey(
                name: "FK_Sefurim_Users_UpdatedUserId",
                table: "Sefurim");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Users_AssingedUserId",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Users_CreatedUserId",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Sources_Users_UpdatedUserId",
                table: "Sources");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Users_CreatedUserId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Users_UpdatedUserId",
                table: "Topics");

            migrationBuilder.DropForeignKey(
                name: "FK_Torahs_Users_CreatedUserId",
                table: "Torahs");

            migrationBuilder.DropForeignKey(
                name: "FK_Torahs_Users_UpdatedUserId",
                table: "Torahs");

            migrationBuilder.DropForeignKey(
                name: "FK_Topics_Maamarim_MaamarID",
                table: "Topics");

            migrationBuilder.DropTable(
                name: "AssignmentResults");

            migrationBuilder.DropTable(
                name: "MaamarimParagraphs");

            migrationBuilder.DropTable(
                name: "ProjectChapters");

            migrationBuilder.DropTable(
                name: "ProjectAssignments");

            migrationBuilder.DropTable(
                name: "ProjectUsers");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Maamarim");

            migrationBuilder.DropTable(
                name: "Sources");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Torahs");

            migrationBuilder.DropTable(
                name: "Sefurim");
        }
    }
}
