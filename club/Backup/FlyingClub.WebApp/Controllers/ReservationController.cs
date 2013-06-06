using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlyingClub.BusinessLogic;
using FlyingClub.Common;
using FlyingClub.Data.Model.Entities;
using FlyingClub.WebApp.Models;
using System.Net.Mail;

namespace FlyingClub.WebApp.Controllers
{
    public class ReservationController : BaseController
    {
        private IClubDataService _dataService;

        public ReservationController()
        {
            _dataService = new ClubDataService();
        }

        public ActionResult Index()
        {
            int memberId = Convert.ToInt32(HttpContext.Profile.GetPropertyValue("MemberId"));

            var memberReservations = _dataService.GetReservationListByMember(memberId);
            var studentReservations = new List<Reservation>();

            if (User.IsInRole("Instructor"))
            {
                studentReservations = _dataService.GetStudentReservationListByInstructor(memberId);
            }

            var model = new ReservationIndexViewModel()
            {
                ReservationList = memberReservations.ConvertToReservationViewModel(),
                StudentReservationList = studentReservations.ConvertToReservationViewModel()
            };

            return View(model);
        }

        public ActionResult Details(int id)
        {
            var reservation = _dataService.GetReservation(id);
            var model = reservation.ConvertToReservationViewModel();
            return View(model);
        }

        public ActionResult Create(int? aircraftId, DateTime? startDate)
        {
            string startTime = "06:00";
            string endTime = "07:00";
            DateTime endDate = DateTime.Now;
            if (startDate == null || startDate.Value < DateTime.Now)
            {
                startDate = DateTime.Now.Date;
                startTime = DateTime.Now.AddHours(1).ToString("HH:00");
                endDate = DateTime.Now.AddHours(2);
                endTime = endDate.ToString("HH:00");
            }                
            
            var model = new ReservationViewModel()
            {
                MemberId = Convert.ToInt32(HttpContext.Profile.GetPropertyValue("MemberId")),//TODO: replace with common call to profile service
                AircraftId = aircraftId != null ? aircraftId.Value : 0,
                InstructorId = 0,
                StartDate = startDate.Value,
                StartTime = startTime,
                EndDate = endDate,
                EndTime = endTime,
                AircraftList = _dataService.GetAllAirplanes(),
                InstructorList = _dataService.GetMembersByRoleAndStatus(UserRoles.Instructor, MemberStatus.Active),
                TimeList = new List<DateTime>().GetListFromRange(DateTime.MinValue.AddHours(6), DateTime.MinValue.AddHours(22), new TimeSpan(0, 30, 0)).ConvertAll(x => new SelectListItem() { Value = x.ToString("HH:mm"), Text = x.ToString("HH:mm") }).ToList(),
                Destination = "TKI"
            };

            // for default selection
            model.InstructorList.Add(new Member() { Id = -1, FirstName = "" });

            if (HttpContext.Request.UrlReferrer != null)
                model.UrlReferrer = HttpContext.Request.UrlReferrer;
            else
                model.UrlReferrer = new Uri(Url.Action("Index", null, null, Request.Url.Scheme));

            model.AircraftList.Insert(0, new Aircraft());
            model.InstructorList.Insert(0, new Member());
            
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                ValidateReservation(model);
            }

            if(ModelState.IsValid)
            {
                _dataService.SaveReservation(model.ConvertToReservation());

                if (model.InstructorId != 0)
                {
                    try
                    {
                        Member pilot = _dataService.GetMember(model.MemberId);
                        Member instructor = _dataService.GetMember((int)model.InstructorId);
                        Aircraft aircraft = _dataService.GetAircraftById(model.AircraftId);

                        MailMessage message = new MailMessage();
                        message.From = new MailAddress("admin@ntxfc.com");
                        message.To.Add(new MailAddress(instructor.Login.Email));

                        message.Subject = "Flight lesson notification";
                        message.Body = "NTXFC club member " + pilot.FullName
                            + " has scheduled flight lesson in " + aircraft.RegistrationNumber
                            + " starting at " + model.StartTime + " on " + model.StartDate.ToString("yyyy-MM-dd") + "\t"
                            + "If you are not able please contact pilot at " + pilot.Login.Email + " or " + pilot.Phone + "\t";

                        SendEmail(message);
                    }
                    catch (Exception ex)
                    {
                        string msg = "Error while trying to send Reservation notification:\t" + ex.ToString();
                        LogError(msg);
                    }

                }

               return RedirectToAction("AircraftByWeek", new { aircraftId = model.AircraftId, startDate = model.StartDate }); 
            }

