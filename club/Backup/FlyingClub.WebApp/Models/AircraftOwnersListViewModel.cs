using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class AircraftOwnersListViewModel
    {
        public AircraftOwnersListViewModel() 
        {
            Owners = new List<MemberViewModel>();
        }
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public List<MemberViewModel> Owners { get; set; }
    }
}