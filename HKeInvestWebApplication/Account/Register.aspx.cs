using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Owin;
using HKeInvestWebApplication.Models;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using System.Data;
using System.Data.SqlClient;

namespace HKeInvestWebApplication.Account
{
    public partial class Register : Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();

        private string formatDateToSQL(string DOB)
        {
            string month = DOB.Substring(0, 2);
            string day = DOB.Substring(3, 2);
            string year = DOB.Substring(6, 4);
            return year + "-" + month + "-" + day;
        }

        private bool verifyClient(object sender, EventArgs e)
        {

            string sql = "";
            string accountNumber = AccountNumber.Text.Trim();
            string firstName = FirstName.Text.Trim();
            string lastName = LastName.Text.Trim();
            string DOB = formatDateToSQL(DateOfBirth.Text.Trim());
            string ID = HKID.Text.Trim();
            string email = Email.Text.Trim();

            sql = "SELECT * FROM Client " +
                "WHERE accountNumber = '" + accountNumber + "' " +
                "AND firstName = '" + firstName + "' " +
                "AND lastName = '" + lastName + "' " +
                "AND dateOfBirth = '" + DOB + "' " +
                "AND HKIDPassportNumber = '" + ID + "' " +
                "AND email = '" + email + "' ";

            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) {
                ErrorMessage.Text = "SQL error occurred. Beware of SQL injection.";
                return false; } // If the DataSet is null, a SQL error occurred.
                 
            // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                ErrorMessage.Text = "Information provided is inaccurate or your in person application has not been completed.";
                return false;
            }
            else
            {
                return true;
            }
        }

        protected void CreateUser_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (verifyClient(sender, e)) { }

                var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var signInManager = Context.GetOwinContext().Get<ApplicationSignInManager>();
                var user = new ApplicationUser() { UserName = UserName.Text, Email = Email.Text };
                IdentityResult result = manager.Create(user, Password.Text);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    //string code = manager.GenerateEmailConfirmationToken(user.Id);
                    //string callbackUrl = IdentityHelper.GetUserConfirmationRedirectUrl(code, user.Id, Request);
                    //manager.SendEmail(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>.");

                    IdentityResult roleResult = manager.AddToRole(user.Id, "Client");
                    if (!roleResult.Succeeded)
                    {
                        ErrorMessage.Text = roleResult.Errors.FirstOrDefault();
                    }

                    SqlTransaction trans = myHKeInvestData.beginTransaction();
                    string sql = "UPDATE Account SET userName = '" + UserName.Text.Trim() + "' " +
                        "WHERE accountNumber = '" + AccountNumber.Text.Trim() + "'";

                    myHKeInvestData.setData(sql, trans);
                    myHKeInvestData.commitTransaction(trans);

                    //sql = "SELECT userName FROM account WHERE accountNumber = '" + AccountNumber.Text.Trim() + "'";

                    //DataTable temp = myHKeInvestData.getData(sql);
                    signInManager.SignIn(user, isPersistent: false, rememberBrowser: false);
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);


                }
                else
                {
                    ErrorMessage.Text = result.Errors.FirstOrDefault();
                }
            }
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
                            //TODO validation
                            //args.IsValid = false;
                            AccountValidator.ErrorMessage = "Account number must match last name";
                        }

                    }
                }
                else
                {
                    //TODO VALIDATION
                    //args.IsValid = false;
                    AccountValidator.ErrorMessage = "Account number must match last name.";
                }
            }
        }
    }
}