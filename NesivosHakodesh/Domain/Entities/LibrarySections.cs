using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace NesivosHakodesh.Domain.Entities
{
    public class LibrarySection
    {
        public int id { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Section { get; set; }
        public int Sort { get; set; }
    }

    public partial class Configuration : IEntityTypeConfiguration<LibrarySection>
    {
        public void Configure(EntityTypeBuilder<LibrarySection> builder)
        {
            //builder.ToTable("LibrarySections");
        }
    }
}
