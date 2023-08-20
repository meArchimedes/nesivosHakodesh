using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NesivosHakodesh.Domain.Entities;
using Newtonsoft.Json.Converters;

namespace NesivosHakodesh.Domain.Entities
{
    public class Project : BaseEntity
       
    {
        public int ProjectID { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpectedDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public ProjectsStatus Status { get; set; }
        public User ProjectManager { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectsStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {
            //builder.ToTable("Projects");

            builder.HasQueryFilter(x => x.Status != ProjectsStatus.Deleted);
        }
    }
}
