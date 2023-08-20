using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NesivosHakodesh.Domain.Entities;

namespace NesivosHakodesh.Comman
{
    public class SearchCriteria
    {
        public int Page { get; set; }
        public int ItemsPerPage { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public SortDirection SortDirection { get; set; }

        public int PageStartIndex
        {
            get
            {
                return (Page - 1) * ItemsPerPage;
            }
        }

        public List<MaamarType> Type { get; set; }
        public List<string> Topic { get; set; }
        public List<MaamarimStatus> Status { get; set; }
        public List<string> Parsha { get; set; }
        public List<string> Years { get; set; }
        public List<string> Sefurim { get; set; }
        public List<int> Source { get; set; }
        public DateRange DateRange { get; set; }

        public List<string> LibraryCategory { get; set; }
        public List<string> LibraryType { get; set; }
        public List<string> LibrarySection { get; set; }
        public List<string> LibraryChepter { get; set; }
        public string LibraryVerse { get; set; }
        public int? CreatedUserId { get; set; }
        public bool IncludePersonalSources { get; set; }
        public int LibraryId { get; set; }
        public bool IsScrolledDown { get; set; }
        public int StartSort { get; set; }
        public int EndSort { get; set; }
        public int CategoryId { get; set; }

        public int ChepterId { get; set; }
    }

    public class DateRange
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool HasValues()
        {
            return StartDate.HasValue && EndDate.HasValue;
        }
    }

    public enum SortDirection
    {
        Ascending,
        Descending
    }
}
