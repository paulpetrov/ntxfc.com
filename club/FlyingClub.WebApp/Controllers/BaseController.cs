using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

using FlyingClub.WebApp.Models;
using System.Net.Mail;

namespace FlyingClub.WebApp.Controllers
{
    /// <summary>
    /// Base class for controllers
    /// </summary>
    public class BaseController : System.Web.Mvc.Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            LogError(filterContext.Exception.ToString());
            try
            {
                //if (filterContext.HttpContext.IsCustomErrorEnabled)
                //{
                SubmitErrorViewModel viewModel = new SubmitErrorViewModel();
                viewModel.ExceptionText = filterContext.Exception.ToString();
                if (User.IsInRole("Admin"))
                {
                    viewModel.ShowErrorDetails = true;
                }

                filterContext.ExceptionHandled = true;
                this.View("Error", viewModel).ExecuteResult(this.ControllerContext);
                //}
            }
            catch (Exception ex)
            {
                LogError("Exception while trying to handle error: " + ex.ToString());
                throw;
            }
        }

        protected void LogError(string userInfo, string errorMessage)
        {
            string logFile = ConfigurationManager.AppSettings["LogFile"];
            string logFileUrl = Server.MapPath(logFile);
            StringBuilder logMessage = new StringBuilder();
            logMessage.Append("************ERROR: " + userInfo + " ******************\t");
            logMessage.AppendLine(DateTime.Now.ToString());
            logMessage.AppendLine(errorMessage);
            logMessage.AppendLine("-------------------------------------------------------------------");

            System.IO.File.AppendAllText(logFileUrl, logMessage.ToString());
        }

        protected void LogError(string errorMessage)
        {
            string userInfo = "";
            //userInfo = "User " + User.Identity.Name + "\t";

            ProfileCommon profile = HttpContext.Profile as ProfileCommon;
            if (profile != null)
            {
                userInfo += "LoginId: " + profile.LoginId.ToString() + " ";
                userInfo += "Name: " + profile.FirstName + " " + profile.LastName + "\t";
            }

            LogError(userInfo, errorMessage);
        }

        protected void SendEmail(MailMessage message)
        {
            if (Boolean.Parse(ConfigurationManager.AppSettings["EnableEmalNotification"]))
            {
                SmtpClient client = new SmtpClient();
                client.Send(message);
            }
        }
    }
}