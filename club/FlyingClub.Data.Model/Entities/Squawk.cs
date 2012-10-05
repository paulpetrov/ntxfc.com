using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class Squawk
    {
        public int Id { get; set; }
        public DateTime PostedOn { get; set; }
        public int AircraftId { get; set; }
        public virtual Aircraft Aircraft { get; set; }
        public int PostedById { get; set; }
        public virtual Member PostedBy { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public bool GroundAircraft { get; set; }
        public string ResolutionNotes { get; set; }
        public int? ResolvedById { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public string Status { get; set; }

        public virtual ICollection<SquawkComment> Comments { get; set; }
    }
}
