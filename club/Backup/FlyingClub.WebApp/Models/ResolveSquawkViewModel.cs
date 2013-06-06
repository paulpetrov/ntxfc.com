using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;

using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class ResolveSquawkViewModel
    {
        public ResolveSquawkViewModel()
        {
            StatusValues = new SelectList(Enum.GetValues(typeof(SquawkStatus))
                .Cast<SquawkStatus>()
                .Where(i => i != SquawkStatus.Archived)
                .Select(i => i.ToString()).ToList(), SquawkStatus.Open.ToString());
            
        }

        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string ResolutionNotes { get; set; }
        public string Status { get; set; }
        public SelectList StatusValues { get; set; }
    }
}