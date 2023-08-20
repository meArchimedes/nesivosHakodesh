using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NesivosHakodesh.Domain.Entities
{
    public class Library : BaseEntity
    {
        public int LibraryId { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Section { get; set; }
        public string Chepter { get; set; }
        public string Verse { get; set; }
        public int Line { get; set; }
        public string Text { get; set; }
        public int SortBy { get; set; }

        public string ParsedText => Text.Replace("יהוה", "ה'").Replace("אלהי", "אלקי");
    }

    public partial class Configuration : IEntityTypeConfiguration<Library>
    {
        public void Configure(EntityTypeBuilder<Library> builder)
        {
            //builder.ToTable("Libraries");
        }
    }
}
