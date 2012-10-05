using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class FlightReview
    {
        public int Id { get; set; }
        public int PilotId { get; set; }
        public DateTime Date { get; set; }
        public string ReviewType { get; set; }
        public float TotalTime { get; set; }
        public float RetractTime { get; set; }
        public string InstructorName { get; set; }
        public string InstructorNotes { get; set; }

        public virtual Member Pilot { get; set; }
    }
}
