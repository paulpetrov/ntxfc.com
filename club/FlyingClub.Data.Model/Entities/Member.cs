using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.Entities
{
    
    public class Member
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public string Status { get; set; }

        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", FirstName, LastName);
            }
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string AddressLine_1 { get; set; }
        public string AddressLine_2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public DateTime? MemberSince { get; set; }
        public DateTime? LastMedical { get; set; }
        public int? TotalHours { get; set; }
        public int? RetractHours { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyPhone { get; set; }
        public string PilotStatus { get; set; }
        public DateTime? FlightReviewLastDate { get; set; }
        public int FlightReviewInstructorId { get; set; }
        public string FlightReviewNotes { get; set; }
        public string FlightReviewInstructrorName { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? LastUpdated { get; set; }

        public virtual Login Login { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<PilotCheckout> Checkouts { get; set; }
        public virtual ICollection<StageCheck> StageChecks { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Squawk> SquawksPosted { get; set; }
        public virtual ICollection<FlightReview> FlightReviews { get; set; }
    }
}
