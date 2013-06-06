using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Data.Entity;

using FlyingClub.Data.Repository;
using FlyingClub.Data.Repository.EntityFramework;

namespace FlyingClub.WebApp
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        private WebDbContextStorage _objectContextStorage;

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
#if DEBUG
            //Database.SetInitializer(new FlyingClub.Data.Model.NtxfcDbContextInitializer());
            //FlyingClub.Data.Model.Entities.NtxfcDbContext ctx = new Data.Model.Entities.NtxfcDbContext();
            //ctx.Database.Initialize(true);
#else
            Database.SetInitializer<FlyingClub.Data.Model.Entities.NtxfcDbContext>(null); 
#endif

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        public override void Init()
        {
            base.Init();
            _objectContextStorage = new WebDbContextStorage(this);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            //Database.SetInitializer(new FlyingClub.Data.Model.NtxfcDbContextInitializer());

            DbContextInitializer.Instance().InitializeObjectContextOnce(() =>
            {
                DbContextManager.InitStorage(_objectContextStorage);
                //ObjectContextManager.Init("ntxfc_main", new[] { Server.MapPath("~/bin/FlyingClub.Data.Model.dll") });
                DbContextManager.Init(new[] { Server.MapPath("~/bin/FlyingClub.Data.Model.dll") });
                System.Diagnostics.Trace.WriteLine("BeginRequest: " + DbContextManager.Current.GetHashCode());
            });
        }

    }
}