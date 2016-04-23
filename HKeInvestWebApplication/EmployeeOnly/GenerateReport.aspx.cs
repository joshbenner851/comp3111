using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Data;

namespace HKeInvestWebApplication
{
    public partial class GenerateReport : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AccountNumber_TextChanged(object sender, EventArgs e)
        {
            lblClientName.Visible = false;
            string sql = "";
            string accountNumber = AccountNumber.Text.Trim(); // Set the account number from a web form control!

            if (accountNumber == "")
            {
                lblClientName.Text = "Please specify an account number.";
                lblClientName.Visible = true;
                return;
            }

            sql = "SELECT firstName, lastName FROM Client " +
                "WHERE Client.accountNumber = '" + accountNumber + "'";

            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) { return; }
            if (dtClient.Rows.Count == 0)
            {
                lblClientName.Text = "No such account number.";
                return;
            }

            // Show the client name(s) on the web page.
            string clientName = "Client(s): ";
            int i = 1;
            foreach (DataRow row in dtClient.Rows)
            {
                clientName = clientName + row["lastName"] + ", " + row["firstName"];
                if (dtClient.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                }
                i = i + 1;
            }
            lblClientName.Text = clientName;
            lblClientName.Visible = true;
        }

    }
}