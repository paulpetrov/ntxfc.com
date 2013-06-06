using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using FlyingClub.Data.Model.Entities;
using FlyingClub.BusinessLogic;

namespace FlyingClub.WebApp
{
    public class MembersPageModel
    {
        public IEnumerable<Member> Members { get; set; }
    }
}