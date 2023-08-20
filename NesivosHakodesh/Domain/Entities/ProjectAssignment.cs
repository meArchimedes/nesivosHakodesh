
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class ProjectAssignment : BaseEntity
    {
        public int ProjectAssignmentID { get; set; }
        public ProjectUser ProjectUser { get; set; }
        public ProjectAssignmentsType Type { get; set; }
        public ProjectAssignmentsStatus Status { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<AssignmentResult> AssignmentResults { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectAssignmentsStatus
    {
        Active = 0,
        Deleted = 1,
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectAssignmentsType
    {
        Word_Search = 0,
        Editing = 1,
        Other = 2,
    }

    public partial class Configuration : IEntityTypeConfiguration<ProjectAssignment>
    {
        public void Configure(EntityTypeBuilder<ProjectAssignment> builder)
        {
            //builder.ToTable("ProjectAssignments");

            builder.HasQueryFilter(x => x.Status != ProjectAssignmentsStatus.Deleted);
        }
    }
}
