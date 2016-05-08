using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HKeInvestWebApplication
{
    public partial class RegistrationPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void LastName_TextChanged(object sender, EventArgs e)
        {

        }

        protected void AccountValidator_ServerValidate1(object source, ServerValidateEventArgs args)
        {
            string accountNumber = AccountNumber.Text.Trim();
            string lastName = LastName.Text.Trim().ToUpper();

            if (!accountNumber.Equals("") && !lastName.Equals(""))
            {
                if (accountNumber[0] == lastName[0])
                {
                    if (accountNumber.Length > 1 && accountNumber[1] >= 65 && accountNumber[1] <= 90)
                    {
                        if (accountNumber[1] != lastName[1])
                        {
                            args.IsValid = false;
                            AccountValidator.ErrorMessage = "Account number must match last name";
                        }

                    }
                }
                else
                {
                    args.IsValid = false;
                    AccountValidator.ErrorMessage = "Account number must match last name.";
                }
            }
        }

        protected void ClickRegister(object source, ServerValidateEventArgs args)
        {
            Page.Validate();
            if (Page.IsValid)
            {
                MailMessage mail = new MailMessage();
                SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

                mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest");
                mail.To.Add(Email.Text);
                mail.Subject = "Account Registration Confirmation";
                mail.Body = "Hello, an account with HKeInvest has been created with this email.\n\n" +
                    "The username of this account is: " + UserName.Text + "\n\n" +
                    "If the creation of this account has not been by you, please reply to this email.";

                emailServer.Send(mail);
            }
        }
    }
}