            model.AircraftList = _dataService.GetAllAirplanes();
            model.InstructorList = _dataService.GetMembersByRoleAndStatus(UserRoles.Instructor, MemberStatus.Active);
            model.TimeList = new List<DateTime>().GetListFromRange(DateTime.MinValue.AddHours(6), DateTime.MinValue.AddHours(22), 
                new TimeSpan(0, 30, 0)).ConvertAll(x => new SelectListItem() { Value = x.ToString("HH:mm"), Text = x.ToString("HH:mm") }).ToList();

            model.AircraftList.Insert(0, new Aircraft());
            model.InstructorList.Insert(0, new Member());
            
            return View(model);
        }

        private bool ValidateReservation(ReservationViewModel model)
        {
            if (!_dataService.IsValidReservationDateRange(model.Id, model.AircraftId,
                    model.StartDate.Date.Add(TimeSpan.Parse(model.StartTime)),
                    model.EndDate.Date.Add(TimeSpan.Parse(model.EndTime))))
            {
                ModelState.AddModelError(string.Empty, "A reservation already exists for this time period.");
            }

            // require provide instructor for non-checked out aircraft
            if ((model.InstructorId == null || model.InstructorId == 0))
            {
                // exclude instructors and this aircraft owners
                if (!(User.IsInRole(UserRoles.Instructor.ToString()) | _dataService.IsAircraftOwner(model.MemberId, model.AircraftId)))
                {
                    Member member = _dataService.GetMemberWithPilotData(model.MemberId);
                    if (member.Checkouts.FirstOrDefault(c => c.AircraftId == model.AircraftId) == null)
                    {
                        ModelState.AddModelError(String.Empty, "Records indicate you are not checked out in this airplane.\nPlease add club instructor to your reservation or use different airplane.");
                    }
                }
            }

            List<Reservation> existingReservations = _dataService.GetReservationListByMember(model.MemberId);
            DateTime today = DateTime.Now;
            // do not allow more than one reservation more than 10 days ahead
            if (model.StartDate >= today.AddDays(10))
            {
                // find all other with start date more than 10 days
                Reservation advanceReservation = existingReservations.FirstOrDefault(r => (r.StartDate >= today.AddDays(10)) && r.Id != model.Id);
                if (advanceReservation != null)
                {
                    ModelState.AddModelError(string.Empty,
                        String.Format("One reservation already exists for time period 10 or more days ahead (start date {0}).\nYou are only allowed to have one reservation more than 10 days in advance",
                        advanceReservation.StartDate.ToString("yyyy-MM-dd hh:mm")));
                }
            }

            // check if there are any other aircraft reservations for the same period (excluding current one if updating)
            DateTime newReservationStart = model.StartDate.Add(TimeSpan.Parse(model.StartTime));
            DateTime newReservationEnd = model.EndDate.Add(TimeSpan.Parse(model.EndTime));
            Reservation overlapReservation = existingReservations.FirstOrDefault(r => r.Id != model.Id &&
                (
                (r.StartDate <= newReservationStart && r.EndDate > newReservationStart) ||
                (r.StartDate < newReservationEnd && r.EndDate >= newReservationEnd) ||
                (r.StartDate >= newReservationStart && r.EndDate <= newReservationEnd)
                ));

            if (overlapReservation != null)
            {
                Aircraft ac = _dataService.GetAircraftById(overlapReservation.AircraftId);
                ModelState.AddModelError(String.Empty,
                    String.Format("Reservation conflicts with another reservation for {0} which starts at {1} and ends at {2}.\nPlease adjust your reservation times so there's no overlap.",
                    ac.RegistrationNumber,
                    overlapReservation.StartDate.ToString("yyyy-MM-dd hh:mm"),
                    overlapReservation.EndDate.ToString("yyyy-MM-dd hh:mm")));
            }

            return ModelState.IsValid;
        }

