using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class PilotReviewItemViewModel
    {
        public int PilotId { get; set; }
        public string PilotName { get; set; }
        public string AircraftCheckouts { get; set; }
        public string LastReviewDate { get; set; }
        public bool IsOverdue { get; set; }
    }
}