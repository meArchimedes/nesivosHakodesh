using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using System.Reflection.Emit;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class Category: BaseEntity
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public CategoryStatus Status { get; set; }
        public List<Topic> Topics { get; set; }

    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum CategoryStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            //builder.ToTable("Categories");

            builder.HasQueryFilter(x => x.Status != CategoryStatus.Deleted);
        }
    }
}
