using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace FlyingClub.Data.Repository.EntityFramework
{
    /// <summary>
    /// Stores object context
    /// </summary>
    public interface IDbContextStorage
    {
        /// <summary>
        /// Gets the db context by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        DbContext GetDbContextForKey(string key);

        /// <summary>
        /// Sets the db context by key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="objectContext">The object context.</param>
        void SetDbContextForKey(string key, DbContext objectContext);

        /// <summary>
        /// Gets all db contexts.
        /// </summary>
        /// <returns></returns>
        IEnumerable<DbContext> GetAllDbContexts();

        void Clear();
    }
}
