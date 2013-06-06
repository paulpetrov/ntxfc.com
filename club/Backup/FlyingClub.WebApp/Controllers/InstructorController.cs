using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using FlyingClub.WebApp.Models;
using FlyingClub.Data.Model.Entities;
using FlyingClub.BusinessLogic;
using FlyingClub.Common;

namespace FlyingClub.WebApp.Controllers
{
    public class InstructorController : BaseController
    {
        private IClubDataService _dataService;

        [ImportingConstructor()]
        public InstructorController(IClubDataService dataService)
        {
            _dataService = dataService;
        }

        public ActionResult InstructorDetails(int memberId)
        {
            InstructorData instructor = _dataService.GetInstructorInfoByMemberId(memberId);
            Member member = _dataService.GetMember(memberId);

            InstructorViewModel instructorVM = new InstructorViewModel()
            {
                MemberId = instructor.MemberId,
                AltPhone = member.Phone,
                AvailableForCheckoutsAnnuals = instructor.AvailableForCheckoutsAnnuals,
                InstructOnWeekdayNights = instructor.InstructOnWeekdayNights,
                InstructOnWeekdays = instructor.InstructOnWeekdays,
                InstructOnWeekends = instructor.InstructOnWeekends,
                CeritifcateNumber = instructor.CertificateNumber,
                Comments = instructor.Comments,
                DesignatedForStageChecks = instructor.DesignatedForStageChecks,
                Email = member.Login.Email,
                FullName = member.FullName,
                Id = instructor.Id,
                Phone = member.Phone,
                Ratings = instructor.Ratings
            };

            if (instructor.AuthorizedAircraft != null)
            {
                instructorVM.AuthorizedAircraft = new List<AircraftListItemViewModel>();

                foreach (var ac in instructor.AuthorizedAircraft)
                {
                    AircraftListItemViewModel avm = new AircraftListItemViewModel
                    {
                        Id = ac.AircraftId,
                        RegistrationNumber = ac.Aircraft.RegistrationNumber
                    };
                    instructorVM.AuthorizedAircraft.Add(avm);
                }
            }

            return View(ViewNames.InstructorDetails, instructorVM);
        }

