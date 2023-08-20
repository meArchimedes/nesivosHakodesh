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
    public class Source : BaseEntity
    {
        public int SourceID { get; set; }
        public string FirstName { get; set; }
        ////public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string SourceDetails { get; set; }
        public User AssingedUser { get; set; }
        public SourcesStatus Status { get; set; }
        public SourceTypes SourceType { get; set; }
        public string Notes { get; set; }

        public string FullName => $"{FirstName}";
        public string TypeValue => SourceType == SourceTypes.Maamarim ? "מאמרים" : "הדרכות פרטית";
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SourcesStatus
    {
        Active = 0,
        Deleted = 1,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SourceTypes
    {
        Maamarim = 0,
        Personals = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<Source>
    {
        public void Configure(EntityTypeBuilder<Source> builder)
        {
            //builder.ToTable("Sources");

            builder.HasQueryFilter(x => x.Status != SourcesStatus.Deleted);
            builder.HasIndex(x => x.FirstName);
        }
    }
}
