using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}