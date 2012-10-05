using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using FlyingClub.Common;
using FlyingClub.WebApp.Models;
using FlyingClub.WebApp.Extensions;
using FlyingClub.BusinessLogic;
using FlyingClub.Data.Model.Entities;

namespace FlyingClub.WebApp.Controllers
{
    public class AccountController : BaseController
    {
        IClubDataService _dataService;

        public AccountController()
        {
            _dataService = new ClubDataService();
        }

        
        //
        // GET: /Account/LogOn
        [HttpGet]
        public ActionResult LogOn()
        {
            ViewData["Url"] = Request.Url.Host;
            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    if (ConfigurationManager.AppSettings["ForumAuthenticationEnabled"] == "true")
                    {
                        ForumAuthentication.SetAuthCookie(model.UserName, Request.UserHostAddress, Request.UserAgent, Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
                    }

                    if (!User.IsInRole("Guest"))
                    {
                        Login login = _dataService.GetLoginByUsername(model.UserName);
                        Member member = login.ClubMember.First();
                        AuthenticatedUser user = new AuthenticatedUser()
                        {
                            LoginId = login.Id,
                            Username = model.UserName,
                            FullName = member.FullName,
                            MemberId = member.Id
                        };

                        Session.Add(FlyingClub.Common.ContextVariables.AuthenticatedUser, user);

                        _dataService.UpdateLoggedInDate(login.Id, DateTime.Now);

                    }

                    if ((User.IsInRole("Guest") && Url.IsLocalToHost(returnUrl) && !Url.IsLocalUrl(returnUrl)) || (!User.IsInRole("Guest") && Url.IsLocalToHost(returnUrl)))
                        return Redirect(returnUrl);
                    else if (User.IsInRole("Guest"))
                        return Redirect(ConfigurationManager.AppSettings["FrontEndUrl"]);
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff(string returnUrl)
        {
            if (ConfigurationManager.AppSettings["ForumAuthenticationEnabled"] == "true")
            {
                ForumAuthentication.SignOut();
            }
            FormsAuthentication.SignOut();

            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalToHost(returnUrl))
                returnUrl = Url.Action("Index", "Home");;

            return Redirect(returnUrl);
        }

        //
        // GET: /Account/Register

        //public ActionResult Register()
        //{
        //    RegisterModel model = new RegisterModel();
        //    model.TimeZoneOffset = -6;
        //    return View(model);
        //}

        //
        // POST: /Account/Register

        //[HttpPost]
        //public ActionResult Register(RegisterModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Attempt to register the user
        //        MembershipCreateStatus createStatus;
        //        Membership.CreateUser(model.UserName, model.Password, model.Email, null, null, true, null, out createStatus);

        //        if (createStatus == MembershipCreateStatus.Success)
        //        {
        //            ViewData["FrontEndUrl"] = ConfigurationManager.AppSettings["FrontEndUrl"];
        //            FormsAuthentication.SetAuthCookie(model.UserName, false /* createPersistentCookie */);
        //            ForumAuthentication.SetAuthCookie(model.UserName, Request.UserHostAddress, Request.UserAgent, Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
        //            return View("ThankYou");
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(createStatus));
        //        }
        //    }

        //    // If we got this far, something failed, redisplay form
        //    return View(model);
        //}

        //[HttpGet]
        //public ActionResult Activation(ActivationModel model)
        //{
        //    ViewData["FrontEndUrl"] = ConfigurationManager.AppSettings["FrontEndUrl"];

        //    if (ModelState.IsValid)
        //    {
        //        MembershipActiviationStatus status;
        //        NtxfcMembershipProvider provider = Membership.Provider as NtxfcMembershipProvider;
        //        var user = provider.ActivateUser(model.u, model.i, out status);
                
        //        if (status == MembershipActiviationStatus.Success)
        //        {
        //            ViewData["Message"] = "Congratulations! Your account has been activated.";

        //            FormsAuthentication.SetAuthCookie(model.u, false);

        //            if (ConfigurationManager.AppSettings["ForumAuthenticationEnabled"] == "true")
        //            {
        //                ForumAuthentication.SetAuthCookie(model.u, Request.UserHostAddress, Request.UserAgent, Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);
        //            }
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", ErrorCodeToString(status));
        //        }
        //    }

        //    return View(model);
        //}

        //
        // GET: /Account/ChangePassword

        [HttpGet]
        //[HandleError]
        public ActionResult ChangeEmail()
        {

            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            if (profile == null)
                RedirectToAction("LogOn", "Account");


            Login login = _dataService.GetLoginById(profile.LoginId);
            UpdateEmailsModel model = new UpdateEmailsModel();
            model.Id = login.Id;
            model.PrimaryEmail = login.Email;
            model.SecondaryEmail = login.Email2;

            return View("UpdateEmail", model);
        }

