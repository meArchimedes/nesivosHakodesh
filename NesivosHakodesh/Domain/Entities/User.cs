using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Providers.Identity;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace NesivosHakodesh.Domain.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cell { get; set; }
        public UserStatus Status { get; set; }

        [NotMapped]
        public string NewPassword { get; set; }

        public List<UserRole> UserRoles { get; set; }

        public bool IsAdmin => PermissionProvider.IsAdmin(this);
        public bool HasMaamarimAccess => PermissionProvider.HasViewAccess(this, Sections.MAAMARIM);
        public bool HasTopicsAccess => PermissionProvider.HasViewAccess(this, Sections.TOPICS);
        public bool HasTorhasAccess => PermissionProvider.HasViewAccess(this, Sections.TORHAS);
        public bool HasSourcesAccess => PermissionProvider.HasViewAccess(this, Sections.SOURCES);
        public bool HasSourcesPrsAccess => PermissionProvider.HasViewAccess(this, Sections.SOURCES_PRS);

        public string FullName => $"{FirstName} {LastName}";
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum UserStatus
    {
        Active = 0,
        Inactive = 2,
        Deleted = 1,
    }

    [Table("Roles")]
    public class Role : IdentityRole<int>
    {

    }

    public class UserRole
    {
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }

    public partial class Configuration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.ToTable("Users");

            builder.HasQueryFilter(x => x.Status != UserStatus.Deleted);           
        }
    }

    public partial class Configuration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles").HasKey(u => new { u.UserId, u.RoleId });
        }
    }
}
