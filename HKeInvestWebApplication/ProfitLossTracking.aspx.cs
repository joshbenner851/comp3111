using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Globalization;
using Microsoft.AspNet.Identity;

namespace HKeInvestWebApplication
{
    public partial class ProfitLossTracking : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();
        ExternalData myExternalData = new ExternalData();

        protected void Page_Load(object sender, EventArgs e)
        {
            //TemplateField tf = new TemplateField();
            //tf.HeaderTemplate = new GridViewLabelTemplate(DataControlRowType.Header, "InsertColumnName", "InsertTypeLike: Int32");

        }

        protected void SearchTypeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(SearchTypeList.SelectedValue == "individualSecurity")
            {
                SingleSecurity.Style.Add("display", "");
                SecuritiesAll.Style.Add("display", "none");
                SecuritiesGivenType.Style.Add("display", "none");
            }
            else if(SearchTypeList.SelectedValue == "allSecuritiesOfType")
            {
                SingleSecurity.Style.Add("display", "none");
                SecuritiesAll.Style.Add("display", "none");
                SecuritiesGivenType.Style.Add("display", "");

            }
            else if (SearchTypeList.SelectedValue == "allSecurities")
            {
                SingleSecurity.Style.Add("display", "none");
                SecuritiesAll.Style.Add("display", "");
                SecuritiesGivenType.Style.Add("display", "none");
            }
        }

        private bool securityCodeIsValid(string securityType, string securityCode)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string dbTableName = textInfo.ToTitleCase(securityType).Replace(" ", string.Empty);
            if (myExternalData.getAggregateValue("select count(*) from [" + dbTableName + "] where [code]='" + securityCode + "'") == 0)
            {
                //showMessage("Invalid or nonexistent " + securityType + " code.\nValue is '" + securityCode + "'.")
                return false;
            }
            return true;
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (!securityCodeIsValid(SecurityTypeList.SelectedValue, SecurityCode.Text))
            {
                InvalidCode.Text = "This code is invalid";
            }
        }


        protected void ShowProfitLoss_Click(object sender, EventArgs e)
        {

            GetIndividualSecurity();

        }

        public string getAccountNumber()
        {
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" +
               HttpContext.Current.User.Identity.GetUserName() + "'";

            DataTable temp = myExternalData.getData(sql);

            //ERROR: no error catching for not having the account number

            return temp.Rows[0]["accountNumber"].ToString();

        }


        private string generateSumSQL(string column,string accountNumber)
        {
            return "select sum(" + column + ") from OrderHistory inner join Transactions on OrderHistory.referenceNumber = Transactions.referenceNumber where status='completed' and securityCode=" + SecurityCode.Text + " and OrderHistory.accountNumber='" + accountNumber + "'";
        }

        private decimal getDollar(string accountNumber, string sqlBase,string buyOrSell)
        {
            decimal sum = 0;
            string sql = "select executeShares, executePrice " + sqlBase + buyOrSell;
            DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
            //if (dtSecurityHolding == null) { return; } // If the DataSet is null, a SQL error occurred.

            // If no result is returned, then display a message that the account does not hold this type of security.
            //if (dtSecurityHolding.Rows.Count == 0)
            //{
                
            //    return;
            //}

            // For each security in the result, add it's total dollar amount to the sum
            
            foreach (DataRow row in dtSecurityHolding.Rows)
            {
                sum += Convert.ToDecimal(row["executeShares"]) * Convert.ToDecimal(row["executePrice"]);
            }

            return sum;
        }

        private void GetIndividualSecurity()
        {
            string securityType = SecurityTypeList.SelectedValue;
            string accountNumber = "YU00000001";
            string sqlBase = "from OrderHistory inner join Transactions on OrderHistory.referenceNumber = Transactions.referenceNumber where status='completed' and securityCode=" + SecurityCode.Text + " and OrderHistory.accountNumber='" + accountNumber + "'";
            string buy = " and buyOrSell='Buy'";
            string sell = " and buyOrSell='Sell'";
            string sql = "SELECT distinct securityType, name, securityCode,  " +
                "cast('0.00' AS numeric(12,2)) AS shares, " +
                "cast('0.00' AS numeric(12,2)) AS amount, " +
                "cast('0.00' AS numeric(12,2)) AS dollarBuying, " +
                "cast('0.00' AS numeric(12,2)) AS dollarSelling, " +
                "cast('0.00' AS numeric(12,2)) AS totalFees, " +
                "cast('0.00' AS numeric(12,2)) AS profitLoss " +
                "from OrderHistory where status='completed' and securityCode=" + SecurityCode.Text + " and accountNumber='" + accountNumber + "'"; // Complete the SQL statement.
            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) { return; } // If the DataSet is null, a SQL error occurred.
                                              //myHKeInvestData.getAggregateValue(“select count(*) from [Person]”);
                                              // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                //lblResultMessage.Text = Context.User.Identity.GetUserName();
                SingleSecurity.Visible = false;
                InvalidCode.Text = "You don't have any transactions with this security";
                return;
            }
            
            var sharesBuy = myHKeInvestData.getAggregateValue(generateSumSQL("executeShares",accountNumber) + " and buyOrSell='Buy'");
            var sharesSell = myHKeInvestData.getAggregateValue(generateSumSQL("executeShares",accountNumber) + " and buyOrSell='Sell'");
            var sharesRemaining = sharesBuy - sharesSell;

            var dollarBuying = getDollar(accountNumber, sqlBase, buy); //myHKeInvestData.getAggregateValue(generateSumSQL("amount",accountNumber)  + " and buyOrSell='Buy'");
            var dollarSelling = getDollar(accountNumber, sqlBase, sell);  //myHKeInvestData.getAggregateValue(generateSumSQL("amount",accountNumber) + " and buyOrSell='Sell'");

            var totalFees = myHKeInvestData.getAggregateValue(generateSumSQL("feesPaid",accountNumber));
            var sharesCurrPrice = myExternalFunctions.getSecuritiesPrice(securityType, SecurityCode.Text);
            var sharesRemainingCurrWorth = sharesRemaining * sharesCurrPrice;

            
            var profitLoss = dollarSelling + sharesRemainingCurrWorth - dollarBuying - totalFees;
            if (ValueListType.SelectedValue == "Percent")
            {
                profitLoss = (profitLoss / dollarBuying) * 100;
            }
            dtClient.Rows[0]["shares"] = sharesRemaining;
            dtClient.Rows[0]["dollarBuying"] = dollarBuying;
            dtClient.Rows[0]["dollarSelling"] = dollarSelling;
            dtClient.Rows[0]["totalFees"] = totalFees;
            dtClient.Rows[0]["profitLoss"] = profitLoss;


            // Bind the GridView to the DataTable.
            SingleSecurity.DataSource = dtClient;
            SingleSecurity.DataBind();

            // Set the visibility of GridView data.
            SingleSecurity.Visible = true;
        }
    }

   
}