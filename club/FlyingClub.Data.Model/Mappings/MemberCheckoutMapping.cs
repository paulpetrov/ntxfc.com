using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class MemberCheckoutMapping : EntityTypeConfiguration<MemberCheckout>
    {
        public MemberCheckoutMapping()
        {
            ToTable("MemberCheckout");
            HasKey(o => o.Id);
            HasRequired(o => o.Member).WithMany(o => o.Checkouts).HasForeignKey(o => o.MemberId).WillCascadeOnDelete(false);
            HasRequired(o => o.Aircraft).WithRequiredDependent();
        }
    }
}
