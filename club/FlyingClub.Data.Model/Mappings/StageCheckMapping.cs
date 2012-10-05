using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class StageCheckMapping : EntityTypeConfiguration<StageCheck>
    {
        public StageCheckMapping()
        {
            ToTable("StageCheck");
            HasRequired(c => c.Pilot).WithMany(p => p.StageChecks).HasForeignKey(c => c.PilotId).WillCascadeOnDelete(false);
        }
    }
}
