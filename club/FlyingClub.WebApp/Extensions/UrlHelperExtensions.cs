using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace FlyingClub.WebApp.Extensions
{
    public static class UrlHelperExtensions
    {
        public static bool IsLocalToHost(this UrlHelper helper, string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                Uri absoluteUri;
                if (Uri.TryCreate(url, UriKind.Absolute, out absoluteUri))
                {
                    if (absoluteUri.Host == "www.ntxfc.com" || absoluteUri.Host == "club.ntxfc.com" || absoluteUri.Host == "dev.ntxfc.com" || absoluteUri.Host == "localhost")
                    {
                        return true;
                    }
                    //return string.Equals(helper.RequestContext.HttpContext.Request.Url.Host, absoluteUri.Host);
                }
                else if (!url.StartsWith("http:", StringComparison.OrdinalIgnoreCase) && !url.StartsWith("https:", StringComparison.OrdinalIgnoreCase) && Uri.IsWellFormedUriString(url, UriKind.Relative))
                {
                    return true;
                }
            }

            return false;
        }
    }
}