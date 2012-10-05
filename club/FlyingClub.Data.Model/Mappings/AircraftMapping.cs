using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class AircraftMapping : EntityTypeConfiguration<Aircraft>
    {
        public AircraftMapping()
        {
            ToTable("Aircraft");

            HasKey(o => o.Id);
            //HasMany(o => o.Owners).WithOptional();
            HasMany(o => o.Owners).WithMany().Map(m => 
            {
                m.MapLeftKey("AircraftId");
                m.MapRightKey("MemberId");
                m.ToTable("AircraftOwner");
            });
            HasMany(o => o.Images).WithRequired();
                //.Map(m =>
                //{
                //    m.MapKey(new string[] { "MemberId", "AircraftId" });
                //    m.ToTable("AircraftOwner");
                //}); ;
        }
    }
}
