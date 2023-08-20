using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Providers.Torahs;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection.Emit;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class Sefer : BaseEntity
    {
        public int SeferID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AuthorSefer { get; set; }
        public string SeferDetails { get; set; }
        public string PrintYear { get; set; }
        public SefurimStatus Status { get; set; }
        public string FileUrl { get; set; }
        public string OutlineJson { get; set; }

        public List<Torah> Torahs { get; set; }

        [NotMapped]
        public List<OutlineItem> Outline
        {
            get
            {
                try
                {
                    if(!string.IsNullOrEmpty(OutlineJson))
                    {
                        return JsonConvert.DeserializeObject<List<OutlineItem>>(OutlineJson);
                    }

                    return new List<OutlineItem>();
                }
                catch (Exception e)
                {
                    return new List<OutlineItem>();
                }
            }
        }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SefurimStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<Sefer>
    {
        public void Configure(EntityTypeBuilder<Sefer> builder)
        {
            //builder.ToTable("Sefurim");

            builder.HasQueryFilter(x => x.Status != SefurimStatus.Deleted);

            builder.HasIndex(x => x.Name);
        }
    }
}
