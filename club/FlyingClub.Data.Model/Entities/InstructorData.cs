using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.Entities
{
    public class InstructorData
    {
        public int Id { get; set; }
        
        public int MemberId { get; set; }
        public string CertificateNumber { get; set; }
        public string Ratings { get; set; }
        public bool InstructOnWeekends { get; set; }
        public bool InstructOnWeekdays { get; set; }
        public bool InstructOnWeekdayNights { get; set; }
        public bool AvailableForCheckoutsAnnuals { get; set; }
        public string About { get; set; }
        public string Comments { get; set; }
        public string Photo { get; set; }
        public bool DesignatedForStageChecks { get; set; }
        public virtual ICollection<InstructorAuthorization> AuthorizedAircraft { get; set; }

        [ForeignKey("MemberId")]
        public virtual Member Member { get; set; }
    }
}
