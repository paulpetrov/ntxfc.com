using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class SquawkComment
    {
        public int Id { get; set; }
        public int PostedByMemberId { get; set; }
        public DateTime PostDate { get; set; }
        public string Text { get; set; }

        public int SquawkId { get; set; }
        public virtual Squawk Squawk { get; set; }
    }
}
