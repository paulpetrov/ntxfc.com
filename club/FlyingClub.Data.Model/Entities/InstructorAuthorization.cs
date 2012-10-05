using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.Entities
{
    public class InstructorAuthorization
    {
        public int Id { get; set; }

        public Aircraft Aircraft { get; set; }
        public int AircraftId { get; set; }

        //public Member Instructor { get; set; }
        public InstructorData Instructor { get; set; }
        public int InstructorId { get; set; }

        public int AuthorizedById { get; set; }
        public DateTime AuthorizedOn { get; set; }
        public string Notes { get; set; }
    }
}
