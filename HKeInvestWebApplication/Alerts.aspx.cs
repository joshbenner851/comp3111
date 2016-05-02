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

namespace HKeInvestWebApplication
{
	public partial class Alerts : System.Web.UI.Page
	{
        HKeInvestData myHKeInvestData = new HKeInvestData();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
		{

        }

        protected void cuvSecurityCodeInput_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string clientUserName = Context.User.Identity.GetUserName();
            string securityType = rblSecurityTypeInput.SelectedValue;
            string securityCode = tbxSecurityCodeInput.Text.ToString();
            float alertValue = float.Parse(tbxAlertValue.Text.ToString());
            string sql = "";

            // Select "*" because we are just making sure the client has the security
            sql = "SELECT * FROM Account, SecurityHolding WHERE UserName='" + clientUserName + "' AND code='" + securityCode + "';";

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
                else
                {
                    if (price < alertValue)
                    {
                        cuvSecurityCodeInput.Text = "The price is already lower than the low alert value.";
                        cuvSecurityCodeInput.IsValid = false;
                    }
                }
            }
        }
    }
}