using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;
using System.Data;
using System.Data.SqlClient;


namespace HKeInvestWebApplication
{
    public partial class DepositWithdraw : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestData extData = new HKeInvestData();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(RadioButtonList1.SelectedValue == "Deposit")
            {
                depositInfo.Style.Add("display", "");
                withdrawInfo.Style.Add("display", "none");
            }
            else
            {
                depositInfo.Style.Add("display", "none");
                withdrawInfo.Style.Add("display", "");
            }
        }
        public string getAccountNumber()
        {
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" +
               HttpContext.Current.User.Identity.GetUserName() + "'";

            DataTable temp = myHKeInvestData.getData(sql);

            //ERROR: no error catching for not having the account number

            return temp.Rows[0]["accountNumber"].ToString();

        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(RadioButtonList1.SelectedValue == "Deposit")
            {
                if(Deposit.Text == "")
                {
                    CustomValidator1.ErrorMessage = "Deposit cannot be blank";
                    args.IsValid = false;

                }
                else
                {
                    decimal deposit = decimal.Parse(Deposit.Text);
                    if(deposit <= 0)
                    {
                        CustomValidator1.ErrorMessage = "Deposit must be a positive number";
                        args.IsValid = false;
                    }

                   
                }
            }
        }


        

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (RadioButtonList1.SelectedValue == "Withdraw")
            {
                
                if (Withdraw.Text == "")
                {
                    CustomValidator2.ErrorMessage = "Withdrawal cannot be blank";
                    args.IsValid = false;

                }
                
                else 
                {
                    decimal withdrawal = decimal.Parse(Withdraw.Text);
                    if (withdrawal <= 0)
                    {
                        CustomValidator2.ErrorMessage = "Withdrawal must be a positive number";
                        args.IsValid = false;
                    }
                }
            }
        }

        protected void ExecuteDepositWithdraw_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (RadioButtonList1.SelectedValue == "Withdraw")
                {
                    decimal withdrawal = decimal.Parse(Withdraw.Text);
                    string sql = "select balance from Account where accountNumber='" + getAccountNumber() + "'";
                    DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
                    decimal balance = decimal.Parse(dtSecurityHolding.Rows[0]["balance"].ToString());
                    if (dtSecurityHolding == null || dtSecurityHolding.Rows.Count == 0)
                    {
                        IncorrectAmount.Text = "A sql error occurred or your balance doesn't exist";
                    }


                    //Withdraw money cannot be greater than what's in the account
                    else if (withdrawal > balance)
                    {
                        // money
                        IncorrectAmount.Text = "Cannot withdraw more money than presently in your account";

                    }
                    else
                    {
                        //Withdraw money
                        string depositSql = "update Account set balance=" + (balance - withdrawal) + " where accountNumber='" + getAccountNumber() + "'";
                        SqlTransaction trans = extData.beginTransaction();
                        extData.setData(depositSql, trans);
                        extData.commitTransaction(trans);
                    }
                }
                else
                {
                    decimal deposit = decimal.Parse(Deposit.Text);
                    string sql = "select balance from Account where accountNumber='" + getAccountNumber() + "'";
                    DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
                    decimal balance = decimal.Parse(dtSecurityHolding.Rows[0]["balance"].ToString());
                    if (dtSecurityHolding == null || dtSecurityHolding.Rows.Count == 0)
                    {
                        IncorrectAmount.Text = "A sql error occurred or your balance doesn't exist";
                    }
                    //Deposit money
                    string depositSql = "update Account set balance=" + (balance + deposit) + " where accountNumber='" + getAccountNumber() + "'";
                    SqlTransaction trans = extData.beginTransaction();
                    extData.setData(depositSql, trans);
                    extData.commitTransaction(trans);
                }
            }
            
        }
    }
}