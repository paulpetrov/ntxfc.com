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
        public string UserName { get; set; }
        public string MemberPIN { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName 
        { 
            get { return string.Format("{0} {1}", FirstName, LastName); } 
        }
        public string PrimaryEmail { get; set; }
        public string SecondaryEmail { get; set; }
        public string Phone { get; set; }
        public string AltPhone { get; set; }
        public string AddressLine_1 { get; set; }
        public string AddressLine_2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        [Display(Name = "Member Since"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? MemberSince { get; set; }
        [Display(Name = "Member Since"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? LastMedical { get; set; }
        public int? TotalHours { get; set; }
        public int? RetractHours { get; set; }
        public string EmergencyName { get; set; }
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