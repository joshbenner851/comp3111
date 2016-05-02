using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;
using System.Collections.Specialized;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication.ClientOnly
{
    public class SecurityAlert
    {
         ExternalFunctions myExternalFunctions = new ExternalFunctions();

        private string securityType;
        private string securityCode;
        private string alertType;
        private float alertValue;
        private string email;
        private DateTime lastTimeFired;

        public SecurityAlert(string newSecurityType, string newSecurityCode, string newAlertType, float newAlertValue, string newEmail)
        {
            this.securityType = newSecurityType;
            this.securityCode = newSecurityCode;
            this.alertType = newAlertType;
            this.alertValue = newAlertValue;
            this.email = newEmail;
            this.lastTimeFired = null;
        }

        public void checkForTrigger()
        {
            if (lastTimeFired == null || lastTimeFired.AddDays(1) < DateTime.UtcNow)
            {
                if (alertType == "high")
                {
                    DataTable currentSecurity = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
                    float price = currentSecurity.Rows[0].Field<float>("price");

                    if (alertValue < price)
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

                        mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest Alerts");
                        mail.To.Add(email);
                        mail.Subject = "An Alert You've Set Has Been Triggered";
                        mail.Body = "Hello, an alert you've set on a security has been triggered.\n\n" +
                            "The Security in question is:\n" +
                            "Type - " + securityType + "; Code - " + securityCode + "\n" +
                            "The price of the security has exceeded the specified value of: " + alertValue;

                        emailServer.Send(mail);
                    }
                }
                else
                {
                    DataTable currentSecurity = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
                    float price = currentSecurity.Rows[0].Field<float>("price");

                    if (alertValue > price)
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

                        mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest Alerts");
                        mail.To.Add(email);
                        mail.Subject = "An Alert You've Set Has Been Triggered";
                        mail.Body = "Hello, an alert you've set on a security has been triggered.\n\n" +
                            "The Security in question is:\n" +
                            "Type - " + securityType + "; Code - " + securityCode + "\n" +
                            "The price of the security has gone below the specified value of: " + alertValue;

                        emailServer.Send(mail);
                    }
                }
            }
        }
    }

    public partial class Alerts : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalData myExternalData = new ExternalData();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {
            MailMessage mail = new MailMessage();
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

            mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest Alerts");
            mail.To.Add("rms@connect.ust.hk");
            mail.Subject = "Test";
            mail.Body = "Test";

            emailServer.Send(mail);
        }

        protected void cuvSecurityCodeInput_ServerValidate(object source, ServerValidateEventArgs args)
        {
            string clientUserName = Context.User.Identity.GetUserName();
            string securityType = rblSecurityTypeInput.SelectedValue;
            string securityCode = tbxSecurityCodeInput.Text.ToString();
            float alertValue = float.Parse(tbxAlertValue.Text.ToString());
            string sql = "";

            // We select "all" because we are just making sure the client has the security
            sql = "SELECT * FROM Account, SecurityHolding WHERE UserName='"
                + clientUserName + "' AND code='" + securityCode + "';";

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

        protected void CreateAlertClick(object source, ServerValidateEventArgs args)
        {
            if (rfvAlertType.IsValid && rfvSecurityTypeInput.IsValid && rfvSecurityCodeInput.IsValid &&
                rfvAlertValue.IsValid && revSecurityCodeInput.IsValid && revAlertValue.IsValid &&
                cuvSecurityCodeInput.IsValid)
            {
                // TODO: Create Actual Alert
                // new SecurityAlert(string newSecurityType, string newSecurityCode, string newAlertType, float newAlertValue, string newEmail)
            }
        }
    }
}