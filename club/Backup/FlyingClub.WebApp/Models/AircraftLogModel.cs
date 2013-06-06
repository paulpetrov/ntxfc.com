using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class AircraftLogModel
    {
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public string LogUrl { get; set; }
        public string ThumbnailUrl { get; set; }
        public Nullable<DateTime> LastUpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public List<LogPageModel> Pages { get; set; }

        public int EditPageNumber { get; set; }
        public bool CanEdit { get; set; }
        
    }

    public class LogPageModel : IComparable<LogPageModel>
    {
        public int PageNumber { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public int CompareTo(LogPageModel other)
        {
            return this.PageNumber.CompareTo(other.PageNumber);
        }

    }
}