        [HttpPost]
        public ActionResult ChangeEmail(UpdateEmailsModel model)
        {
            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            if (profile == null)
                RedirectToAction("LogOn", "Account");

            if (ModelState.IsValid)
            {
                Login login = _dataService.GetLoginById(profile.LoginId);

                login.Email = model.PrimaryEmail;
                login.Email2 = model.SecondaryEmail;

                _dataService.UpdateLogin(login);

                return RedirectToAction("Details", new { id = login.Id });
            }


            return View("UpdateEmail", model);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult SetPassword(int id)
        {
            Login login = _dataService.GetLoginById(id);
            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                LoginId = id
            };

            return View("SetPassword", model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult SetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Login login = _dataService.GetLoginById(model.LoginId);

                string salt = SimpleHash.GetSalt(32);

                login.Password = SimpleHash.MD5(model.NewPassword, salt);
                login.PasswordSalt = salt;
                _dataService.UpdateLogin(login);

                return RedirectToAction("Details", new { id = login.Id });
            }

            return View("SetPassword", model);
        }

        [Authorize]
        public ActionResult ChangeMyPassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword

        [Authorize]
        [HttpPost]
        public ActionResult ChangeMyPassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                // ChangePassword will throw an exception rather
                // than return false in certain failure scenarios.
                bool changePasswordSucceeded;
                try
                {
                    MembershipUser currentUser = Membership.GetUser(User.Identity.Name, true /* userIsOnline */);
                    changePasswordSucceeded = currentUser.ChangePassword(model.OldPassword, model.NewPassword);
                }
                catch (Exception ex)
                {
                    changePasswordSucceeded = false;
                    throw ex;
                }

                if (changePasswordSucceeded)
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult GetMd5Salt()
        {
            string salt = SimpleHash.GetSalt(32);
            ViewData["salt"] = salt;
            ViewData["hashed"] = SimpleHash.MD5(Request.QueryString["value"], salt);
            return View();
        }

        #region Admin Actions

        [Authorize(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            Login login = _dataService.GetLoginById(id);

            if (login == null)
                throw new HttpException(404, "Account not found");

            AccountViewModel viewModel = new AccountViewModel()
            {
                Email = login.Email,
                LoginId = login.Id,
                UserName = login.Username,
                MemberPIN = login.MemberPIN
            };

            if (login.ClubMember.Count > 0)
                viewModel.HasMember = true;
            else
                viewModel.HasMember = false;

            return View(ViewNames.AccountDetails, viewModel);
        }


        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var model = _dataService.GetAllLogins().Select(a => new AccountViewModel(a)).ToList();
            return View(model);
        }

        //
        // GET: /Account/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View(new AccountViewModel());
        }

        /// <summary>
        /// POST: /Account/Create
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Create(AccountViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                if (_dataService.GetLoginByUsername(viewModel.UserName) == null)
                {
                    string salt = SimpleHash.GetSalt(32);

                    Login login = viewModel.ConvertToEntity();
                    login.Password = SimpleHash.MD5(login.Password, salt);
                    login.PasswordSalt = salt;
                    _dataService.CreateLogin(login);

                    return RedirectToAction("Create", "Member", new { loginId = login.Id });
                }
                else
                {
                    ModelState.AddModelError("UserName", "An Account with this username already exists in the database");
                }
            }

            return View(viewModel);
        }

        /// <summary>
        /// GET: /Account/Edit/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Login login = _dataService.GetLoginById(id);
            AccountEditViewModel viewModel = login.ConvertToEditViewModel();
            return View(viewModel);
        }

        /// <summary>
        /// POST: /Account/Edit/5
        /// </summary>
        /// <param name="aircraft"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(AccountEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Login login = _dataService.GetLoginById(viewModel.LoginId);
                if (login.Username != viewModel.UserName &&
                    _dataService.GetLoginByUsername(viewModel.UserName) != null)
                {
                    ModelState.AddModelError("UserName", "An Account with this username already exists in the database");
                }
                else
                {
                    _dataService.UpdateLoginInfo(viewModel.ConvertToEntity(login));
                    return RedirectToAction("Index");
                }
            }
            return View(viewModel);
        }

        /// <summary>
        /// GET: /Account/Delete/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            _dataService.DeleteLogin(id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        public static string ErrorCodeToString(MembershipActiviationStatus status)
        {
            switch (status)
            {
                case MembershipActiviationStatus.InvalidUsername:
                    return "The user name provided is invalid. Please check the value and try again.";
                case MembershipActiviationStatus.InvalidActivationCode:
                    return "The activation code provided is invalid. Please check the value and try again.";
                case MembershipActiviationStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
