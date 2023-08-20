using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json.Converters;

namespace NesivosHakodesh.Domain.Entities
{
    public class AssignmentResult : BaseEntity
    {
        public int AssignmentResultID { get; set; }
        public ProjectAssignment ProjectAssignment { get; set; }
        public Maamar Maamar { get; set; }
        public string Note { get; set; }
        public AssignmentResultsStatus Status { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum AssignmentResultsStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<AssignmentResult>
    {
        public void Configure(EntityTypeBuilder<AssignmentResult> builder)
        {
            //builder.ToTable("AssignmentResults");

            builder.HasQueryFilter(x => x.Status != AssignmentResultsStatus.Deleted);
        }
    }
}
