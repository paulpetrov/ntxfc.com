using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class MemberViewModel
    {
        public int MemberId { get; set; }
        public string FirstName { get; set; }
        public string LastName {get;set;}
        public string DisplayName { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string PrimaryPhone { get; set; }
        public string SecondaryPhone { get; set; }

    }
}