using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Providers.Solr
{

    public class SolrRecord
    {
        //identifiers
        public string id { get; set; }
        public int internalId { get; set; }
        public MaamarType Type { get; set; }

        //text search
        public string Title { get; set; }
        public string Content { get; set; }

        //filters
        public string Topic { get; set; }
        public string Source { get; set; }
        public string MaamarType { get; set; }
        public string Parsha { get; set; }
        public string Year { get; set; }
        public DateTime CreatedTime { get; set; }

        //Display 
        public string OtherDetails { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public List<string> SubTopics { get; set; }
    }
    public class SolrDelete
    {
        public List<SolrRecord> delete { get; set; }
    }
}
