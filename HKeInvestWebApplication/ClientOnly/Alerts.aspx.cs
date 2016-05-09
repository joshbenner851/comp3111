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
            rfvAlertType.Validate();
            rfvSecurityTypeInput.Validate();
            rfvSecurityCodeInput.Validate();
            rfvAlertValue.Validate();
            revSecurityCodeInput.Validate();
            revAlertValue.Validate();

            if (rfvAlertType.IsValid && rfvSecurityTypeInput.IsValid && rfvSecurityCodeInput.IsValid && rfvAlertValue.IsValid 
                && revSecurityCodeInput.IsValid && revAlertValue.IsValid)
            {
                string clientUserName = Context.User.Identity.GetUserName();
                string securityType = rblSecurityTypeInput.SelectedValue.ToString();
                string securityCode = tbxSecurityCodeInput.Text.ToString();
                float alertValue = float.Parse(tbxAlertValue.Text.ToString());
                string sql = "";

                // Select "*" because we are just making sure the client has the security
                sql = "SELECT * "
                    + "FROM Account AS a FULL JOIN SecurityHolding AS sh ON a.accountNumber=sh.accountNumber "
                    + "WHERE userName='" + clientUserName + "' AND code='" + securityCode + "'";
                // Create a DataTable to hold our query to the local database
                DataTable clientSecurity = myHKeInvestData.getData(sql);

                if (clientSecurity == null || clientSecurity.Rows.Count == 0)
                {
                    cuvSecurityCodeInput.ErrorMessage = "Client has no security of this type with this code.";
                    args.IsValid = false;
                }
                else
                {
                    DataTable desiredSecurity = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);

                    if (desiredSecurity == null)
                    {
                        cuvSecurityCodeInput.ErrorMessage = "This security does not exist.";
                        args.IsValid = false;
                    }
                    else
                    {
                        float price;
                        if (securityType == "stock")
                        {
                            price = float.Parse(desiredSecurity.Rows[0]["close"].ToString());
                        }
                        else
                        {
                            price = float.Parse(desiredSecurity.Rows[0]["price"].ToString());
                        }

                        if (rblAlertType.SelectedValue == "High")
                        {
                            if (price > alertValue)
                            {
                                cuvSecurityCodeInput.ErrorMessage = "The price is already higher than the alert value.";
                                args.IsValid = false;
                            }
                        }
                        else // Alert type should only either be "High" or "Low"
                        {
                            if (price < alertValue)
                            {
                                cuvSecurityCodeInput.ErrorMessage = "The price is already lower than the alert value.";
                                args.IsValid = false;
                            }
                        }
                    }
                }
            } 
        }

        protected void CreateAlertClick(object sender, EventArgs e)
        {
            Page.Validate("alertValidation");
            cuvSecurityCodeInput.Validate();
            if (Page.IsValid & cuvSecurityCodeInput.IsValid)
            {
                string securityType = rblSecurityTypeInput.SelectedValue.ToString();
                string securityCode = tbxSecurityCodeInput.Text.ToString();
                string alertType = rblAlertType.SelectedValue.ToString();
                string number = tbxAlertValue.Text.ToString();
                float alertValue = float.Parse(tbxAlertValue.Text.ToString());

                string sql = "";
                sql = "SELECT email "
                    + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                    + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                DataTable getClientEmail = myHKeInvestData.getData(sql);
                if (getClientEmail == null)
                {
                    throw new System.ArgumentNullException("Query for client email returned null.");
                }
                string clientEmail = getClientEmail.Rows[0]["email"].ToString();

                myInternalFunctions.createAlert(securityType, securityCode, alertType, alertValue, clientEmail);

                // Reset the page
                rblAlertType.SelectedIndex = -1;
                rblSecurityTypeInput.SelectedIndex = -1;
                tbxSecurityCodeInput.Text = "";
                tbxAlertValue.Text = "";

                MessageBox.Show(new Form { TopMost = true }, "Alert successfully set.");
            }
        }
    }
}