using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class PilotCheckoutMapping : EntityTypeConfiguration<PilotCheckout>
    {
        public PilotCheckoutMapping()
        {
            ToTable("PilotCheckout");
            HasKey(o => o.Id);
            HasRequired(o => o.Pilot).WithMany(o => o.Checkouts).HasForeignKey(o => o.PilotId).WillCascadeOnDelete(false);
            HasRequired(o => o.Aircraft).WithMany(o => o.Checkouts).HasForeignKey(o => o.AircraftId);
        }
    }
}
