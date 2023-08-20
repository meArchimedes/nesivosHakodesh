using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesivosHakodesh.Comman
{
    public class ListResData<T>
    {
        public List<T> List { get; set; } = new List<T>();
        public int Page = 1;
        public int ItemsPerPage = 20;
        public int TotalCount { get; set; }

        //public ListResData(){}

        public ListResData(SearchCriteria search)
        {
            Page = search.Page;
            ItemsPerPage = search.ItemsPerPage;
        }
    }
}
