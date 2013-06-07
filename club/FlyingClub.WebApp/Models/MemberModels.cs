using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using FlyingClub.Data.Model.Entities;

namespace FlyingClub.WebApp.Models
{
    public class MemberViewModel
    {
        public int Id { get; set; }
        public int LoginId { get; set; }

        [Display(Name = "Username"), DisplayFormat(NullDisplayText = "-")]
        public string UserName { get; set; }

        [Display(Name = "PIN"), DisplayFormat(NullDisplayText = "-")]
        public string MemberPIN { get; set; }

        [Display(Name = "Status"), DisplayFormat(NullDisplayText = "-")]
        public string Status { get; set; }

        [Display(Name = "First Name"), DisplayFormat(NullDisplayText = "-")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name"), DisplayFormat(NullDisplayText = "-")]
        public string LastName { get; set; }

        public string DisplayName 
        { 
            get { return string.Format("{0} {1}", FirstName, LastName); } 
        }

        [Display(Name = "Email"), DisplayFormat(NullDisplayText = "-")]
        public string PrimaryEmail { get; set; }

        [Display(Name = "Alt. email"), DisplayFormat(NullDisplayText = "-")]
        public string SecondaryEmail { get; set; }

        [Display(Name = "Phone"), DisplayFormat(NullDisplayText="-")]
        public string Phone { get; set; }

        [Display(Name = "Alt. Phone"), DisplayFormat(NullDisplayText = "-")]
        public string AltPhone { get; set; }

        [Display(Name = "Address 1"), DisplayFormat(NullDisplayText="-")]
        public string AddressLine_1 { get; set; }

        [Display(Name = "Address 2"), DisplayFormat(NullDisplayText = "-")]
        public string AddressLine_2 { get; set; }

        [Display(Name = "City"), DisplayFormat(NullDisplayText = "-")]
        public string City { get; set; }

        [Display(Name = "Zip"), DisplayFormat(NullDisplayText = "-")]
        public string Zip { get; set; }

        [Display(Name = "Member Since"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "-")]
        public DateTime? MemberSince { get; set; }

        [Display(Name = "Last Medical"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}", NullDisplayText = "-")]
        public DateTime? LastMedical { get; set; }

        [Display(Name = "Total Hours"), DisplayFormat(NullDisplayText = "-")]
        public int? TotalHours { get; set; }

        [Display(Name = "Retract Hours"), DisplayFormat(NullDisplayText = "-")]
        public int? RetractHours { get; set; }

        [Display(Name = "Emergency Name"), DisplayFormat(NullDisplayText = "-")]
        public string EmergencyName { get; set; }

        [Display(Name = "Emergency Phone"), DisplayFormat(NullDisplayText = "-")]
        public string EmergencyPhone { get; set; }

        public string PilotStatus { get; set; }
        public DateTime? FlightReviewLastDate { get; set; }
        public int FlightReviewInstructorId { get; set; }
        public string FlightReviewNotes { get; set; }
        public string FlightReviewInstructrorName { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    public static class MemberExtensionMethods
    {
        public static MemberViewModel ConvertToViewModel(this Member entity)
        {
            if (entity == null)
                throw new Exception("Member not found");

            return new MemberViewModel()
            {
                AddressLine_1 = entity.AddressLine_1,
                AddressLine_2 = entity.AddressLine_2,
                AltPhone = entity.AltPhone,
                BirthDate = entity.BirthDate,
                City = entity.City,
                EmergencyName = entity.EmergencyName,
                EmergencyPhone = entity.EmergencyPhone,
                FirstName = entity.FirstName,
                FlightReviewInstructorId = entity.FlightReviewInstructorId,
                FlightReviewInstructrorName = entity.FlightReviewInstructrorName,
                FlightReviewLastDate = entity.FlightReviewLastDate,
                FlightReviewNotes = entity.FlightReviewNotes,
                Id = entity.Id,
                LastMedical = entity.LastMedical,
                LastName = entity.LastName,
                LoginId = entity.LoginId,
                MemberSince = entity.MemberSince,
                Phone = entity.Phone,
                PilotStatus = entity.PilotStatus,
                PrimaryEmail = entity.Login.Email,
                RetractHours = entity.RetractHours,
                Status = entity.Status,
                TotalHours = entity.TotalHours,
                Zip = entity.Zip,
                MemberPIN = entity.Login.MemberPIN
            };
        }

        public static Member ConvertToEntity(this EditMemberViewModel model)
        {
            Member member = new Member();
            return model.CopyToEntity(member);
        }

        public static Member CopyToEntity(this EditMemberViewModel model, Member member)
        {
            member.FirstName = model.FirstName;
            member.LastName = model.LastName;
            member.AddressLine_1 = model.AddressLine_1;
            member.AddressLine_2 = model.AddressLine_2;
            member.AltPhone = model.AltPhone;
            member.City = model.City;
            member.EmergencyName = model.EmergencyName;
            member.EmergencyPhone = model.EmergencyPhone;
            member.LastMedical = model.LastMedical;
            member.Phone = model.Phone;
            //member.PrimaryEmail = model.PrimaryEmail;
            member.RetractHours = model.RetractHours;
            //member.SecondaryEmail = model.SecondaryEmail;
            member.MemberSince = model.MemberSince;
            member.TotalHours = model.TotalHours;
            member.Zip = model.Zip;

            return member;
        }
    }

    public class MemberListItemViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PIN { get; set; }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string City { get; set; }
        public string Status { get; set; }
    }

    public class MemberListViewModel
    {
        public bool ShowInactive { get; set; }
        public List<MemberListItemViewModel> Members { get; set; }
    }
}