using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class InstructorDataMapping : EntityTypeConfiguration<InstructorData>
    {
        public InstructorDataMapping()
        {
            ToTable("InstructorData");

            HasKey(o => o.Id);
            HasMany(o => o.AuthorizedAircraft).WithRequired(aa => aa.Instructor);

            //HasRequired(o => o.Member).WithOptional();
        }
    }
}
