using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.Entities
{
    public class Login
    {
        public int Id { get;set; }
        public int ForumUserId { get; set; }
        public string Username { get;set; }
        public string Password { get; set; } 
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
        public string Email2 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? LastLogOn { get; set; }

        public string MemberPIN { get; set; }

        public virtual ICollection<Member> ClubMember { get; set; }
    }
}
