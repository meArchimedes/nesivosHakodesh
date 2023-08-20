using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Providers.Torahs
{
    public class MaamarFolderView
    {
        public bool IsHome { get; set; }
        public string Text { get; set; }
        public string AllText { get; set; }
        public int TotalCount { get; set; }
        public List<MaamarFolderView> Folders { get; set; }

        public SearchType SearchType { get; set; }
        public string SearchValue { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SearchType
    {
        None,
        Types,
        Parshas,
        ParshaBook,
        Year,

    }
}
