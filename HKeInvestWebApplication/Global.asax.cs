using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;

namespace HKeInvestWebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            //Thread template
            Thread mythread = new Thread(PeriodicTask);
            mythread.IsBackground = true;
            mythread.Start();
        }

        //Thread template
        private void PeriodicTask()
        {
            do
            {
                //Method call in here

                //sleep time in ms
                Thread.Sleep(10000);
            } while (true);
        }




    }


}