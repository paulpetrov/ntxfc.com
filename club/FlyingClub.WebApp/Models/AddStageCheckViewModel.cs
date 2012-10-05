using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class AddStageCheckViewModel
    {
        public int PilotId { get; set; }
        public string PilotName { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }

        [Display(Name = "Check Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CheckDate { get; set; }
        public string StageName { get; set; }

        public Dictionary<string, string> AvailableStages { get; set; }
    }
}