using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class SquawkCommentMapping : EntityTypeConfiguration<SquawkComment>
    {
        public SquawkCommentMapping()
        {
            ToTable("SquawkComment");
            HasKey(o => o.Id);
        }
    }
}
