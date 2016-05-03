using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Threading;
using Microsoft.AspNet.Identity;
using HKeInvestWebApplication.Code_File;

namespace HKeInvestWebApplication
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            Thread updateLocalTransaction = new Thread(updateLocalTransactionTask);
            updateLocalTransaction.IsBackground = true;
            updateLocalTransaction.Start();

            Thread checkAlertsThread = new Thread(checkAlerts);
            checkAlertsThread.IsBackground = true;
            checkAlertsThread.Start();
        }

        InternalFunctions intFunction = new InternalFunctions();

        private void updateLocalTransactionTask()
        {
            do
            {
                intFunction.updateLocalTransaction();
                intFunction.updateLocalOrderStatus();
                Thread.Sleep(10000);
            } while (true);
        }

        private void checkAlerts()
        {
            do
            {
                intFunction.checkAlerts();
                Thread.Sleep(10000);
            } while (true);
        }
    } 
}