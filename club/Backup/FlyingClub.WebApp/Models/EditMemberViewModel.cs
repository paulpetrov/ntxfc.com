using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlyingClub.Data.Model.Entities;
using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class EditMemberViewModel //: IValidatableObject
    {
        public EditMemberViewModel() 
        {
            Init();
        }

        public EditMemberViewModel(Member member)
        {
            Init();

            Id = member.Id;
            AddressLine_1 = member.AddressLine_1;
            AddressLine_2 = member.AddressLine_2;
            AltPhone = member.AltPhone;
            City = member.City;
            EmergencyName = member.EmergencyName;
            EmergencyPhone = member.EmergencyPhone;
            LastMedical = member.LastMedical;
            LastName = member.LastName;
            FirstName = member.FirstName;
            MemberSince = member.MemberSince;
            Phone = member.Phone;
            //PrimaryEmail = member.PrimaryEmail;
            RetractHours = member.RetractHours;
            //SecondaryEmail = member.SecondaryEmail;
            Status = member.Status;
            State = member.State;
            TotalHours = member.TotalHours;
            Zip = member.Zip;
        }

        private void Init()
        {
            StatusValues = Enum.GetValues(typeof(MemberStatus)).Cast<MemberStatus>().Select(i => i.ToString()).ToList();
            
            MemberRoles = new List<int>();
        }

        public int LoginId { get; set; }

        public string Username { get; set; }

        public List<string> StatusValues { get; set; }
        public List<Role> ClubRoles { get; set; }

        public int Id { get; set; }
        public string Status { get; set; }

        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        //[Required]
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }

        [Required]
        public string Phone { get; set; }
        public string AltPhone { get; set; }

        public string AddressLine_1 { get; set; }
        public string AddressLine_2 { get; set; }

        [Required]
        public string City { get; set; }
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }

        [Display(Name = "Member Since"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? MemberSince { get; set; }

        [Display(Name = "Last Medical"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastMedical { get; set; }

        public int? TotalHours { get; set; }
        public int? RetractHours { get; set; }
        public string EmergencyName { get; set; }
        public string EmergencyPhone { get; set; }
        public List<int> MemberRoles { get; set; }
    }
}