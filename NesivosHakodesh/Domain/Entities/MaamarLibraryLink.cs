using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace NesivosHakodesh.Domain.Entities
{
    public class MaamarLibraryLink : BaseEntity
    {
        public int MaamarLibraryLinkId { get; set; }

        public Library Library { get; set; }

        public Maamar Maamar { get; set; }

        public bool IsDeleted { get; set; }
    }

    public partial class Configuration : IEntityTypeConfiguration<MaamarLibraryLink>
    {
        public void Configure(EntityTypeBuilder<MaamarLibraryLink> builder)
        {
            //builder.ToTable("MaamarLibraryLinks");

            builder.HasQueryFilter(x => x.IsDeleted != true);
        }
    }
}
