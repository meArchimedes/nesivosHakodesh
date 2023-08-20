using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class TorahSeferLink : BaseEntity
    {
        public int TorahSeferLinkId { get; set; }

        public Torah Torah { get; set; }

        public Sefer Sefer { get; set; }

        public bool IsDeleted { get; set; }
    }

    public partial class Configuration : IEntityTypeConfiguration<TorahSeferLink>
    {
        public void Configure(EntityTypeBuilder<TorahSeferLink> builder)
        {
            builder.ToTable("TorahSeferLinks");

            builder.HasQueryFilter(x => x.IsDeleted != true);
        }
    }
}