        public ActionResult ListInstructors()
        {
            List<Member> instructors = _dataService.GetAllInstructors();

            List<MemberListItemViewModel> viewModel = new List<MemberListItemViewModel>();
            string status = MemberStatus.Active.ToString();
            viewModel = instructors.Where(m => m.Status == status).Select(m => new MemberListItemViewModel()
            {
                Id = m.Id,
                AltPhone = m.AltPhone,
                City = m.City,
                FullName = m.FullName,
                Phone = m.Phone,
                PIN = m.Login.MemberPIN,
                PrimaryEmail = m.Login.Email,
                SecondaryEmail = m.Login.Email2,
                Status = m.Status

            }).ToList();

            return View(ViewNames.InstructorsList, viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ManageInstructors()
        {
            List<Member> instructors = _dataService.GetAllInstructors();

            List<MemberListItemViewModel> vm = new List<MemberListItemViewModel>();
            foreach (var instructor in instructors)
            {
                MemberListItemViewModel vmItem = new MemberListItemViewModel()
                {
                    Id = instructor.Id,
                    AltPhone = instructor.AltPhone,
                    City = instructor.City,
                    FullName = instructor.FullName,
                    Phone = instructor.Phone,
                    PIN = instructor.Login.MemberPIN,
                    PrimaryEmail = instructor.Login.Email,
                    SecondaryEmail = instructor.Login.Email2,
                    Status = instructor.Status
                };

                vm.Add(vmItem);
            }
            return View(ViewNames.InstructorsList, vm);
        }

        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult UpdateInstructor(int? id)
        {
            if (User.IsInRole("Instructor"))
            {
                ProfileCommon profile = HttpContext.Profile as ProfileCommon;
                int memberId = profile.MemberId;
                InstructorData instructor = _dataService.GetInstructorInfoByMemberId(memberId);

                UpdateInstructorViewModel viewModel = null;
                if (instructor != null)
                {
                    viewModel = new UpdateInstructorViewModel(instructor);
                    if (instructor.AuthorizedAircraft != null && instructor.AuthorizedAircraft.Count > 0)
                        viewModel.AuthorizedAircraft = instructor.AuthorizedAircraft.Select(a => a.AircraftId).ToList();
                }
                else
                {
                    viewModel = new UpdateInstructorViewModel();
                }

                viewModel.MemberId = memberId;
                viewModel.AircraftList = GetAircraftList();

                return View(ViewNames.UpdateInstructor, viewModel);
            }
            else if (User.IsInRole("Admin"))
            {
                if (id == null)
                    throw new HttpException("Must have member ID");

                InstructorData instructor = _dataService.GetInstructorInfoByMemberId((int)id);
                UpdateInstructorViewModel viewModel = null;
                if (instructor != null)
                {
                    viewModel = new UpdateInstructorViewModel(instructor);
                    if (instructor.AuthorizedAircraft != null && instructor.AuthorizedAircraft.Count > 0)
                        viewModel.AuthorizedAircraft = instructor.AuthorizedAircraft.Select(a => a.AircraftId).ToList();
                }
                else
                {
                    viewModel = new UpdateInstructorViewModel();
                }
                    
                viewModel.MemberId = (int)id;
                viewModel.AircraftList = GetAircraftList();

                return View(ViewNames.UpdateInstructor, viewModel);
            }
            else
            {
                throw new HttpException(403, "You are not authorized to perform this operation.");
            }
        }

        private List<AircraftListItemViewModel> GetAircraftList()
        {
            List<AircraftListItemViewModel> avmList = new List<AircraftListItemViewModel>();
            List<Aircraft> aircraft = _dataService.GetAllAirplanes();

            foreach (var a in aircraft)
            {
                AircraftListItemViewModel avm = new AircraftListItemViewModel();
                avm.Id = a.Id;
                avm.RegistrationNumber = a.RegistrationNumber;
                avmList.Add(avm);
            }
            return avmList;
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult UpdateInstructor(UpdateInstructorViewModel viewModel)
        {
            InstructorData instructor = _dataService.GetInstructorInfoByMemberId(viewModel.MemberId);
            if (instructor == null)
                instructor = new InstructorData();

            ProfileCommon profile = ProfileCommon.GetProfile();
            instructor.MemberId = viewModel.MemberId;
            instructor.AvailableForCheckoutsAnnuals = viewModel.AvailableForCheckoutsAnnuals;
            instructor.CertificateNumber = viewModel.CertificateNumber;
            instructor.Ratings = viewModel.Ratings;
            instructor.InstructOnWeekdayNights = viewModel.InstructOnWeekdayNights;
            instructor.InstructOnWeekdays = viewModel.InstructOnWeekdays;
            instructor.InstructOnWeekends = viewModel.InstructOnWeekends;
            instructor.Comments = viewModel.Comments;
            if (User.IsInRole(UserRoles.Admin.ToString()))
            {
                instructor.DesignatedForStageChecks = viewModel.DesignatedForStageChecks;

                if (viewModel.AuthorizedAircraft.Count > 0)
                {
                    if (instructor.AuthorizedAircraft == null)
                        instructor.AuthorizedAircraft = new List<InstructorAuthorization>();

                    foreach (var acId in viewModel.AuthorizedAircraft)
                    {
                        if (instructor.AuthorizedAircraft.Any(aa => aa.AircraftId == acId))
                            continue;

                        InstructorAuthorization auth = new InstructorAuthorization()
                        {
                            AircraftId = acId,
                            InstructorId = viewModel.Id,
                            AuthorizedOn = DateTime.Now,
                            AuthorizedById = profile.MemberId
                        };

                        instructor.AuthorizedAircraft.Add(auth);
                    }
                }
            }

            _dataService.SaveInstructor(instructor);


            //Member member = _dataService.GetMember(instructor.MemberId);

            //InstructorViewModel instructorVM = new InstructorViewModel()
            //{
            //    MemberId = instructor.MemberId,
            //    AltPhone = member.Phone,
            //    AvailableForCheckoutsAnnuals = instructor.AvailableForCheckoutsAnnuals,
            //    InstructOnWeekdayNights = instructor.InstructOnWeekdayNights,
            //    InstructOnWeekdays = instructor.InstructOnWeekdays,
            //    InstructOnWeekends = instructor.InstructOnWeekends,
            //    CeritifcateNumber = instructor.CertificateNumber,
            //    Comments = instructor.Comments,
            //    DesignatedForStageChecks = instructor.DesignatedForStageChecks,
            //    Email = member.Login.Email,
            //    FullName = member.FullName,
            //    Id = instructor.Id,
            //    Phone = member.Phone,
            //    Ratings = instructor.Ratings
            //};

            return RedirectToAction("InstructorDetails", new { memberId = instructor.MemberId });

        }

        [Authorize(Roles = "Admin")]
        public ActionResult RemoveInstructor(int id)
        {
            _dataService.RemoveInstructor(id);

            return RedirectToAction("ManageInstructors");
        }

        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult ReviewPilots()
        {
            List<Member> members = _dataService.GetMembersWithFlightReview();
            List<PilotReviewItemViewModel> pilots = new List<PilotReviewItemViewModel>();
            foreach (var member in members)
            {
                PilotReviewItemViewModel item = new PilotReviewItemViewModel();
                item.PilotId = member.Id;
                item.PilotName = member.FullName;
                item.AircraftCheckouts = String.Empty;

                //foreach (var c in member.Checkouts)
                //{
                //    Aircraft aircraft = _dataService.GetAircraftById(c.AircraftId);
                //    item.AircraftCheckouts += aircraft.RegistrationNumber + ",";
                //}
                //if (item.AircraftCheckouts != String.Empty)
                //    item.AircraftCheckouts = item.AircraftCheckouts.Remove(item.AircraftCheckouts.Length - 1, 1);

                if (member.FlightReviews != null && member.FlightReviews.Count > 0)
                {
                    DateTime reviewDate = member.FlightReviews.OrderByDescending(r => r.Date).First().Date;
                    item.LastReviewDate = reviewDate.ToString("MM-dd-yyyy");
                    if (reviewDate.AddYears(1) < DateTime.Now)
                        item.IsOverdue = true;
                }
                else
                {
                    item.IsOverdue = true;
                }
                    
                pilots.Add(item);
            }

            //List<PilotCheckoutsViewModel> pilots = new List<PilotCheckoutsViewModel>();
            //foreach (var member in members)
            //    pilots.Add(InitializeCheckoutViewModel(member));

            return View(ViewNames.ReviewPilots, pilots);
        }

        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult PilotReview(int memberId)
        {
            PilotReviewViewModel viewModel = new PilotReviewViewModel();
            Member member = _dataService.GetMemberWithPilotData(memberId);
            viewModel = InitializePilotReviewViewModel(member);
            if (User.IsInRole(UserRoles.Admin.ToString()))
            {
                viewModel.CanEditStageChecks = true;
            }
            else
            {
                int instructorId = ProfileCommon.GetProfile().MemberId;
                viewModel.CanEditStageChecks = _dataService.IsDesignatedForStageChecks(instructorId);
            }
            
            return View(ViewNames.PilotReview, viewModel);
        }
        
        [HttpGet]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult AddCheckout(int memberId, string memberName)
        {
            List<Aircraft> availableAircraft = _dataService.GetAircraftAvailableForCheckIn(memberId);
            ProfileCommon profile = ProfileCommon.GetProfile();
            string instructorName = profile.FirstName + " " + profile.LastName;
            AddCheckoutViewModel viewModel = new AddCheckoutViewModel();
            viewModel.PilotName = memberName;
            viewModel.InstructorId = profile.MemberId;
            viewModel.InstructorName = instructorName;
            viewModel.AircraftId = -1;
            viewModel.AircraftList = availableAircraft.Select(a =>
                new AircraftCheckoutInfoViewModel() 
                { 
                    Id = a.Id,
                    RegistrationNumber = a.RegistrationNumber,
                    CheckoutRequirements = a.CheckoutRequirements
                }).ToList();
            viewModel.PilotId = memberId;
            viewModel.CheckoutDate = DateTime.Now;

            return View(ViewNames.AddCheckout, viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult AddCheckout(AddCheckoutViewModel viewModel)
        {
            PilotCheckout checkout = new PilotCheckout();
            checkout.AircraftId = viewModel.AircraftId;
            checkout.PilotId = viewModel.PilotId;
            checkout.InstructorId = viewModel.InstructorId;
            checkout.CheckoutDate = viewModel.CheckoutDate;

            _dataService.AddCheckout(checkout);

            return PilotReview(viewModel.PilotId);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult RemoveCheckout(int checkoutId)
        {
            PilotCheckout checkout = _dataService.GetCheckout(checkoutId);
            AircraftCheckoutViewModel viewModel = new AircraftCheckoutViewModel();
            viewModel.Id = checkout.Id;
            viewModel.CheckoutDate = checkout.CheckoutDate;
            viewModel.AircraftId = checkout.AircraftId;
            viewModel.RegistrationNumber = checkout.Aircraft.RegistrationNumber;
            viewModel.PilotId = checkout.PilotId;
            viewModel.PilotName = checkout.Pilot.FullName;
            
            return View(ViewNames.RemoveCheckout, viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult RemoveCheckout(int checkoutId, int pilotId)
        {
            _dataService.RemoveCheckout(checkoutId);
            return PilotReview(pilotId);
        }

        [HttpGet]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult UpdateFlightReview(int pilotId)
        {
            FlightReviewViewModel viewModel = new FlightReviewViewModel();
            viewModel.PilotId = pilotId;
            ProfileCommon profile = ProfileCommon.GetProfile();

            Member member = _dataService.GetMemberWithPilotData(pilotId);
            viewModel.PilotId = pilotId;
            viewModel.PilotName = member.FullName;

            viewModel.InstructorId = profile.MemberId;
            viewModel.InstructorName = profile.FirstName + " " + profile.LastName;

            if (member.FlightReviews != null && member.FlightReviews.Count() > 0)
            {
                FlightReview lastReview = member.FlightReviews.OrderByDescending(r => r.Date).First();
                viewModel.TotalTime = lastReview.TotalTime;
                viewModel.RetractTime = lastReview.RetractTime;
            }
            viewModel.ReviewDate = DateTime.Now;

            return View(ViewNames.UpdateFlightReview, viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult UpdateFlightReview(FlightReviewViewModel viewModel)
        {
            Member pilot = _dataService.GetMember(viewModel.PilotId);

            FlightReview review = new FlightReview()
            {
                Date = (DateTime)viewModel.ReviewDate,
                InstructorName = viewModel.InstructorName,
                InstructorNotes = viewModel.Notes,
                PilotId = viewModel.PilotId,
                ReviewType = viewModel.ReviewType,
                RetractTime = viewModel.RetractTime,
                TotalTime = viewModel.TotalTime
            };

            _dataService.AddFlightReview(review);

            return PilotReview(pilot.Id);
        }

        [HttpGet]
        [Authorize(Roles="Admin, Instructor")]
        public ActionResult AddStageCheck(int pilotId)
        {
            Member pilot = _dataService.GetMemberWithPilotData(pilotId);
            
            AddStageCheckViewModel viewModel = new AddStageCheckViewModel();
            viewModel.CheckDate = DateTime.Now;
            viewModel.PilotId = pilotId;
            viewModel.PilotName = pilot.FullName;

            viewModel.AvailableStages = new Dictionary<string, string>();
            string[] stagenames = Enum.GetNames(typeof(StageChecks));
            foreach(var name in stagenames)
            {
                if(!pilot.StageChecks.Any(s => s.StageName == name))
                    viewModel.AvailableStages.Add(name, name.ToFriendlyString());
            }

            int instructorId = ProfileCommon.GetProfile().MemberId;
            Member instructor = _dataService.GetMember(instructorId);
            viewModel.InstructorId = instructor.Id;
            viewModel.InstructorName = instructor.FullName;

            return View(ViewNames.AddStageCheck, viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult AddStageCheck(AddStageCheckViewModel viewModel)
        {
            StageCheck stageCheck = new StageCheck()
            {
                Date = viewModel.CheckDate,
                InstructorId = viewModel.InstructorId,
                PilotId = viewModel.PilotId,
                StageName = viewModel.StageName
            };

            _dataService.AddPilotStageCheck(stageCheck);
            return PilotReview(viewModel.PilotId);
        }

        private PilotReviewViewModel InitializePilotReviewViewModel(Member member)
        {
            PilotReviewViewModel pilotVM = new PilotReviewViewModel()
            {
                MemberId = member.Id,
                PilotName = member.FullName,
                AircraftCheckouts = new List<AircraftCheckoutViewModel>()
            };

            pilotVM.FlightReview = new FlightReviewViewModel();
            if (member.FlightReviews != null && member.FlightReviews.Count > 0)
            {
                FlightReview review = member.FlightReviews.OrderByDescending(r => r.Date).First();
                pilotVM.FlightReview.ReviewDate = review.Date;//ToString("MMMM dd, yyyy");
                pilotVM.FlightReview.InstructorName = review.InstructorName;
                pilotVM.FlightReview.Notes = review.InstructorNotes;
                pilotVM.FlightReview.RetractTime = review.RetractTime;
                pilotVM.FlightReview.TotalTime = review.TotalTime;
                pilotVM.FlightReview.ReviewType = review.ReviewType;
                //pilotVM.FlightReview.I = member.FlightReviews.First().Id;
            }  

            foreach (var pilotCheckout in member.Checkouts)
            {
                AircraftCheckoutViewModel checkoutVM = new AircraftCheckoutViewModel();
                checkoutVM.Id = pilotCheckout.Id;
                checkoutVM.AircraftId = pilotCheckout.AircraftId;

                Aircraft aircraft = _dataService.GetAircraftById(pilotCheckout.AircraftId);
                checkoutVM.RegistrationNumber = aircraft.RegistrationNumber;
                checkoutVM.CheckoutDate = pilotCheckout.CheckoutDate;
                checkoutVM.InstructorId = pilotCheckout.InstructorId;

                var instructor = _dataService.GetMember(pilotCheckout.InstructorId);
                checkoutVM.InstructorName = instructor.FullName;

                pilotVM.AircraftCheckouts.Add(checkoutVM);
            }

            if(member.StageChecks != null)
                pilotVM.StageChecks = member.StageChecks.OrderBy(s => s.Date).ToList();

            return pilotVM;
        }

    }
}
