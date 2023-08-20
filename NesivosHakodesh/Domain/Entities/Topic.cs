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
    public class Topic : BaseEntity
    {
        public int TopicID { get; set; }
        public Category Category { get; set; }
        public Topic ParentTopic { get; set; }
        public string Name { get; set; }
        public TopicsStatus Status { get; set; }
        public List<Topic> SubTopices { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum TopicsStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<Topic>
    {
        public void Configure(EntityTypeBuilder<Topic> builder)
        {
            //builder.ToTable("Topics");

            builder.HasQueryFilter(x => x.Status != TopicsStatus.Deleted);
        }
    }
}
