using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.WebApp.Models
{
    public class PilotReviewViewModel
    {
        public PilotReviewViewModel() { }

        //public PilotCheckoutsViewModel(Member member)
        //{
        //    MemberId = member.Id;
        //    DisplayName = member.FullName;
        //    AircraftCheckouts = new List<AircraftCheckoutViewModel>();

        //    foreach(var checkout in member.Che)
        //}

        public int MemberId { get; set; }
        public string PilotName { get; set; }

        public FlightReviewViewModel FlightReview { get; set; }

        public List<StageCheck> StageChecks { get; set; }
        public bool CanEditStageChecks { get; set; }

        public List<AircraftCheckoutViewModel> AircraftCheckouts { get; set; }

        public string AircraftCheckoutsString
        {
            get
            {
                string result = String.Empty;
                AircraftCheckouts.ForEach(item => result += item.RegistrationNumber + ",");
                if (result != String.Empty)
                    result = result.Remove(result.Length - 1, 1);
                return result;
            }
        }
    }
}