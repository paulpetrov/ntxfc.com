using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class MemberMapping : EntityTypeConfiguration<Member>
    {
        public MemberMapping()
        {
            ToTable("Member");

            HasKey(o => o.Id);

            Property(o => o.LoginId).IsRequired();

            HasMany(o => o.Roles).WithMany(o => o.Members)
                .Map(m => 
                    {
                        m.MapLeftKey("MemberId");
                        m.MapRightKey("RoleId");
                        m.ToTable("Member_Role");
                    });

            //HasRequired(m => m.Login).WithOptional();

            //HasOptional(m => m.StageChecks).WithRequired().WillCascadeOnDelete(false);    
            //HasMany(o => o.FlightReviews).WithRequired().HasForeignKey(o => o.PilotId);
            
        }
    }
}
