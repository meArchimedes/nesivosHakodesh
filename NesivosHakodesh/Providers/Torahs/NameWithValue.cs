using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Torahs
{
    public class NameWithValue
    {
        public string Name { get; set; }
        public string TypeValue { get; set; }
    }

    public class OutlineItem
    {
        public string Title { get; set; }
        public int PageNumber { get; set; }
    }
}
