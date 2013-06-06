using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlyingClub.BusinessLogic;

namespace FlyingClub.WebApp
{
    public class ForumAuthentication
    {
        public static void SetAuthCookie(string username, string host, string useragent, string forwardedfor)
        {
            ClubDataService dataService = new ClubDataService();
            HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["bb_sessionhash"];
            
            if (sessionCookie != null && dataService.IsExistingForumSession(sessionCookie.Value))
            {
                dataService.UpdateForumSession(username, sessionCookie.Value, 1);
            }
            else
            {
                string sessionhash = dataService.SaveForumSession(username, host, useragent, forwardedfor);

                if (!String.IsNullOrEmpty(sessionhash))
                {
                    sessionCookie = new HttpCookie("bb_sessionhash", sessionhash);
                    sessionCookie.Domain = ".ntxfc.com";
                    HttpContext.Current.Response.Cookies.Add(sessionCookie);
                }
            }
        }

        public static void SignOut()
        {
            ClubDataService dataService = new ClubDataService();

            HttpCookie sessionCookie = HttpContext.Current.Request.Cookies["bb_sessionhash"];

            if (sessionCookie != null)
            {
                dataService.DeleteForumSession(sessionCookie.Value);
                sessionCookie.Expires = DateTime.Now.AddDays(-1);
                sessionCookie.Domain = ".ntxfc.com";
                HttpContext.Current.Response.Cookies.Add(sessionCookie);
            }

            HttpCookie cpSessionCookie = HttpContext.Current.Request.Cookies["bb_cpsession"];

            if (cpSessionCookie != null)
            {
                dataService.DeleteForumCpSession(cpSessionCookie.Value);
                cpSessionCookie.Expires = DateTime.Now.AddDays(-1);
                cpSessionCookie.Domain = ".ntxfc.com";
                HttpContext.Current.Response.Cookies.Add(cpSessionCookie);
            }

            HttpCookie useridCookie = HttpContext.Current.Request.Cookies["bb_userid"];

            if (useridCookie != null)
            {
                useridCookie.Expires = DateTime.Now.AddDays(-1);
                useridCookie.Domain = ".ntxfc.com";
                HttpContext.Current.Response.Cookies.Add(useridCookie);
            }

            HttpCookie passwordCookie = HttpContext.Current.Request.Cookies["bb_password"];

            if (passwordCookie != null)
            {
                passwordCookie.Expires = DateTime.Now.AddDays(-1);
                passwordCookie.Domain = ".ntxfc.com";
                HttpContext.Current.Response.Cookies.Add(passwordCookie);
            }
        }
    }
}