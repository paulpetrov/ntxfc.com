using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.Composition;

using FlyingClub.Common;
using FlyingClub.WebApp.Models;
using FlyingClub.WebApp.Extensions;
using FlyingClub.Data.Model.Entities;
using FlyingClub.BusinessLogic;
using System.Net.Mail;

namespace FlyingClub.WebApp.Controllers
{ 
    public class SquawkController : BaseController
    {
        private IClubDataService _dataService;

        [ImportingConstructor]
        public SquawkController(IClubDataService dataService)
        {
            _dataService = dataService;
            ViewData["ControllerName"] = "Squawk";
            ViewData["PluralControllerName"] = "Squawks";
        }


        public ViewResult ListActiveSquawks()
        {
            List<Squawk> squawks = null;
            if (User.IsInRole("Admin"))
                squawks = _dataService.GetAllSquawks();
            else
                squawks = _dataService.GetActiveSquawks();

            SquawkListViewModel model = new SquawkListViewModel();
            if (squawks != null && squawks.Count > 0)
            {
                model.SquawkList = squawks.Select(s => new SquawkListItemViewModel()
                    {
                        Id = s.Id,
                        PostedBy = s.PostedBy.FullName,
                        PostedOn = s.PostedOn,
                        Status = s.Status,
                        Subject = s.Subject,
                        TailNumber = s.Aircraft.RegistrationNumber
                    }).ToList();
            }

            return View(ViewNames.ActiveSquawks, model);
        }


        public ViewResult Details(int id)
        {
            Squawk squawk = _dataService.GetSquawkById(id);

            SquawkDetailViewModel model = new SquawkDetailViewModel() 
            { 
                AircraftId = squawk.AircraftId,
                Description = squawk.Description,
                Id = squawk.Id,
                PostedBy = squawk.PostedBy.FullName,
                PostedOn = squawk.PostedOn,
                RegistrationNumber = squawk.Aircraft.RegistrationNumber,
                ResolvedOn = squawk.ResolvedOn,
                ResolutionNotes = squawk.ResolutionNotes,
                Status = squawk.Status,
                Subject = squawk.Subject
            };

            if (squawk.ResolvedById != null)
            {
                Member resolvedBy = _dataService.GetMember((int)squawk.ResolvedById);
                model.ResolvedBy = resolvedBy.FullName;
            }

            foreach (var comment in squawk.Comments)
            {
                SquawkCommentViewModel commentVM = new SquawkCommentViewModel()
                {
                    Id = comment.Id,
                    PostedById = comment.PostedByMemberId,
                    PostedOn = comment.PostDate,
                    Text = comment.Text
                };

                Member poster = _dataService.GetMember(comment.PostedByMemberId);
                commentVM.PostedBy = poster.FullName;

                model.Comments.Add(commentVM);
            }

            return View(ViewNames.SquawkDetails, model);
        }

        public ActionResult Create(int? aircraftId)
        {
            CreateSquawkViewModel model = new CreateSquawkViewModel()
            {
                AircraftList = _dataService.GetAllAirplanes()
            };

            if (aircraftId != null)
            {
                model.AircraftId = (int)aircraftId;
            }

            return View(model);
        } 

