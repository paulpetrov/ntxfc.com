using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlyingClub.Data.Model.Entities;
using System.ComponentModel.DataAnnotations;
using FlyingClub.Common;

namespace FlyingClub.WebApp.Models
{
    public class SquawkListViewModel
    {
        public SquawkListViewModel()
        {
            SquawkList = new List<SquawkListItemViewModel>();
        }

        public List<SquawkListItemViewModel> SquawkList { get; set; }
    }

    public class CreateSquawkViewModel
    {
        public CreateSquawkViewModel()
        {
            
        }

        public int Id { get; set; }
        public int AircraftId { get; set; }
        public int PostedById { get; set; }
        public Member PostedBy { get; set; }
        public string Subject { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public bool GroundAircraft { get; set; }

        public List<Aircraft> AircraftList { get; set; }
    }

    public class EditSquawkViewModel
    {
        public EditSquawkViewModel()
        {
            StatusValues = new List<string>(Enum.GetNames(typeof(SquawkStatus)));
        }

        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public int PostedById { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public string Status { get; set; }

        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string ResolutionNotes { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public int ResolvedById { get; set; }
        public string ResolvedBy { get; set; }

        public List<SquawkCommentViewModel> Comments { get; set; }
        public List<string> StatusValues { get; set; }
        public Squawk Squawk { get; set; }
    }

    public class SquawkDetailViewModel
    {
        public SquawkDetailViewModel()
        {
            Comments = new List<SquawkCommentViewModel>();
        }

        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public int PostedById { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public string Status { get; set; }

        public string Subject { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string ResolutionNotes { get; set; }
        public DateTime? ResolvedOn { get; set; }
        public int ResolvedById { get; set; }
        public string ResolvedBy { get; set; }

        public List<SquawkCommentViewModel> Comments { get; set; }
    }

    public class SquawkCommentViewModel
    {
        public int Id { get; set; }
        public int SquawkId { get; set; }
        public int PostedById { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
    }

    public class SquawkListItemViewModel
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string TailNumber { get; set; }
        public string PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public string Response { get; set; }
        public DateTime? RespondedOn { get; set; }
        public string Status { get; set; }
    }

    public class SquawksForAircraftViewModel
    {
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Name { get; set; }
        public List<SquawkListItemViewModel> Squawks { get; set; }
        public bool CanResolveSquawks { get; set; }
    }

    public static class SquawkExtensionMethods
    {
        public static List<SquawkListItemViewModel> ConvertToSquawkItemViewModel(this List<Squawk> squawkList)
        {
            return squawkList.ConvertAll(x => new SquawkListItemViewModel()
            {
                Id = x.Id,
                Subject = x.Subject,
                TailNumber = x.Aircraft.RegistrationNumber,
                PostedBy = x.PostedBy.FirstName + " " + x.PostedBy.LastName,
                PostedOn = x.PostedOn,
                Status = x.Status
            });
        }
    }
}