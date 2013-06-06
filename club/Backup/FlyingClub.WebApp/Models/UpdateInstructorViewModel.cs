using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using FlyingClub.Data.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.WebApp.Models
{
    public class UpdateInstructorViewModel
    {
        public UpdateInstructorViewModel() { }

        public UpdateInstructorViewModel(InstructorData entity)
        {
            Id = entity.Id;
            MemberId = entity.MemberId;
            
            CertificateNumber = entity.CertificateNumber;
            Ratings = entity.Ratings;
            InstructOnWeekends = entity.InstructOnWeekends;
            InstructOnWeekdays = entity.InstructOnWeekdays;
            InstructOnWeekdayNights = entity.InstructOnWeekdayNights;
            AvailableForCheckoutsAnnuals = entity.AvailableForCheckoutsAnnuals;
            DesignatedForStageChecks = entity.DesignatedForStageChecks;
            Comments = entity.Comments;
            AuthorizedAircraft = new List<int>();
        }

        public int Id { get; set; }
        public int MemberId { get; set; }

        [Required()]
        public string CertificateNumber { get; set; }
        public string Ratings { get; set; }

        public bool InstructOnWeekends { get; set; }
        public bool InstructOnWeekdays { get; set; }
        public bool InstructOnWeekdayNights { get; set; }
        public bool AvailableForCheckoutsAnnuals { get; set; }
        public bool DesignatedForStageChecks { get; set; }
        [UIHint("MultilineText")]
        public string Comments { get; set; }

        public List<AircraftListItemViewModel> AircraftList { get; set; }
        public List<int> AuthorizedAircraft { get; set; }
    }
}