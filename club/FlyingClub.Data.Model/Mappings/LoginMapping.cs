using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity.ModelConfiguration;

using FlyingClub.Data.Model.Entities;

namespace FlyingClub.Data.Model.Mappings
{
    public class LoginMapping : EntityTypeConfiguration<Login>
    {
        public LoginMapping()
        {
            ToTable("Login");

            HasKey(o => o.Id);
            Property(o => o.Username).IsRequired().HasMaxLength(50);
            Property(o => o.Password).IsRequired().HasMaxLength(128);
            Property(o => o.PasswordSalt).IsRequired().HasMaxLength(128);

            HasMany(o => o.ClubMember).WithRequired(m => m.Login).HasForeignKey(m => m.LoginId);

            //HasOptional(l => l.ClubMember).WithOptionalPrincipal();
            //HasOptional(l => l.ClubMember).WithRequired(m => m.Login).Map(m => m.MapKey("LoginId"));
            //HasRequired(o => o.Member).WithMany();
        }
    }
}
