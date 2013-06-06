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
    public class MemberController : BaseController
    {
        private IClubDataService _dataService;

        [ImportingConstructor]
        public MemberController(IClubDataService dataService)
        {
            _dataService = dataService;
        }

        //
        // GET: /Member/
        public ViewResult Index()
        {
            MemberListViewModel viewModel = new MemberListViewModel();
            viewModel.ShowInactive = false;

            //if (User.IsInRole(UserRoles.Admin.ToString()))
            //    members = _dataService.GetAllClubMembers();
            //else
            //    members = _dataService.GetClubMembersByStatus(MemberStatus.Active);

            //List<MemberListItemViewModel> items = members.Select(m => new MemberListItemViewModel()
            //    {
            //        Id = m.Id,
            //        AltPhone = m.AltPhone,
            //        City = m.City,
            //        FullName = m.FullName,
            //        FirstName = m.FirstName,
            //        LastName = m.LastName,
            //        Phone = m.Phone,
            //        PIN = m.Login.MemberPIN,
            //        PrimaryEmail = m.Login.Email,
            //        SecondaryEmail = m.Login.Email2,
            //        Status = m.Status

            //    }).ToList();

            viewModel.Members = GetMemberListItems(false);

            return View(ViewNames.MemberList, viewModel);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ListMembers(bool showInactive)
        {
            MemberListViewModel viewModel = new MemberListViewModel();
            viewModel.ShowInactive = showInactive;
            viewModel.Members = GetMemberListItems(showInactive);
            return View(ViewNames.MemberList, viewModel);
        }

        private List<MemberListItemViewModel> GetMemberListItems(bool showInactive)
        {
            List<Member> members = null;
            MemberListViewModel viewModel = new MemberListViewModel();

            if (showInactive && User.IsInRole(UserRoles.Admin.ToString()))
                members = _dataService.GetAllClubMembers().Where(m => m.Status != "Active").ToList();
            else
                members = _dataService.GetClubMembersByStatus(MemberStatus.Active);

            List<MemberListItemViewModel> items = members.Select(m => new MemberListItemViewModel()
            {
                Id = m.Id,
                AltPhone = m.AltPhone,
                City = m.City,
                FullName = m.FullName,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Phone = m.Phone,
                PIN = m.Login.MemberPIN,
                PrimaryEmail = m.Login.Email,
                SecondaryEmail = m.Login.Email2,
                Status = m.Status

            }).ToList();

            return items;
        }

        public ActionResult MembersXml()
        {
            List<Member> members = null;

            if (User.IsInRole(UserRoles.Admin.ToString()))
                members = _dataService.GetAllClubMembers();
            else
                members = _dataService.GetClubMembersByStatus(MemberStatus.Active);

            List<MemberListItemViewModel> viewModel = members.Select(m => new MemberListItemViewModel()
            {
                Id = m.Id,
                AltPhone = m.AltPhone,
                City = m.City,
                FullName = m.FullName,
                FirstName = m.FirstName,
                LastName = m.LastName,
                Phone = m.Phone,
                PIN = m.Login.MemberPIN,
                PrimaryEmail = m.Login.Email,
                SecondaryEmail = m.Login.Email2,
                Status = m.Status

            }).ToList();

            Response.ContentType = "text/xml";

            return View("MembersXml", viewModel);
        }

        /// <summary>
        /// GET: /Member/Details/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewResult Details(int id)
        {
            Member member = _dataService.GetMember(id);

            if (member == null)
                throw new HttpException(404, "Member not found");

            MemberViewModel model = member.ConvertToViewModel();
            model.UserName = member.Login.Username;
            return View(model);
        }

        public ViewResult MyProfile()
        {
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;

            if (profile == null)
                throw new HttpException(404, "Member not found");

            Member member = _dataService.GetMember(profile.MemberId);

            MemberViewModel model = member.ConvertToViewModel();
            model.UserName = member.Login.Username;
            return View(model);
        }

        [HttpGet]
        public ActionResult EditMyProfile()
        {
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;

            if (profile == null)
                throw new HttpException(404, "Member not found");

            Member member = _dataService.GetMember(profile.MemberId);
            EditMemberViewModel model = new EditMemberViewModel(member);

            return View(model);
        }

        [HttpPost]
        public ActionResult EditMyProfile(EditMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                Member origMember = _dataService.GetMember(model.Id);
                origMember.FirstName = model.FirstName;
                origMember.LastName = model.LastName;
                origMember.Phone = model.Phone;
                origMember.AltPhone = model.AltPhone;
                origMember.AddressLine_1 = model.AddressLine_1;
                origMember.AddressLine_2 = model.AddressLine_2;        
                origMember.City = model.City;
                origMember.State = model.State;
                origMember.Zip = model.Zip;
                origMember.LastMedical = model.LastMedical;
                origMember.TotalHours = model.TotalHours;
                origMember.RetractHours = model.RetractHours;
                origMember.EmergencyName = model.EmergencyName;
                origMember.EmergencyPhone = model.EmergencyPhone;
                // emails updated in separate view
                //origMember.PrimaryEmail = model.PrimaryEmail;
                //origMember.SecondaryEmail = model.SecondaryEmail;

                _dataService.UpdateMember(origMember);
                return RedirectToAction("Details", new { id = model.Id });
            }

            // use this to collect errors and log them
            //var errors = from v in ModelState.Values
            //             where v.Errors.Count > 0
            //             select v.Errors;

            return View(model);
        }

        //public ActionResult Update()
        //{
        //    int memberId = ((ProfileCommon)HttpContext.Profile).MemberId;
        //    Member member = _dataService.GetMember(memberId);
        //    EditMemberViewModel model = new EditMemberViewModel(member);
        //    model.ClubRoles = _dataService.GetAllRoles();
        //    model.MemberRoles = member.Roles.Select(r => r.Id).ToList();

        //    return View(ViewNames.EditMember, model);
        //}

        #region Admin Actions

        /// <summary>
        /// GET: /Member/Create
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Create(int? loginId)
        {
            EditMemberViewModel viewModel = new EditMemberViewModel();
            viewModel.ClubRoles = _dataService.GetAllRoles();
            viewModel.Status = MemberStatus.Pending.ToString();
            viewModel.MemberRoles = new List<int>();
            if (loginId != null)
            {
                Login login = _dataService.GetLoginById((int)loginId);
                viewModel.LoginId = login.Id;
                viewModel.Username = login.Username;
            }
            return View(ViewNames.CreateMember, viewModel);
        }

        /// <summary>
        /// POST: /Member/Create
        /// </summary>
        /// <param name="member"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(EditMemberViewModel model)
        {
            if (ModelState.IsValid)
            {
                Member member = model.ConvertToEntity();
                member.MemberSince = DateTime.Now;

                Login login = null;
                if (model.LoginId == 0)
                    login = _dataService.GetLoginByUsername(model.Username);
                else
                    login = _dataService.GetLoginById(model.LoginId);

                if (login == null)
                {
                    ModelState.AddModelError("Username", String.Format("Username '{0}' can not be found in the database", model.Username));
                    model.ClubRoles = _dataService.GetAllRoles();
                    return View(ViewNames.CreateMember, model);
                }

                member.LoginId = login.Id;

                if (model.MemberRoles != null && model.MemberRoles.Count > 0)
                {
                    List<Role> allRoles = _dataService.GetAllRoles();
                    member.Roles = new List<Role>();
                    foreach (int roleId in model.MemberRoles)
                        member.Roles.Add(allRoles.FirstOrDefault(r => r.Id == roleId));
                }

                member.Status = model.Status;
                _dataService.SaveMember(member);
                return RedirectToAction("Details", new { id = member.Id });
            }

            model.MemberRoles = new List<int>();
            model.ClubRoles = _dataService.GetAllRoles();
            return View(ViewNames.EditMember, model);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult CreateLogin()
        {
            return View(ViewNames.CreateLogin);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateLogin(LoginViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Login login = new Login()
                {
                    Username = viewModel.Username,
                    Password = viewModel.Password
                };
                _dataService.CreateLogin(login);
            }

            return View(ViewNames.CreateLogin, viewModel);
        }

        /// <summary>
        /// GET: /Member/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _dataService.DeleteMember(id);
            return RedirectToAction("Index");
        }

        /// <summary>
        /// GET: /Member/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            int memberId = 0;
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            if (profile == null)
                RedirectToAction("LogOn", "Account");

            // make sure member information is edited by self or admin only
            if (id == null)
            {
                memberId = profile.MemberId;
            }
            else
            {
                if (id == profile.MemberId)
                {
                    memberId = profile.MemberId;
                }
                else
                {
                    if (User.IsInRole(FlyingClub.Common.UserRoles.Admin.ToString()))
                        memberId = (int)id;
                    else
                        throw new HttpException(403, "Operation is not authorized.");
                }
            }

            Member member = _dataService.GetMember(memberId);
            EditMemberViewModel model = new EditMemberViewModel(member);
            model.ClubRoles = _dataService.GetAllRoles();
            model.MemberRoles = member.Roles.Select(r => r.Id).ToList();

            return View(ViewNames.EditMember, model);
        }

        /// <summary>
        /// POST: /Member/Edit/5
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(EditMemberViewModel model)
        {
            List<Role> roles = _dataService.GetAllRoles();
            if (ModelState.IsValid)
            {
                Member origMember = _dataService.GetMember(model.Id); //model.ConvertToEntity();
                model.CopyToEntity(origMember);

                if (User.IsInRole(UserRoles.Admin.ToString()))
                {
                    origMember.Roles = model.MemberRoles.Select(id => roles.FirstOrDefault(cr => cr.Id == id)).ToList();
                    origMember.Status = model.Status;
                }

                _dataService.UpdateMember(origMember);

                return RedirectToAction("Details", new { id = model.Id });
            }

            //var errors = from v in ModelState.Values
            //             where v.Errors.Count > 0
            //             select v.Errors;

            model.ClubRoles = roles;
            return View(ViewNames.EditMember, model);
        }

        #endregion
    }
}