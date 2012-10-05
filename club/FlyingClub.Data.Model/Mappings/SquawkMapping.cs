using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class SquawkMapping : EntityTypeConfiguration<Squawk>
    {
        public SquawkMapping()
        {
            ToTable("Squawk");
            HasKey(o => o.Id);
            HasRequired(o => o.PostedBy).WithMany(o => o.SquawksPosted).HasForeignKey(o => o.PostedById);
            HasRequired(o => o.Aircraft).WithMany(o => o.Squawks).HasForeignKey(o => o.AircraftId);
            HasMany(o => o.Comments).WithRequired(c => c.Squawk).HasForeignKey(c => c.SquawkId); ;
        }
    }
}