        //
        // POST: /Squawk/Create
        [HttpPost]
        public ActionResult Create(CreateSquawkViewModel model)
        {
            if (ModelState.IsValid)
            {
                Squawk squawk = new Squawk()
                {
                    AircraftId = model.AircraftId,
                    Subject = model.Subject,
                    Description = model.Description,
                    GroundAircraft = model.GroundAircraft,
                    PostedById = ProfileCommon.GetProfile().MemberId,
                    PostedOn = DateTime.Now,
                    Status = SquawkStatus.Open.ToString()
                };
                _dataService.AddSquawk(squawk);

                if (model.GroundAircraft)
                    _dataService.UpdateAircraftStatus(model.AircraftId, AircraftStatus.Grounded.ToString());

                try
                {
                    Aircraft aircraft = _dataService.GetAircraftById(model.AircraftId);

                    MailMessage message = new MailMessage();
                    message.Subject = "Squawk posted for " + aircraft.RegistrationNumber;
                    message.From = new MailAddress("admin@ntxfc.com");
                    message.Body = model.Subject + "\t";
                    message.Body += model.Description;

                    List<Member> owners = _dataService.GetAircraftOwners(model.AircraftId);
                    foreach (var owner in aircraft.Owners)
                        message.To.Add(new MailAddress(owner.Login.Email));

                    SendEmail(message);
                }
                catch (Exception ex)
                {
                    LogError("Error while sending squawk notification email for aircraftId " + model.AircraftId + "\t" + ex.ToString());
                }

                return RedirectToAction("ListActiveSquawks");  
            }

            return View(model);
        }

        public ActionResult GetSquawksForAircraft(int id)
        {
            Aircraft aircraft = _dataService.GetAircraftById(id);

            if (aircraft == null)
                throw new HttpException(404, "Aircraft Not Found");

            List<Squawk> squawks = _dataService.GetSquawksByAircraftId(id);

            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            if (profile == null)
                RedirectToAction("LogOn", "Account");
            

            SquawksForAircraftViewModel viewModel = new SquawksForAircraftViewModel()
            {
                AircraftId = id,
                RegistrationNumber = aircraft.RegistrationNumber,
                Name = aircraft.Name,
                Squawks = squawks.ConvertToSquawkItemViewModel(),
            };

            bool isOwner = _dataService.IsAircraftOwner(profile.MemberId, id);
            if (isOwner || User.IsInRole(UserRoles.Admin.ToString()) || User.IsInRole(UserRoles.AircraftMaintenance.ToString()))
                viewModel.CanResolveSquawks = true;
            else
                viewModel.CanResolveSquawks = false;

            return View(ViewNames.SquawksForAircraft, viewModel);
        }

        [Authorize(Roles = "Admin, AircraftOwner, AircraftMaintenance")]
        public ActionResult Edit(int id)
        {
            Squawk squawk = _dataService.GetSquawkById(id);
            List<Aircraft> aircraftList = _dataService.GetAllAirplanes();
            List<Member> memberList = _dataService.GetAllMembersByRole("Admin");

            EditSquawkViewModel viewModel = new EditSquawkViewModel();
            viewModel.AircraftId = squawk.AircraftId;
            viewModel.Description = squawk.Description;
            viewModel.Id = squawk.Id;
            viewModel.PostedById = squawk.PostedById;
            viewModel.PostedBy = squawk.PostedBy.FullName;
            viewModel.PostedOn = squawk.PostedOn;
            viewModel.RegistrationNumber = squawk.Aircraft.RegistrationNumber;
            viewModel.Status = squawk.Status;
            viewModel.Subject = squawk.Subject;

            //TODO: Finish this!
            if (squawk.Comments.Count() > 0)
            {
                viewModel.Comments = squawk.Comments.Select(c => new SquawkCommentViewModel() { Text = c.Text }).ToList(); 
            }

            ViewBag.AircraftId = new SelectList(aircraftList, "Id", "Name", squawk.AircraftId);
            ViewBag.OriginatorId = new SelectList(memberList, "Id", "Status", squawk.PostedById);

            return View(viewModel);
        }

