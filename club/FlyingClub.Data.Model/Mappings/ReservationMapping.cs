using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class ReservationMapping : EntityTypeConfiguration<Reservation>
    {
        public ReservationMapping()
        {
            ToTable("Reservation");
            HasKey(o => o.Id);
            HasRequired(o => o.Member).WithMany(o => o.Reservations).HasForeignKey(o => o.MemberId);
            HasRequired(o => o.Aircraft).WithMany(o => o.Reservations).HasForeignKey(o => o.AircraftId);
        }
    }
}
