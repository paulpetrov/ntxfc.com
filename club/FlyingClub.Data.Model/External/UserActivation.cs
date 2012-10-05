using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.External
{
    [Table("vb3_useractivation")]
    public class UserActivation
    {
        [Key()]
        public int useractivationid { get; set; }
        public int userid { get; set; }
        public int dateline { get; set; }
        public string activationid { get; set; }
        public int type { get; set; }
        public int usergroupid { get; set; }
        public int emailchange { get; set; }
    }
}
