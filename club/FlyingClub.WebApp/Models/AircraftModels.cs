using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FlyingClub.Data.Model.Entities;
using FlyingClub.Common;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace FlyingClub.WebApp.Models
{
    public class AircraftIndexViewModel
    {
        public List<FlyingClub.Data.Model.Entities.Aircraft> AircraftList { get; set; }
    }

    public class AircraftListItemViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string ImageUrl { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string HourlyRate { get; set; }
        public string CheckoutRequirements { get; set; }
    }

    public class AircraftDetailsViewModel
    {
        public Aircraft Aircraft { get; set; }
        public AircraftImageListViewModel AircraftImages { get; set; }
    }

    public class AircraftStatusViewModel
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string TypeDesignation { get; set; }
        public string Status { get; set; }
        public string StatusNotes { get; set; }
        public string OperationalNotes { get; set; }
        public string ImageUrl { get; set; }

        public virtual ICollection<Squawk> Squawks { get; set; }
        public Boolean HasSquawks { get; set; }
    }

    public class AircraftImageListViewModel
    {
        public IEnumerable<AircraftImage> ImageList { get; set; }
    }

    public class AircraftImageViewModel
    {
        public AircraftImageViewModel()
        {
            ImageTypes = Enum.GetValues(typeof(AircraftImageTypes)).Cast<AircraftImageTypes>().Select(i => i.ToString()).ToList();
        }

        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string FileName_Small { get; set; }
        public string FileName_Medium { get; set; }
        public string FileName_Large { get; set; }
        public List<string> ImageTypes { get; set; }
    }

    public class AircraftEditViewModel
    {
        public AircraftEditViewModel()
        {
            StatusValues = new SelectList(Enum.GetValues(typeof(AircraftStatus)).Cast<AircraftStatus>().Select(i => i.ToString()).ToList(), AircraftStatus.Online.ToString());
            Owners = new List<AircraftOwnerInfo>();
            Images = new List<AircraftImage>();
        }

        public AircraftEditViewModel(Aircraft entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            RegistrationNumber = entity.RegistrationNumber;
            this.Model = entity.Model;
            TypeDesignation = entity.TypeDesignation;
            Make = entity.Make;
            Year = entity.Year;
            MaxGrossWeight = entity.MaxGrossWeight;
            FuelCapacity = entity.FuelCapacity;
            //FuelTabCapacity = entity.FuelTabCapacity;
            UsefulLoad = entity.UsefulLoad;
            //EmptyWeight = entity.EmptyWeight;
            //EmptyCG = entity.EmptyCG;
            CruiseSpeed = entity.CruiseSpeed;
            MaxRange = entity.MaxRange;
            Description = entity.Description;
            EquipmentList = entity.EquipmentList;
            CheckoutRequirements = entity.CheckoutRequirements;
            OperationalNotes = entity.OperationalNotes;
            HourlyRate = entity.HourlyRate;
            BasedAt = entity.BasedAt;
            Status = entity.Status;
            StatusNotes = entity.StatusNotes;

            if (entity.Owners != null)
                Owners = entity.Owners.Select(o =>
                    new AircraftOwnerInfo() { OwnerId = o.Id, Name = o.FirstName + " " + o.LastName })
                    .ToList();
            if (entity.Images != null)
                Images = entity.Images.ToList();

            StatusValues = new SelectList(Enum.GetValues(typeof(AircraftStatus)).Cast<AircraftStatus>().Select(i => i.ToString()).ToList(), entity.Status);

        }

        public int Id { get; set; }
        
        [Required]
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string TypeDesignation { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public Nullable<int> MaxGrossWeight { get; set; }
        public Nullable<short> FuelCapacity { get; set; }
        public Nullable<int> UsefulLoad { get; set; }
        public Nullable<short> CruiseSpeed { get; set; }
        public Nullable<short> MaxRange { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        [DataType(DataType.MultilineText)]
        public string EquipmentList { get; set; }
        [DataType(DataType.MultilineText)]
        public string CheckoutRequirements { get; set; }
        [DataType(DataType.MultilineText)]
        public string OperationalNotes { get; set; }
        public Nullable<short> HourlyRate { get; set; }
        public string BasedAt { get; set; }
        public string Status { get; set; }
        [DataType(DataType.MultilineText)]
        public string StatusNotes { get; set; }
        public List<AircraftOwnerInfo> Owners { get; set; }
        public List<AircraftImage> Images { get; set; }
        public SelectList StatusValues { get; set; }
    }

    public class AircraftCheckoutViewModel
    {
        public int Id { get; set; }
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }

        [Display(Name = "Checkout Date"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime CheckoutDate { get; set; }
        public int InstructorId { get; set; }
        public string InstructorName { get; set; }
        public int PilotId { get; set; }
        public string PilotName { get; set; }
    }

    public class AircraftOwnerInfo
    {
        public int OwnerId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
    }

    public static class AircraftExtensionMethods
    {
        public static AircraftStatusViewModel ConvertToAircraftStatusViewModel(this Aircraft entity)
        {
            if (entity == null)
                throw new Exception("Aircraft not found");

            string imageType = AircraftImageTypes.ExteriorMain.ToString();
            return new AircraftStatusViewModel()
            {
                Id = entity.Id,
                RegistrationNumber = entity.RegistrationNumber,
                Model = entity.Model,
                TypeDesignation = entity.TypeDesignation,
                Status = entity.Status,
                StatusNotes = entity.StatusNotes,
                OperationalNotes = entity.OperationalNotes,
                Squawks = entity.Squawks,
                HasSquawks = entity.Squawks.Count > 0 ? true : false,
                ImageUrl = (entity.Images != null) && entity.Images.Count > 0
                    ? entity.Images.Where(i => i.Type == imageType).FirstOrDefault().FileName_Small
                    : String.Empty
            };
        }
    }

    public class WeightAndBalanceViewModel
    {
        public WeightAndBalanceViewModel() { }

        public void Intialize(System.Xml.XmlNode wbData)
        {
            System.Xml.XmlNode node = wbData.SelectSingleNode("EmptyWeight");
            if (node != null)
                EmptyWeight = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("MaxGrossWeight");
            if (node != null)
                MaxGrossWeight = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("FuelMax");
            if (node != null)
                FuelCapacity = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("FuelTabs");
            if (node != null)
                FuelTabCapacity = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("arms/EmptyCG");
            if (node != null)
                EmptyCG = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("arms/Fuel");
            if (node != null)
                FuelArm = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("arms/Front1");
            if (node != null)
                FrontArm = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("arms/Rear");
            if (node != null)
                RearArm = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("arms/Baggage1");
            if (node != null)
                Baggage1Arm = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("arms/Baggage2");
            if (node != null)
                Baggage2Arm = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("Baggage1Max");
            if (node != null)
                Baggage1Max = Double.Parse(node.InnerText);

            node = wbData.SelectSingleNode("Baggage2Max");
            if (node != null)
                Baggage2Max = Double.Parse(node.InnerText);

            IsDataAvailable = true;
        }

        
        public int AircraftId { get; set; }
        public string RegistrationNumber { get; set; }
        public string Model { get; set; }
        public string TypeDesignation { get; set; }
        public bool IsDataAvailable { get; set; }
        public double MaxGrossWeight { get; set; }
        public double UsefulLoad { get; set; }
        public double EmptyWeight { get; set; }
        public double EmptyCG { get; set; }
        public double PayloadWithFullFuel { get; set; }
        public double FuelCapacity { get; set; }
        public double FuelTabCapacity { get; set; }
        public double OilCapacity { get; set; }
        public double FuelArm { get; set; }
        public double FrontArm { get; set; }
        public double RearArm { get; set; }
        public double Baggage1Arm { get; set; }
        public double Baggage2Arm { get; set; }
        public double Baggage1Max { get; set; }
        public double Baggage2Max { get; set; }
        public string ImageUrl { get; set; }
    }
}