        [Authorize(Roles = "Admin, AircraftOwner, AircraftMaintenance")]
        [HttpPost]
        public ActionResult Edit(EditSquawkViewModel viewModel)
        {

            if (ModelState.IsValid)
            {
                Squawk squawk = _dataService.GetSquawkById(viewModel.Id);
                squawk.Status = viewModel.Status;
                squawk.ResolutionNotes = viewModel.ResolutionNotes;
                squawk.Subject = viewModel.Subject;
                squawk.Description = viewModel.Description;

                _dataService.UpdateSquawk(squawk);

                return RedirectToAction("ListActiveSquawks");
            }

            //List<Aircraft> aircraftList = _dataService.GetAllAirplanes();
            //List<Member> memberList = _dataService.GetAllMembersByRole("Admin");
            //ViewBag.AircraftId = new SelectList(aircraftList, "Id", "Name", squawk.AircraftId);
            //ViewBag.OriginatorId = new SelectList(memberList, "Id", "Status", squawk.PostedById);

            return View(viewModel);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _dataService.DeleteSquawk(id);
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Admin,AircraftOwner,AircraftMaintenance")]
        public ActionResult ResolveSquawk(int squawkId)
        {
            Squawk squawk = _dataService.GetSquawkById(squawkId);

            ResolveSquawkViewModel vm = new ResolveSquawkViewModel();
            vm.Status = squawk.Status;
            vm.AircraftId = squawk.AircraftId;
            vm.Description = squawk.Description;
            vm.Id = squawk.Id;
            vm.RegistrationNumber = squawk.Aircraft.RegistrationNumber;
            vm.ResolutionNotes = squawk.ResolutionNotes;
            vm.Subject = squawk.Subject;

            return View(ViewNames.ResolveSquawk, vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,AircraftOwner,AircraftMaintenance")]
        public ActionResult ResolveSquawk(ResolveSquawkViewModel viewModel)
        {
            ProfileCommon profile = (ProfileCommon)HttpContext.Profile;
            Squawk squawk = _dataService.GetSquawkById(viewModel.Id);
            squawk.ResolutionNotes = viewModel.ResolutionNotes;
            squawk.ResolvedById = profile.MemberId;
            squawk.ResolvedOn = DateTime.Now;
            squawk.Status = viewModel.Status;

            _dataService.UpdateSquawk(squawk);

            SquawkDetailViewModel sqVM = new SquawkDetailViewModel()
            {
                Id = squawk.Id,
                AircraftId = squawk.AircraftId,
                Description = squawk.Description,
                PostedById = squawk.PostedById,
                PostedOn = squawk.PostedOn,
                RegistrationNumber = squawk.Aircraft.RegistrationNumber,
                ResolutionNotes = squawk.ResolutionNotes,
                ResolvedOn = squawk.ResolvedOn,
                ResolvedBy = profile.FirstName + " " + profile.LastName,
                Status = squawk.Status,
                Subject = squawk.Subject
            };

            return View(ViewNames.SquawkDetails, sqVM);
        }

        [HttpGet]
        public ActionResult AddComment(int squawkId)
        {
            SquawkCommentViewModel viewModel = new SquawkCommentViewModel();
            viewModel.SquawkId = squawkId;

            return View("AddSquawkComment", viewModel);
        }

        [HttpPost]
        public ActionResult AddComment(SquawkCommentViewModel viewModel)
        {
            SquawkComment comment = new SquawkComment()
            {
                PostDate = DateTime.Now,
                PostedByMemberId = ((AuthenticatedUser)Session[ContextVariables.AuthenticatedUser]).MemberId,
                SquawkId = viewModel.SquawkId,
                Text = viewModel.Text
            };

            _dataService.AddSquawkComment(comment);

            return RedirectToAction("Details", new { id = viewModel.SquawkId });
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(int commentId)
        {
            SquawkComment comment = _dataService.GetSquawkComment(commentId);
            SquawkCommentViewModel vm = new SquawkCommentViewModel()
            {
                Id = comment.Id,
                Text = comment.Text,
                SquawkId = comment.SquawkId
            };

            return View("DeleteSquawkComment", vm);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteComment(SquawkCommentViewModel viewModel)
        {
            _dataService.DeleteSquawkComment(viewModel.Id);

            return RedirectToAction("Details", new { id = viewModel.SquawkId });
        }
    }
}