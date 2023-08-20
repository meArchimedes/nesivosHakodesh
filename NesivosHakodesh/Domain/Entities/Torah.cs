using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NesivosHakodesh.Providers.Utils;
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
    public class Torah : BaseEntity
    {
        public int TorahID { get; set; }
        public Sefer Sefer { get; set; }
        public string Parsha { get; set; }
        public string MaarahMakoim { get; set; }
        public string Title { get; set; }
        public string Index { get; set; }
        public string OriginalFileName { get; set; }
        public Status Status { get; set; }

        public int? AnnPageNumber { get; set; }
        public double? AnnX { get; set; }
        public double? AnnY { get; set; }
        public double? AnnWidth { get; set; }
        public double? AnnHeight { get; set; }


        public List<TorahParagraph> TorahParagraphs { get; set; }

        public List<MaamarTorahLink> MaamarLinks { get; set; }

        public List<TorahSeferLink> SeferLinks { get; set; }

        [NotMapped]
        public BaseEntity LastUpdatedObject { get; set; }
        public DateTime? LastUpdatedTime => LastUpdatedObject?.UpdatedTime;
        public User LastUpdatedUser => LastUpdatedObject?.UpdatedUser;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Status
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<Torah>
    {
        public void Configure(EntityTypeBuilder<Torah> builder)
        {
            //builder.ToTable("Torahs");

            builder.HasQueryFilter(x => x.Status != Status.Deleted);
            builder.HasIndex(x => x.Parsha);
            builder.HasIndex(x => x.Status);
            builder.HasIndex(x => x.Title);
        }
    }
}
