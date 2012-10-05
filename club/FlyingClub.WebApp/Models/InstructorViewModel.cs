using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class InstructorViewModel
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string CeritifcateNumber { get; set; }
        public string Ratings { get; set; }
        public bool InstructOnWeekends { get; set; }
        public bool InstructOnWeekdays { get; set; }
        public bool InstructOnWeekdayNights { get; set; }
        public bool AvailableForCheckoutsAnnuals { get; set; }
        public string Comments { get; set; }
        public bool DesignatedForStageChecks { get; set; }
        public List<AircraftListItemViewModel> AuthorizedAircraft { get; set; }
    }
}