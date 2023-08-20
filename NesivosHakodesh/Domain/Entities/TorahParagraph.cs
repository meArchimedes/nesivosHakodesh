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
    public class TorahParagraph : BaseEntity
    {
        public int TorahParagraphID { get; set; }
        public string Text { get; set; }
        public string SortIndex { get; set; }
        public bool IsDeleted { get; set; }
    }

    public partial class Configuration : IEntityTypeConfiguration<TorahParagraph>
    {
        public void Configure(EntityTypeBuilder<TorahParagraph> builder)
        {
            //builder.ToTable("TorahParagraphs");

            builder.HasQueryFilter(x => x.IsDeleted != true);
        }
    }
}
