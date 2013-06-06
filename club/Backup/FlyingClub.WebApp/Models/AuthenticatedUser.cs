using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class AuthenticatedUser
    {
        public int LoginId { get; set; }
        public int MemberId { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
    }
}