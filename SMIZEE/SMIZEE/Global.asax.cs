using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using SMIZEE.Models;

namespace SMIZEE
{
    public class Global : HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterOpenAuth();

            //Database.SetInitializer(new SMIZEEDatabaseInitializer());

        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        void Application_Error(object sender, EventArgs e)
        {
            //Exception exc = Server.GetLastError();

            //if (exc is HttpUnhandledException)
            //{
            //    // Pass the error on to the error page.
            //    Server.Transfer("~/ErrorPage.aspx?handler=Application_Error%20-%20Global.asax", true);
            //}

        }

        
    }
}
