using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FlyingClub.WebApp.Models
{
    public class AircraftOwnerHomeViewModel
    {
        public AircraftOwnerHomeViewModel()
        {
            Aircraft = new List<AircraftListItemViewModel>();
        }
        public int MemberId { get; set; }
        public string DisplayName { get; set; }
        public List<AircraftListItemViewModel> Aircraft { get; set; }
    }
}