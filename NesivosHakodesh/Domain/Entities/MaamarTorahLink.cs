using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class MaamarTorahLink : BaseEntity
    {
        public int MaamarTorahLinkID { get; set; }
        public Maamar Maamar { get; set; }
        public Torah Torah { get; set; }
        public TorahParagraph TorahParagraph { get; set; }
        public bool IsDeleted { get; set; }
    }

    public partial class Configuration : IEntityTypeConfiguration<MaamarTorahLink>
    {
        public void Configure(EntityTypeBuilder<MaamarTorahLink> builder)
        {
            //builder.ToTable("MaamarTorahLinks");

            builder.HasQueryFilter(x => x.IsDeleted != true);
        }
    }
}
