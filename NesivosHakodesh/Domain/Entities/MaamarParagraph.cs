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
    public class MaamarParagraph : BaseEntity
    {
        public int MaamarParagraphID { get; set; }
        public string Text { get; set; }
        public ParagraphType ParagraphType { get; set; }
        public string Sort { get; set; }
        public ParagraphStatus Status { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ParagraphStatus
    {
        Active = 0,
        Deleted = 1,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ParagraphType
    {
        Paragraph = 0,
        Subtitle = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<MaamarParagraph>
    {
        public void Configure(EntityTypeBuilder<MaamarParagraph> builder)
        {
            //builder.ToTable("MaamarimParagraphs");

            builder.HasQueryFilter(x => x.Status != ParagraphStatus.Deleted);
        }
    }
}
