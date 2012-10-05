using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.External
{
    [Table("vb3_userfield")]
    public class UserField
    {
        [Key()]
        public int userid { get; set; }
        public string temp { get; set; }
        public string field1 { get; set; }
        public string field2 { get; set; }
        public string field3 { get; set; }
        public string field4 { get; set; }
    }
}
