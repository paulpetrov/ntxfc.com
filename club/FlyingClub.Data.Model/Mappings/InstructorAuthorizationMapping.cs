using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class InstructorAuthorizationMapping : EntityTypeConfiguration<InstructorAuthorization>
    {
        public InstructorAuthorizationMapping()
        {
            ToTable("InstructorAuthorization");
            HasKey(o => o.Id);
            HasRequired(o => o.Aircraft);
        }
    }
}
