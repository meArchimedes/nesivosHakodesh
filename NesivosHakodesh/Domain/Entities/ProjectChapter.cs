using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NesivosHakodesh.Domain.Entities;
using Newtonsoft.Json.Converters;

namespace NesivosHakodesh.Domain.Entities
{
    public class ProjectChapter : BaseEntity
    {
        public int ProjectChapterID { get; set; }
        public Project Project { get; set; }
        public ProjectChapter ParentChapter { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public ProjectChaptersStatus Status { get; set; }
    }
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ProjectChaptersStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<ProjectChapter>
    {
        public void Configure(EntityTypeBuilder<ProjectChapter> builder)
        {
            //builder.ToTable("ProjectChapters");

            builder.HasQueryFilter(x => x.Status != ProjectChaptersStatus.Deleted);
        }
    }
}
