using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;

using FlyingClub.Data.Model.Mappings;

namespace FlyingClub.Data.Model.Entities
{
    public class NtxfcDbContext : DbContext
    {
        public NtxfcDbContext() : this("DefaultDb") { }

        public NtxfcDbContext(string connStringName) : base(connStringName)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
        }

        public DbSet<Login> Logins { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Aircraft> Aircraft { get; set; }
        public DbSet<PilotCheckout> PilotCheckouts { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Squawk> Squawks { get; set; }
        public DbSet<SquawkComment> SquawkComments { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AircraftImage> AircraftImages { get; set; }
        public DbSet<FlightReview> FlightReviews { get; set; }
        public DbSet<InstructorData> InstructorData { get; set; }
        public DbSet<InstructorAuthorization> InstructorAuthorizations { get; set; }
        public DbSet<StageCheck> StageChecks { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Configurations.Add(new LoginMapping());
            modelBuilder.Configurations.Add(new MemberMapping());
            modelBuilder.Configurations.Add(new RoleMapping());
            modelBuilder.Configurations.Add(new AircraftMapping());
            modelBuilder.Configurations.Add(new PilotCheckoutMapping());
            modelBuilder.Configurations.Add(new ReservationMapping());
            modelBuilder.Configurations.Add(new SquawkMapping());
            modelBuilder.Configurations.Add(new SquawkCommentMapping());
            modelBuilder.Configurations.Add(new AircraftImageMapping());
            modelBuilder.Configurations.Add(new FlightReviewMapping());
            modelBuilder.Configurations.Add(new StageCheckMapping());
            modelBuilder.Configurations.Add(new InstructorDataMapping());
            modelBuilder.Configurations.Add(new InstructorAuthorizationMapping());
        }
    }
}
