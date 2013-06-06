using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using FlyingClub.Data.Model.Entities;
using System.ComponentModel.DataAnnotations;
using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class AddAircraftOwnerViewModel
    {
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public int OwnerId { get; set; }
        public List<AircraftOwnerInfo> ClubMembers { get; set; }
    }
}