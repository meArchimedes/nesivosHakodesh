using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

using System.Threading.Tasks;
using System.Reflection.Emit;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Domain.Entities
{
    public class MaamarTopic : BaseEntity
    {
        public int MaamarTopicID { get; set; }
        public Maamar Maamar { get; set; }
        public Topic Topic { get; set; }
        public bool MainTopic { get; set; }
        public MaamarTopicStatus Status { get; set; }

        [NotMapped]
        public string CategoryName { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum MaamarTopicStatus
    {
        Active = 0,
        Deleted = 1,
    }

    public partial class Configuration : IEntityTypeConfiguration<MaamarTopic>
    {
        public void Configure(EntityTypeBuilder<MaamarTopic> builder)
        {
            //builder.ToTable("MaamarTopics");

            builder.HasQueryFilter(x => x.Status != MaamarTopicStatus.Deleted);
        }
    }
}
