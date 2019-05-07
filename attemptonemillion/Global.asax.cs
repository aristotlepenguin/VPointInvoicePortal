﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;

namespace attemptonemillion
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_End(Object sender, EventArgs e)
        {
            if (File.Exists(this.Session["path"].ToString()+"\\display.pdf") && this.Session["path"].ToString()!=@"C:\Users\danie\source\repos\attemptonemillion\attemptonemillion\pdfpaths")
            {
                Debug.Write("Two");
                Directory.Delete(this.Session["path"].ToString(), true);

            }
        }
    }
}