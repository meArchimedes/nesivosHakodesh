using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Torahs
{
    public class ParseBook
    {
        public string Name { get; set; }
        public List<string> Parshas { get; set; }

        public List<ParseBook> ParsesObjects => Parshas?.ConvertAll(x => new ParseBook { Name = x});
    }
}
