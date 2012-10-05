using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FlyingClub.Data.Model.Entities
{
    public class Aircraft
    {
        public int Id { get; set; }
        
        public string RegistrationNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string TypeDesignation { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        public Nullable<int> MaxGrossWeight { get; set; }
        public Nullable<short> FuelCapacity { get; set; }
        public Nullable<int> UsefulLoad { get; set; }

        //public Nullable<double> EmptyWeight { get; set; }
        //public Nullable<double> EmptyCG { get; set; }
        //public Nullable<double> OilCapacity { get; set; }
        //public Nullable<double> FuelTabCapacity { get; set; }
        //public Nullable<double> FuelArm { get; set; }
        //public Nullable<double> FrontArm { get; set; }
        //public Nullable<double> RearArm { get; set; }
        //public Nullable<double> BaggageArea1Arm { get; set; }
        //public Nullable<double> BaggageArea2Arm { get; set; }

        public Nullable<short> CruiseSpeed { get; set; }
        public Nullable<short> MaxRange { get; set; }
        public string Description { get; set; }
        public string EquipmentList { get; set; }
        public string CheckoutRequirements { get; set; }
        public string OperationalNotes { get; set; }
        public Nullable<short> HourlyRate { get; set; }
        public string BasedAt { get; set; }
        public string Status { get; set; }
        public string StatusNotes { get; set; }
        public Nullable<DateTime> LogUpdloadedOn { get; set; }
        public Nullable<int> LogUploadedByMemberId { get; set; }

        public virtual ICollection<Member> Owners { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Squawk> Squawks { get; set; }
        public virtual ICollection<AircraftImage> Images { get; set; }
        public virtual ICollection<PilotCheckout> Checkouts { get; set; }
    }
}
