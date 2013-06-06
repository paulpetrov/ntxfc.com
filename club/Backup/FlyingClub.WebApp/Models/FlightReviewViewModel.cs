using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.WebApp.Models
{
    public class FlightReviewViewModel
    {
        public int PilotId { get; set; }
        public string PilotName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public float TotalTime { get; set; }
        public float RetractTime { get; set; }
        public string ReviewType { get; set; }

        [Display(Name = "Review Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? ReviewDate { get; set; }
        public bool IsOverdue { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }
    }
}