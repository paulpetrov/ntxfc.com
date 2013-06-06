using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.WebApp.Models
{
    public class AddCheckoutViewModel
    {
        public int PilotId { get; set; }
        public string PilotName { get; set; }
        public int AircraftId { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        [Display(Name = "Checkou Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CheckoutDate { get; set; }

        public List<AircraftCheckoutInfoViewModel> AircraftList { get; set; }       
    }

    public class AircraftCheckoutInfoViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string CheckoutRequirements { get; set; }
    }
}