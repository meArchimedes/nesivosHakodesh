using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Library
{
    public class LibraryObject
    {
        
        public string Category { get; set; }
        public string Type { get; set; }
        public List<List<string>> text { get; set; }

      public string Section { get; set; }
        /*public string Chepter { get; set; }
        public string Verse { get; set; }
        public int Line { get; set; }
        public string Text { get; set; }*/

    }

    public class Chepter
    {
        public List<Verse> Verses { get; set; }
    }

    public class Verse
    {
        public string VerseValue { get; set; }
    }
}
