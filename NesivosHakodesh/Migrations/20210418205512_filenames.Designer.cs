﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NesivosHakodesh.Core.DB;

namespace NesivosHakodesh.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20210418205512_filenames")]
    partial class filenames
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("NesivosHakodesh.Models.AssignmentResult", b =>
                {
                    b.Property<int>("AssignmentResultID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<int?>("MaamarID")
                        .HasColumnType("int");

                    b.Property<string>("Note")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ProjectAssignmentID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("AssignmentResultID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("MaamarID");

                    b.HasIndex("ProjectAssignmentID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("AssignmentResults");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Maamar", b =>
                {
                    b.Property<int>("MaamarID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AccuracyDescriptin")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("AccuracyRate")
                        .HasColumnType("int");

                    b.Property<string>("AudioFileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("BechatzrPrinted")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Content")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("LocationDetails")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OriginalFileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Parsha")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("PdfFileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("SourceID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int?>("TopicID")
                        .HasColumnType("int");

                    b.Property<int?>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("WeeklyIndex")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Year")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.HasKey("MaamarID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Date");

                    b.HasIndex("Parsha");

                    b.HasIndex("SourceID");

                    b.HasIndex("Status");

                    b.HasIndex("Title");

                    b.HasIndex("TopicID");

                    b.HasIndex("Type");

                    b.HasIndex("UpdatedUserId");

                    b.HasIndex("Year");

                    b.ToTable("Maamarim");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.MaamarParagraph", b =>
                {
                    b.Property<int>("MaamarParagraphID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<int?>("MaamarID")
                        .HasColumnType("int");

                    b.Property<int>("ParagraphType")
                        .HasColumnType("int");

                    b.Property<string>("Sort")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("MaamarParagraphID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("MaamarID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("MaamarimParagraphs");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.MaamarTopic", b =>
                {
                    b.Property<int>("MaamarTopicID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<int?>("MaamarID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int?>("TopicID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("MaamarTopicID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("MaamarID");

                    b.HasIndex("TopicID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("MaamarTopic");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.MaamarTorahLink", b =>
                {
                    b.Property<int>("MaamarTorahLinkID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<int?>("MaamarID")
                        .HasColumnType("int");

                    b.Property<int?>("TorahID")
                        .HasColumnType("int");

                    b.Property<int?>("TorahParagraphID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("MaamarTorahLinkID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("MaamarID");

                    b.HasIndex("TorahID");

                    b.HasIndex("TorahParagraphID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("MaamarTorahLinks");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Project", b =>
                {
                    b.Property<int>("ProjectID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ExpectedDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("FinishDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ProjectManagerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ProjectManagerId");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.ProjectAssignment", b =>
                {
                    b.Property<int>("ProjectAssignmentID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ProjectUserID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectAssignmentID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ProjectUserID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("ProjectAssignments");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.ProjectChapter", b =>
                {
                    b.Property<int>("ProjectChapterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ParentChapterProjectChapterID")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("SubTitle")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Title")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectChapterID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ParentChapterProjectChapterID");

                    b.HasIndex("ProjectID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("ProjectChapters");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.ProjectUser", b =>
                {
                    b.Property<int>("ProjectUserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<int?>("ProjectID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("ProjectUserID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ProjectID");

                    b.HasIndex("UpdatedUserId");

                    b.HasIndex("UserId");

                    b.ToTable("ProjectUsers");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Role");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Sefer", b =>
                {
                    b.Property<int>("SeferID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FileUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("OutlineJson")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("SeferID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Name");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Sefurim");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Source", b =>
                {
                    b.Property<int>("SourceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("AssingedUserId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<string>("Notes")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("SourceDetails")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("SourceID");

                    b.HasIndex("AssingedUserId");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("FirstName");

                    b.HasIndex("LastName");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Sources");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Topic", b =>
                {
                    b.Property<int>("TopicID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("ParentTopicTopicID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("TopicID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("ParentTopicTopicID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Torah", b =>
                {
                    b.Property<int>("TorahID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<double?>("AnnHeight")
                        .HasColumnType("double");

                    b.Property<int?>("AnnPageNumber")
                        .HasColumnType("int");

                    b.Property<double?>("AnnWidth")
                        .HasColumnType("double");

                    b.Property<double?>("AnnX")
                        .HasColumnType("double");

                    b.Property<double?>("AnnY")
                        .HasColumnType("double");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<string>("Index")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MaarahMakoim")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("OriginalFileName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Parsha")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<int?>("SeferID")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .HasColumnType("varchar(255) CHARACTER SET utf8mb4");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("TorahID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("Parsha");

                    b.HasIndex("SeferID");

                    b.HasIndex("Status");

                    b.HasIndex("Title");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("Torahs");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.TorahParagraph", b =>
                {
                    b.Property<int>("TorahParagraphID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SortIndex")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Text")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int?>("TorahID")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("UpdatedUserId")
                        .HasColumnType("int");

                    b.HasKey("TorahParagraphID");

                    b.HasIndex("CreatedUserId");

                    b.HasIndex("TorahID");

                    b.HasIndex("UpdatedUserId");

                    b.ToTable("TorahParagraphs");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Cell")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ConcurrencyStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.UserRole", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("UserRoles");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.AssignmentResult", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Maamar", "Maamar")
                        .WithMany()
                        .HasForeignKey("MaamarID");

                    b.HasOne("NesivosHakodesh.Models.ProjectAssignment", "ProjectAssignment")
                        .WithMany("AssignmentResults")
                        .HasForeignKey("ProjectAssignmentID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Maamar", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Source", "Source")
                        .WithMany()
                        .HasForeignKey("SourceID");

                    b.HasOne("NesivosHakodesh.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.MaamarParagraph", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Maamar", null)
                        .WithMany("MaamarParagraphs")
                        .HasForeignKey("MaamarID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.MaamarTopic", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Maamar", "Maamar")
                        .WithMany("SubTopics")
                        .HasForeignKey("MaamarID");

                    b.HasOne("NesivosHakodesh.Models.Topic", "Topic")
                        .WithMany()
                        .HasForeignKey("TopicID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.MaamarTorahLink", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Maamar", "Maamar")
                        .WithMany("TorahLinks")
                        .HasForeignKey("MaamarID");

                    b.HasOne("NesivosHakodesh.Models.Torah", "Torah")
                        .WithMany("MaamarLinks")
                        .HasForeignKey("TorahID");

                    b.HasOne("NesivosHakodesh.Models.TorahParagraph", "TorahParagraph")
                        .WithMany()
                        .HasForeignKey("TorahParagraphID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Project", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.User", "ProjectManager")
                        .WithMany()
                        .HasForeignKey("ProjectManagerId");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.ProjectAssignment", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.ProjectUser", "ProjectUser")
                        .WithMany("ProjectAssignments")
                        .HasForeignKey("ProjectUserID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.ProjectChapter", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.ProjectChapter", "ParentChapter")
                        .WithMany()
                        .HasForeignKey("ParentChapterProjectChapterID");

                    b.HasOne("NesivosHakodesh.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.ProjectUser", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");

                    b.HasOne("NesivosHakodesh.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Sefer", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Source", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "AssingedUser")
                        .WithMany()
                        .HasForeignKey("AssingedUserId");

                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Topic", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Topic", "ParentTopic")
                        .WithMany()
                        .HasForeignKey("ParentTopicTopicID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.Torah", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Sefer", "Sefer")
                        .WithMany("Torahs")
                        .HasForeignKey("SeferID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.TorahParagraph", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedUserId");

                    b.HasOne("NesivosHakodesh.Models.Torah", null)
                        .WithMany("TorahParagraphs")
                        .HasForeignKey("TorahID");

                    b.HasOne("NesivosHakodesh.Models.User", "UpdatedUser")
                        .WithMany()
                        .HasForeignKey("UpdatedUserId");
                });

            modelBuilder.Entity("NesivosHakodesh.Models.UserRole", b =>
                {
                    b.HasOne("NesivosHakodesh.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NesivosHakodesh.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
