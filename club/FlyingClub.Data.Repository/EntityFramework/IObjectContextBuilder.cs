using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace FlyingClub.Data.Repository.EntityFramework
{
    public interface IDbContextBuilder<T> where T : DbContext
    {
        T BuildDbContext();
    }
}
