using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.External
{
    [Table("vb3_session")]
    public class Session
    {
        [Key()]
        public string sessionhash { get; set; }
        public int userid { get; set; }
        public string host { get; set; }
        public string idhash { get; set; }
        public int lastactivity { get; set; }
        public string location { get; set; }
        public string useragent { get; set; }
        public int styleid { get; set; }
        public int languageid { get; set; }
        public int loggedin { get; set; }
        public int inforum { get; set; }
        public int inthread { get; set; }
        public int incalendar { get; set; }
        public int badlocation { get; set; }
        public int bypass { get; set; }
        public int profileupdate { get; set; }
        public int apiclientid { get; set; }
        public string apiaccesstoken { get; set; }
        public int isbot { get; set; }
    }
}
