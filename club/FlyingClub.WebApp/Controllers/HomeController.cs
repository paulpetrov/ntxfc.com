using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;

using FlyingClub.Common;
using FlyingClub.Data.Model.Entities;
using FlyingClub.WebApp.Extensions;
using FlyingClub.BusinessLogic;
using FlyingClub.WebApp.Models;

namespace FlyingClub.WebApp.Controllers
{
    public class HomeController : BaseController
    {
        private IClubDataService _dataService;

        public HomeController(IClubDataService clubDataService)
        {
            _dataService = clubDataService;
        }


        public ActionResult Index()
        {
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            ViewData["MemberId"] = profile.MemberId;
            ViewData["LoginId"] = profile.LoginId;
            ViewBag.Message = String.Format("Welcome {0} {1} to the North Texas Flying Club Members Area!", profile.FirstName, profile.LastName);
            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "Admin, Instructor")]
        public ActionResult Instructor()
        {
            return View("Instructors");
        }

        public ActionResult OwnerHome(int memberId)
        {
            List<Aircraft> aircraftList = _dataService.GetManagedAircraftForMember(memberId);

            AircraftOwnerHomeViewModel pageVM = new AircraftOwnerHomeViewModel();
            pageVM.MemberId = memberId;

            foreach (var ac in aircraftList)
            {
                AircraftListItemViewModel acVM = new AircraftListItemViewModel()
                {
                    Id = ac.Id,
                    ImageUrl = "",
                    Make = ac.Make,
                    Model = ac.Model,
                    RegistrationNumber = ac.RegistrationNumber
                };

                pageVM.Aircraft.Add(acVM);
            }

            return View("OwnerHome", pageVM);
        }

        public ActionResult UpdateMemberInfo()
        {
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            return RedirectToAction("Details", "Member", new { id = profile.MemberId });
        }

        public ActionResult FuelReceipts()
        {
            return View("FuelReceipts");
        }

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SubmitError(SubmitErrorViewModel model)
        {
            string errorMessage = model.UserText + ":\t" + model.ExceptionText;;
            try
            {
                MailMessage message = new MailMessage();
                message.From = new MailAddress("admin@ntxfc.com");
                message.To.Add(new MailAddress("admin@ntxfc.com"));

                message.Subject = "NTXFC web site error";
                ProfileCommon profile = HttpContext.Profile as ProfileCommon;
                if (profile != null)
                {
                    message.Body = "User " + profile.FirstName + " " + profile.LastName + " (LoginID: " + profile.LoginId.ToString() + ") experienced following error:\t"; 
                }
                message.Body += errorMessage;

                SendEmail(message);
            }
            catch (Exception ex)
            {
                string msg = "Error while sending email with error details:\t" + ex.ToString();
                msg += "Original error was:\t" + errorMessage;
                LogError(errorMessage);
            }


            return RedirectToAction("Index");
        }
    }
}
