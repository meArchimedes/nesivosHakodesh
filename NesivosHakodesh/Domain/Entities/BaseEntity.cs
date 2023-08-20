using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NesivosHakodesh.Domain.Entities
{
    public class BaseEntity
    {
        public DateTime CreatedTime { get; set; }
        public User CreatedUser { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public User UpdatedUser { get; set; }
    }
}
