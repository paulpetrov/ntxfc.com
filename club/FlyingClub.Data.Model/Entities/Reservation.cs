using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.Entities
{
    public class Reservation
    {
        public int Id { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int? InstructorId { get; set; }
        public Member Instructor { get; set; }
        
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Status { get; set; }

        public string Destination { get; set; }
        public string Notes { get; set; }
    }
}