        public ActionResult Edit(int id)
        {
            var reservation = _dataService.GetReservation(id);
            var model = reservation.ConvertToReservationViewModel();

            if (!model.CanEdit())
                throw new Exception("The edit is not authorized");

            model.AircraftList = _dataService.GetAllAirplanes();
            model.InstructorList = _dataService.GetAllMembersByRole("Instructor");
            model.TimeList = new List<DateTime>().GetListFromRange(
                DateTime.MinValue.AddHours(6), 
                DateTime.MinValue.AddHours(22), 
                new TimeSpan(0, 30, 0)).ConvertAll(x => new SelectListItem() { Value = x.ToString("HH:mm"), Text = x.ToString("HH:mm") })
                .ToList();

            model.AircraftList.Insert(0, new Aircraft());
            model.InstructorList.Insert(0, new Member());
            model.AircraftNumber = reservation.Aircraft.RegistrationNumber;

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(ReservationViewModel model)
        {
            if (ModelState.IsValid)
            {
                if(ValidateReservation(model))
                {
                    Reservation reservation = _dataService.GetReservation(model.Id);
                    reservation.AircraftId = model.AircraftId;
                    reservation.Destination = model.Destination;
                    reservation.EndDate = model.EndDate.Add(TimeSpan.Parse(model.EndTime));
                    reservation.InstructorId = model.InstructorId != 0 ? model.InstructorId : null;
                    reservation.MemberId = model.MemberId;
                    reservation.Notes = model.Notes;
                    reservation.StartDate = model.StartDate.Add(TimeSpan.Parse(model.StartTime));
                    _dataService.SaveReservation(reservation);

                    return RedirectToAction("Details", new { id = model.Id });
                }
            }

            model.AircraftList = _dataService.GetAllAirplanes();
            model.InstructorList = _dataService.GetAllMembersByRole("Instructor");
            model.TimeList = new List<DateTime>().GetListFromRange(DateTime.MinValue.AddHours(6), DateTime.MinValue.AddHours(22), new TimeSpan(0, 30, 0)).ConvertAll(x => new SelectListItem() { Value = x.ToString("HH:mm"), Text = x.ToString("HH:mm") }).ToList();
            
            model.AircraftList.Insert(0, new Aircraft());
            model.InstructorList.Insert(0, new Member());

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            var reservation = _dataService.GetReservation(id);
            var model = reservation.ConvertToReservationViewModel();
            
            if (!model.CanDelete())
                throw new Exception("Not allowed");

            _dataService.DeleteReservation(reservation);

            return RedirectToAction("Index");
        }

        public ActionResult Select()
        {
            var model = new ReservationSelectViewModel()
            {
                AircraftList = _dataService.GetAllAirplanes()
            };

            return View(model);
        }

        public ActionResult AircraftByWeek(int aircraftId, DateTime startDate)
        {
            var memberReservations = _dataService.GetReservationListByAircraft(aircraftId, startDate, 7);
            var aircraft = _dataService.GetAircraftById(aircraftId);

            var model = new AircraftByWeekViewModel()
            {
                ReservationList = memberReservations.ConvertToReservationViewModel(),
                DateList = new List<DateTime>().GetListFromRange(startDate, startDate.AddDays(7), 0, 0, 1),
                TimeList = new List<DateTime>().GetListFromRange(DateTime.MinValue.AddHours(6), DateTime.MinValue.AddHours(22), new TimeSpan(0, 30, 0)),
                AircraftId = aircraftId,
                Aircraft = aircraft
            };

            return View(model);
        }

        public ActionResult AircraftByDay(int aircraftId, DateTime date)
        {
            var memberReservations = _dataService.GetReservationListByAircraft(aircraftId, date, 1);
            var aircraft = _dataService.GetAircraftById(aircraftId);

            var model = new AircraftByDayViewModel()
            {
                Date = date,
                ReservationList = memberReservations.ConvertToReservationViewModel(),
                TimeList = new List<DateTime>().GetListFromRange(DateTime.MinValue.AddHours(6), DateTime.MinValue.AddHours(22), new TimeSpan(0, 30, 0)),
                AircraftId = aircraftId,
                Aircraft = aircraft
            };

            return View(model);
        }
    }
}