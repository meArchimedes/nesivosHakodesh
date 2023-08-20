using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Reflection.Emit;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class ProjectUser : BaseEntity
    {
        public int ProjectUserID { get; set; }
        public Project Project { get; set; }
        public User User { get; set; }
        public ProjectUsersStatus Status { get; set; }

        public List<ProjectAssignment> ProjectAssignments { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectUsersStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<ProjectUser>
    {
        public void Configure(EntityTypeBuilder<ProjectUser> builder)
        {
            //builder.ToTable("ProjectUsers");

            builder.HasQueryFilter(x => x.Status != ProjectUsersStatus.Deleted);
        }
    }
}
