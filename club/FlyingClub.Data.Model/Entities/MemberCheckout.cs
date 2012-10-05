using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class MemberCheckout
    {
        public int Id { get; set; }
        
        public int AircraftId { get; set; }
        public virtual Aircraft Aircraft { get; set; }

        public int MemberId { get; set; }
        public virtual Member Member { get; set; }
        
        public int InstructorId { get; set; }
        public virtual Member Instructor { get; set; }

        public DateTime CheckoutDate { get; set; }
    }
}
