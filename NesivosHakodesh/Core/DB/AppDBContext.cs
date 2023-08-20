using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Core.DB
{
    public class AppDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        public DbSet<Sefer> Sefurim { get; set; }
        public DbSet<Torah> Torahs { get; set; }
        public DbSet<TorahParagraph> TorahParagraphs { get; set; }
        public DbSet<MaamarTorahLink> MaamarTorahLinks { get; set; }
        public DbSet<TorahSeferLink> TorahSeferLinks { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Maamar> Maamarim { get; set; }
        public DbSet<MaamarParagraph> MaamarimParagraphs { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectUser> ProjectUsers { get; set; }
        public DbSet<ProjectAssignment> ProjectAssignments { get; set; }
        public DbSet<AssignmentResult> AssignmentResults { get; set; }
        public DbSet<ProjectChapter> ProjectChapters { get; set; }
        public DbSet<MaamarTopic> MaamarTopic { get; set; }
        public DbSet<Library> Library { get; set; }
        public DbSet<LibrarySection> LibrarySections { get; set; }
        public DbSet<MaamarLibraryLink> MaamarLibraryLinks { get; set; }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            //The names should never be changes
           // modelBuilder.Entity<Role>().HasData(PermissionProvider.GetAllRoles());
        }

        public override int SaveChanges()
        {
            var now = DateTime.UtcNow;

            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).CreatedTime = now;
                    ((BaseEntity)entity.Entity).CreatedUser = AppProvider.GetCurrentUserFromDB();
                }
                else
                {
                    ((BaseEntity)entity.Entity).UpdatedTime = now;
                    ((BaseEntity)entity.Entity).UpdatedUser = AppProvider.GetCurrentUserFromDB();
                }
            }

            return base.SaveChanges();
        }
    }
}
