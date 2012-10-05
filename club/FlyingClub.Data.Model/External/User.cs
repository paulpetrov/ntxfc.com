using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.External
{
    [Table("vb3_user")]
    public class User
    {
        public int userid { get; set; }
        public int usergroupid { get; set; }
        public string membergroupids { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public DateTime passworddate { get; set; }
        public string email { get; set; }
        public int joindate { get; set; }
        public int showvbcode { get; set; }
        public int showbirthday { get; set; }
        public string usertitle { get; set; }
        public string salt { get; set; }
        public int timezoneoffset { get; set; }
        public int options { get; set; }
    }
}
