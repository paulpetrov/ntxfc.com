using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

using FlyingClub.Common;
using FlyingClub.Data.Model.Entities;

namespace FlyingClub.WebApp.Models
{
    public class NewMemberViewModel
    {
        public NewMemberViewModel()
        {
            StatusValues = Enum.GetValues(typeof(MemberStatus)).Cast<string>().ToList();
            MemberRoles = new List<Role>();
        }

        public List<string> StatusValues { get; set; }
        public List<Role> ClubRoles { get; set; }

        public int Id { get; set; }
        [Required()]
        public string LoginName { get; set; }
        public string Status { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        [Required]
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string AddressLine_1 { get; set; }
        public string AddressLine_2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public DateTime? MemberSince { get; set; }
        public DateTime? LastMedical { get; set; }
        public int? TotalHours { get; set; }
        public int? RetractHours { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyPhone { get; set; }
        public List<Role> MemberRoles { get; set; }
    }
}