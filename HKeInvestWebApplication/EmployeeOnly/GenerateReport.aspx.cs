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
            DataTable data;

            AccountNum.Text = accountNumber;

            if (accountNumber == "")
            {
                lblClientName.Text = "Please specify an account number.";
                lblClientName.Visible = true;
                return;
            }

            sql = "SELECT firstName, lastName FROM Client " +
                "WHERE Client.accountNumber = '" + accountNumber + "'";

            data = myHKeInvestData.getData(sql);
            if (data == null) { return; }
            if (data.Rows.Count == 0)
            {
                lblClientName.Text = "No such account number.";
                return;
            }

            // Show the client name(s) on the web page.
            string clientName = "";
            int i = 1;
            foreach (DataRow row in data.Rows)
            {
                clientName = clientName + row["lastName"] + ", " + row["firstName"];
                if (data.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                }
                i++;
            }

            ClientName.Text = clientName;

            sql = "select shares from SecurityHolding where accountNumber='" + accountNumber + "'";
            //Multiply shares by stock price

            sql = "select balance from Account where accountNumber='" + accountNumber + "'";
            data = myHKeInvestData.getData(sql);
            FreeBalance.Text = data.Rows[0][0].ToString();
        }

    }
}