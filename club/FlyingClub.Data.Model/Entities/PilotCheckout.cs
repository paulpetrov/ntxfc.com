using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class PilotCheckout
    {
        public int Id { get; set; }
        
        public int AircraftId { get; set; }
        public virtual Aircraft Aircraft { get; set; }

        public int PilotId { get; set; }
        public virtual Member Pilot { get; set; }
        
        public int InstructorId { get; set; }
        public virtual Member Instructor { get; set; }

        public DateTime CheckoutDate { get; set; }
    }
}
