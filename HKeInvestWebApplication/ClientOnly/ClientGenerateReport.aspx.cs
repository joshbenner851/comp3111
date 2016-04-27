using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Data;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class GenerateReport : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        private string sql = "";
        private string accountNumber = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            sql = "SELECT accountNumber FROM Account WHERE userName = '" +
                    Context.User.Identity.GetUserName() + "'";

            DataTable temp = myHKeInvestData.getData(sql);

            if (temp == null || temp.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow row in temp.Rows)
            {
                accountNumber = row["accountNumber"].ToString();
            }

        }

        protected void UpdateSummary()
        {

        }

        protected void UpdateSecurities()
        {

        }

        protected void UpdateOrders()
        {

        }
    }
}