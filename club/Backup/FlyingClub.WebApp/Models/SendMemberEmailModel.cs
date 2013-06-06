using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

using FlyingClub.Data.Model.Entities;
using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class SendMemberEmailModel
    {
        public SendMemberEmailModel()
        {
            //MemberRoles = Enum.GetNames(typeof(UserRoles)).Cast<string>().ToList();
        }

        [Required]
        [StringLength(100)]
        public string Subject { get; set;}

        [DataType(DataType.MultilineText)]
        [UIHint("tinymce_jquery_full"), AllowHtml]
        public string EmailText { get; set; }

        public List<Role> MemberRoles { get; set; }
        [Required(ErrorMessage="At least one group must be selected.")]
        public List<string> SendToRoles { get; set; }
    }

    public class SendEmailConfirmationModel
    {
        public string Message { get; set; }
        public int SentNumber { get; set; }
    }
}