using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class AircraftImageMapping : EntityTypeConfiguration<AircraftImage>
    {
        public AircraftImageMapping()
        {
            ToTable("AircraftImage");
            HasKey(o => o.Id);
        }

    }
}
