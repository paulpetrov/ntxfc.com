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
    public class ReservationViewModel : IValidatableObject
    {
        public ReservationViewModel()
        {
            InstructorId = 0; // to default during post-backs if value is not selected
        }

        public int Id { get; set; }
        
        [Required(ErrorMessage = "Member is required")]
        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int? InstructorId { get; set; }
        public Member Instructor { get; set; }

        [Required(ErrorMessage = "Aircraft is required")]
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
        public string AircraftNumber { get; set; }

        [Required(ErrorMessage = "Start Date is required"), Display(Name = "Start Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage="Start Time is required")]
        public string StartTime { get; set; }

        [Required(ErrorMessage = "End Date is required"),Display(Name = "End Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "End Time is required")]
        public string EndTime { get; set; }

        public List<Aircraft> AircraftList { get; set; }
        public List<Member> InstructorList { get; set; }
        public List<SelectListItem> TimeList { get; set; }
        public Uri UrlReferrer { get; set; }

        public string Destination { get; set; }
        [DataType(DataType.MultilineText)]
        public string Notes { get; set; }

        public bool CanDelete()
        {
            int memberId = Convert.ToInt32(HttpContext.Current.Profile.GetPropertyValue("MemberId"));
            return MemberId == memberId && StartDate > DateTime.Now;
        }
        public bool CanEdit()
        {
            int memberId = Convert.ToInt32(HttpContext.Current.Profile.GetPropertyValue("MemberId"));
            return MemberId == memberId && EndDate > DateTime.Now;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();

            DateTime startDate = StartDate.Date;
            DateTime endDate = EndDate.Date;

            if (!String.IsNullOrEmpty(StartTime))
                startDate = startDate.Add(TimeSpan.Parse(StartTime));

            if (!String.IsNullOrEmpty(EndTime))
                endDate = endDate.Add(TimeSpan.Parse(EndTime));

            if (AircraftId == 0)
                results.Add(new ValidationResult("Aircraft is required."));

            if (Id == 0 && startDate < DateTime.Now)
                results.Add(new ValidationResult("The Start Date must be in the future."));

            if (endDate < DateTime.Now)
                results.Add(new ValidationResult("The End Date must be in the future."));

            if (startDate >= endDate)
                results.Add(new ValidationResult("The End Date must be greater than the Start Date."));
            
            return results;
        }
    }

    public class ReservationIndexViewModel
    {
        public List<ReservationViewModel> ReservationList { get; set; }
        public List<ReservationViewModel> StudentReservationList { get; set; }
    }

    public class ReservationSelectViewModel
    {
        public int? AircraftId { get; set; }
        public List<Aircraft> AircraftList { get; set; }
    }

    public class AircraftByWeekViewModel
    {
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
        public List<DateTime> DateList { get; set; }
        public List<DateTime> TimeList { get; set; }
        public List<ReservationViewModel> ReservationList { get; set; }
        public DateTime StartDate { get { return DateList.Min(); } }
        public DateTime PreviousStartDate { get { return DateList.Min().AddDays(-7); } }
        public DateTime NextStartDate { get { return DateList.Min().AddDays(7); } }

        public string GetReservationFromDateTime(DateTime date, DateTime time)
        {
            DateTime reservationDate = date.Add(time.TimeOfDay);
            ReservationViewModel reservation = ReservationList.Find(x => x.StartDate.CompareTo(reservationDate) <= 0 && x.EndDate.CompareTo(reservationDate) > 0);
            string result = string.Empty;

            if (reservation != null)
            {
                UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                result = String.Format("<a href=\"{0}\">{1} {2}</a>", url.Action("Details", new { id = reservation.Id }), reservation.Member.FirstName, reservation.Member.LastName);
            }

            return result;
        }
    }

    public class AircraftByDayViewModel
    {
        public int AircraftId { get; set; }
        public Aircraft Aircraft { get; set; }
        public DateTime Date { get; set; }
        public DateTime PreviousDate { get { return Date.AddDays(-1); } }
        public DateTime NextDate { get { return Date.AddDays(1); } }
        public List<DateTime> TimeList { get; set; }
        public List<ReservationViewModel> ReservationList { get; set; }

        public string GetReservationFromTime(DateTime time)
        {
            DateTime reservationDate = Date.Add(time.TimeOfDay);
            ReservationViewModel reservation = ReservationList.Find(x => x.StartDate.CompareTo(reservationDate) <= 0 && x.EndDate.CompareTo(reservationDate) > 0);
            string result = string.Empty;

            if (reservation != null)
            {
                UrlHelper url = new UrlHelper(HttpContext.Current.Request.RequestContext);
                result = String.Format("<a href=\"{0}\">{1} {2}</a>", url.Action("Details", new { id = reservation.Id }), reservation.Member.FirstName, reservation.Member.LastName);
            }

            return result;
        }
    }

    public static class ReservationExtensionMethods
    {
        public static List<ReservationViewModel> ConvertToReservationViewModel(this List<Reservation> reservationList)
        {
            return reservationList.ConvertAll(x => new ReservationViewModel()
            {
                Id = x.Id,
                MemberId = x.MemberId,
                Member = x.Member,
                AircraftId = x.AircraftId,
                Aircraft = x.Aircraft,
                StartDate = x.StartDate,
                StartTime = x.StartDate.ToString("HH:mm"),
                EndDate = x.EndDate,
                EndTime = x.EndDate.ToString("HH:mm")
            });
        }

        public static Reservation ConvertToReservation(this ReservationViewModel viewModel)
        {
            return new Reservation()
            {
                Id = viewModel.Id,
                Aircraft = viewModel.Aircraft,
                AircraftId = viewModel.AircraftId,
                EndDate = viewModel.EndDate.Date.Add(TimeSpan.Parse(viewModel.EndTime)),
                //Instructor = viewModel.Instructor,
                InstructorId = viewModel.InstructorId == 0 ? null : viewModel.InstructorId,
                Member = viewModel.Member,
                MemberId = viewModel.MemberId,
                StartDate = viewModel.StartDate.Date.Add(TimeSpan.Parse(viewModel.StartTime)),
                Destination = viewModel.Destination,
                Notes = viewModel.Notes
            };
        }

        public static ReservationViewModel ConvertToReservationViewModel(this Reservation reservation)
        {
            if (reservation == null)
                throw new Exception("Reservation not found");

            return new ReservationViewModel()
            {
                Id = reservation.Id,
                Aircraft = reservation.Aircraft,
                AircraftId = reservation.AircraftId,
                Destination = reservation.Destination,
                EndDate = reservation.EndDate,
                EndTime = reservation.EndDate.ToString("HH:mm"),
                Instructor = reservation.Instructor,
                InstructorId = reservation.InstructorId,
                Member = reservation.Member,
                MemberId = reservation.MemberId,
                Notes = reservation.Notes,
                StartDate = reservation.StartDate,
                StartTime = reservation.StartDate.ToString("HH:mm"),
            };
        }
    }
}