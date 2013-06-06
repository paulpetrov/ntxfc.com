using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.WebApp.Models
{
    public class SubmitErrorViewModel
    {
        public bool ShowErrorDetails { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public int LoginId { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        [AllowHtml]
        public string ExceptionText { get; set; }
        [DataType(DataType.MultilineText)]
        public string UserText { get; set; }
    }
}