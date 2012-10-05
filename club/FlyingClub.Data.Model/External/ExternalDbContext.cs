using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace FlyingClub.Data.Model.External
{
    public class ExternalDbContext : DbContext
    {
        public ExternalDbContext() : this("FrontEndDb") { }

        public ExternalDbContext(string connStringName)
            : base(connStringName)
        {
            this.Configuration.LazyLoadingEnabled = true;
            this.Configuration.ProxyCreationEnabled = false;
            this.Configuration.AutoDetectChangesEnabled = false;
        }
        
        public DbSet<User> Users { get; set; }
        //public DbSet<UserField> UserFields { get; set; }
        //public DbSet<UserActivation> UserActivations { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<CpSession> CpSessions { get; set; }
    }
}
