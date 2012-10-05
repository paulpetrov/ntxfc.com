using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class InstructorInfo
    {
        public int InstructorInfoId { get; set; }
        public int MemberId { get; set; }
        public string CertificateNumber { get; set; }
        public string Ratings { get; set; }
    }
}
