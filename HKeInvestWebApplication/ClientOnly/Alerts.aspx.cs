using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNet.Identity;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using HKeInvestWebApplication.Code_File;
using System.Windows.Forms;

namespace HKeInvestWebApplication
{
	public partial class Alerts : System.Web.UI.Page
	{
        HKeInvestData myHKeInvestData = new HKeInvestData();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        InternalFunctions myInternalFunctions = new InternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
		{

        }

        protected void cuvSecurityCodeInput_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string clientUserName = Context.User.Identity.GetUserName();
            string securityType = rblSecurityTypeInput.SelectedValue.ToString();
            string securityCode = tbxSecurityCodeInput.Text.ToString();
            float alertValue = float.Parse(tbxAlertValue.Text.ToString());
            string sql = "";

            // Select "*" because we are just making sure the client has the security
            sql = "SELECT * FROM Account NATURAL JOIN SecurityHolding WHERE userName='" + clientUserName + "' AND code='" + securityCode + "';";

            // Create a DataTable to hold our query to the local database
            DataTable clientSecurity = myHKeInvestData.getData(sql);

            if (clientSecurity == null || clientSecurity.Rows.Count == 0)
            {
                cuvSecurityCodeInput.Text = "Client has no security of this type with this code.";
                cuvSecurityCodeInput.IsValid = false;
            }
            else
            {
                sql = "SELECT price FROM " + securityType + " WHERE code='" + securityCode + "';";
                DataTable desiredSecurity = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);

                if (desiredSecurity == null)
                {
                    cuvSecurityCodeInput.Text = "This security does not exist.";
                    cuvSecurityCodeInput.IsValid = false;
                }

                float price = desiredSecurity.Rows[0].Field<float>("price");
                if (rblAlertType.SelectedValue == "High")
                {
                    if (price > alertValue)
                    {
                        cuvSecurityCodeInput.Text = "The price is already higher than the high alert value.";
                        cuvSecurityCodeInput.IsValid = false;
                    }
                }
                else // Alert type should only either be "High" or "Low"
                {
                    if (price < alertValue)
                    {
                        cuvSecurityCodeInput.Text = "The price is already lower than the low alert value.";
                        cuvSecurityCodeInput.IsValid = false;
                    }
                }
            }
        }

        protected void CreateAlertClick(object sender, EventArgs e)
        {
            if (rfvAlertType.IsValid && rfvAlertValue.IsValid && rfvSecurityCodeInput.IsValid && rfvSecurityTypeInput.IsValid &&
                revAlertValue.IsValid && revSecurityCodeInput.IsValid && cuvSecurityCodeInput.IsValid)
            {
                string securityType = rblSecurityTypeInput.SelectedValue.ToString();
                string securityCode = tbxSecurityCodeInput.Text.ToString();
                string alertType = rblAlertType.SelectedValue.ToString();
                float alertValue = float.Parse(tbxAlertValue.Text.ToString());

                // TODO: make sure to get the primary account holder's email
                DataTable getClientEmail = myHKeInvestData.getData("SELECT email FROM Client WHERE userName='" + Context.User.Identity.GetUserName() + "';");
                string clientEmail = getClientEmail.Rows[0].Field<string>("email");

                myInternalFunctions.createAlert(securityType, securityCode, alertType, alertValue, clientEmail);

                // Reset the page
                rblAlertType.SelectedIndex = -1;
                rblSecurityTypeInput.SelectedIndex = -1;
                tbxSecurityCodeInput.Text = "";
                tbxAlertValue.Text = "";

                MessageBox.Show(new Form { TopMost = true }, "Alert successfully set.");
            }
            else
            {
                MessageBox.Show(new Form { TopMost = true }, "Failed to set alert.");
            }
        }
    }
}