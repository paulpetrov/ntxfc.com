using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.ComponentModel.Composition;

using FlyingClub.Data.Model.Entities;
using FlyingClub.BusinessLogic;
using FlyingClub.WebApp.Models;

namespace FlyingClub.WebApp.Controllers
{
    public class AdminController : BaseController
    {
        IClubDataService _dataService;

        [ImportingConstructor()]
        public AdminController(IClubDataService dataService)
        {
            _dataService = dataService;
        }

        /// <summary>
        /// GET: /Admin/
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult SendMassEmail()
        {
            SendMemberEmailModel vm = new SendMemberEmailModel();
            vm.MemberRoles = _dataService.GetAllRoles();
            
            return View("SendMemberEmail", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult SendMassEmail(SendMemberEmailModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    viewModel.MemberRoles = _dataService.GetAllRoles();
            //    return View("SendMemberEmail", viewModel);
            //}
                

            List<Member> members = _dataService.GetDistinctMembersForRoles(viewModel.SendToRoles);
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            MailMessage message = new System.Net.Mail.MailMessage()
            {
                Subject = viewModel.Subject,
                Body = viewModel.EmailText,
                From = new MailAddress("admin@ntxfc.com"),
                IsBodyHtml = true
            };

            int count = 0;
            foreach (var member in members)
            {
                if (String.IsNullOrEmpty(member.Login.Email) || member.Login.Email.Trim() == String.Empty)
                    continue;

                MailAddress address = null;
                try
                {
                    address = new MailAddress(member.Login.Email);
                }
                catch (FormatException ex)
                {
                    LogError("Error while trying to send email to member " + member.Id + " (" + member.FullName + "). Exception:\n" + ex.ToString());
                    continue;
                }

                //int number = members.Count(m => m.Id == member.Id);
                //System.Diagnostics.Debug.Assert(number == 1);

                message.To.Clear();
                message.To.Add(new MailAddress(member.Login.Email));
                SendEmail(message);

                count++;
            }

            SendEmailConfirmationModel vm = new SendEmailConfirmationModel()
            {
                SentNumber = count
            };

            return View("SendEmailConfirmation", vm);
        }
    }
}
