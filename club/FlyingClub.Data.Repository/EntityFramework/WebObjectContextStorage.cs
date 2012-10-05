using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Data.Entity;

namespace FlyingClub.Data.Repository.EntityFramework
{
    /// <example>    
    ///  In web application, esp. MVC app, this is how an ObjectContext is initialized and stored in WebObjectContextStorage.
    ///  This is, again, inspired from S#arp Architect code
    ///  
    /// using Infrastructure.Data;
    /// public class MvcApplication : System.Web.HttpApplication
    /// {
    ///     // other code ...
    ///     public override void Init()
    ///     {
    ///         base.Init();            
    ///         _storage = new WebObjectContextStorage(this);
    ///     }        
    ///      
    ///     protected void Application_BeginRequest(object sender, EventArgs e)
    ///     {
    ///         ObjectContextInitializer.Instance().InitializeObjectContextOnce(() =>
    ///         {
    ///             ObjectContextManager.InitStorage(_storage);
    ///             ObjectContextManager.Init(new[] { Server.MapPath("~/bin/mapping-assembly.dll") });
    ///         });
    ///     }
    ///     private WebObjectContextStorage _storage;
    /// }
    /// </example>    
    public class WebDbContextStorage : IDbContextStorage
    {
        public WebDbContextStorage(HttpApplication app)
        {
            app.EndRequest += (sender, args) =>
            {
                DbContextManager.CloseAllDbContexts();
                HttpContext.Current.Items.Remove(STORAGE_KEY);
            };
        }

        public DbContext GetDbContextForKey(string key)
        {
            SimpleDbContextStorage storage = GetSimpleDbContextStorage();
            return storage.GetDbContextForKey(key);
        }

        public void SetDbContextForKey(string factoryKey, DbContext context)
        {
            SimpleDbContextStorage storage = GetSimpleDbContextStorage();
            storage.SetDbContextForKey(factoryKey, context);
        }

        public IEnumerable<DbContext> GetAllDbContexts()
        {
            SimpleDbContextStorage storage = GetSimpleDbContextStorage();
            return storage.GetAllDbContexts();
        }

        public void Clear()
        {
            HttpContext context = HttpContext.Current;
            SimpleDbContextStorage storage = context.Items[STORAGE_KEY] as SimpleDbContextStorage;
            if (storage != null)
            {
                storage.Clear();
            }
        }

        private SimpleDbContextStorage GetSimpleDbContextStorage()
        {
            HttpContext context = HttpContext.Current;
            SimpleDbContextStorage storage = context.Items[STORAGE_KEY] as SimpleDbContextStorage;
            if (storage == null)
            {
                storage = new SimpleDbContextStorage();
                context.Items[STORAGE_KEY] = storage;
            }
            return storage;
        }

        private const string STORAGE_KEY = "HttpContextObjectContextStorageKey";
    }
}
