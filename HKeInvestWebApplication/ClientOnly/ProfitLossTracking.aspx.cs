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
using System.Data;
using System.Data.SqlClient;


namespace HKeInvestWebApplication
{
    public partial class ProfitLossTracking : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

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
            DataTable data = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
            if(data == null)
            {
                return false;
            }
            if (data.Rows.Count == 0)
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
           
                if (SearchTypeList.SelectedValue == "individualSecurity")
                {
                    DataTable dtClient = GetIndividualSecurity(SecurityCode.Text);

                    // Bind the GridView to the DataTable.
                    SingleSecurity.DataSource = dtClient;
                    SingleSecurity.DataBind();

                    // Set the visibility of GridView data.
                    SingleSecurity.Visible = true;
                }
                else if (SearchTypeList.SelectedValue == "allSecuritiesOfType")
                {
                    DataTable dtAllGivenSecurities = GetAllGivenTypeSecurity(SecurityTypeList.SelectedValue);

                    // Bind the GridView to the DataTable.
                    SecuritiesGivenType.DataSource = dtAllGivenSecurities;
                    SecuritiesGivenType.DataBind();

                    // Set the visibility of GridView data.
                    SecuritiesGivenType.Visible = true;
                }
                else
                {
                    GetAllSecurities();
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

        /*
        Generates the sum sql's for a given column
        */
        private string generateSumSQL(string column,string accountNumber, string securityCode)
        {
            return "select sum(" + column + ") from OrderHistory inner join Transactions on OrderHistory.referenceNumber = Transactions.referenceNumber where status='completed' and securityCode=" + securityCode + " and OrderHistory.accountNumber='" + accountNumber + "'";
        }

        /*
        Gets the total dollars either buying or selling for a base
        */
        private decimal getDollar(string accountNumber, string sqlBase,string buyOrSell)
        {
            decimal sum = 0;
            string sql = "select executeShares, executePrice " + sqlBase + buyOrSell;
            DataTable dtSecurityHolding = myHKeInvestData.getData(sql);
            if (dtSecurityHolding == null) { return 0; } // If the DataSet is null, a SQL error occurred.

            // no result is returned, then display a message that the account does not hold this type of security.
            if (dtSecurityHolding.Rows.Count == 0)
            {
                return 0;
            }

            // For each security in the result, add it's total dollar amount to the sum
            
            foreach (DataRow row in dtSecurityHolding.Rows)
            {
                sum += Convert.ToDecimal(row["executeShares"]) * Convert.ToDecimal(row["executePrice"]);
            }

            return sum;
        }

        private DataTable GetAllGivenTypeSecurity(string type)
        {
            string accountNumber = getAccountNumber();
            //string type = SecurityTypeList.SelectedValue;
            DataTable dtAllGivenSecurities = new DataTable();
            dtAllGivenSecurities.Columns.Add("dollarBuying");
            dtAllGivenSecurities.Columns.Add("dollarSelling");
            dtAllGivenSecurities.Columns.Add("totalFees");
            dtAllGivenSecurities.Columns.Add("profitLoss");
            DataRow newRow = dtAllGivenSecurities.NewRow();
            newRow["dollarBuying"] = 0;
            newRow["dollarSelling"] = 0;
            newRow["totalFees"] = 0;
            newRow["profitLoss"] = 0;

            dtAllGivenSecurities.Rows.Add(newRow);

            //dtAllGivenSecurities.Rows.Add("dollarBuying");
            string sql = "";
            if(type == "")
            {
                sql = "select distinct securityCode from OrderHistory where accountNumber='" + accountNumber + "'";
            }
            else
            {
                sql = "select distinct securityCode from OrderHistory where securityType='" + type + "' and accountNumber='" + accountNumber + "'";
            }
            DataTable dtClient = myHKeInvestData.getData(sql); // Holds all the security codes that a client owns for the given type
            if (dtClient == null) { return null; } // If the DataSet is null, a SQL error occurred.
                                              //myHKeInvestData.getAggregateValue(“select count(*) from [Person]”);
                                              // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                //lblResultMessage.Text = Context.User.Identity.GetUserName();
                SingleSecurity.Visible = false;
                InvalidCode.Text = "You don't have any transactions with this security";
                return null;
            }
            decimal dollarBuying = 0;
            decimal dollarSelling = 0;
            decimal totalFees = 0;
            decimal profitLoss = 0;
            foreach ( DataRow row in dtClient.Rows)
            {
                DataTable tempSecurityTable = GetIndividualSecurity(row["securityCode"].ToString());
                dollarBuying += decimal.Parse(tempSecurityTable.Rows[0]["dollarBuying"].ToString());
                dollarSelling += decimal.Parse(tempSecurityTable.Rows[0]["dollarSelling"].ToString());
                totalFees += decimal.Parse(tempSecurityTable.Rows[0]["totalFees"].ToString());
                profitLoss += decimal.Parse(tempSecurityTable.Rows[0]["profitLoss"].ToString());
              
            }

            if (ValueListType.SelectedValue == "Percent")
            {
                profitLoss = (profitLoss / dollarBuying) * 100;
            }

            dtAllGivenSecurities.Rows[0]["dollarBuying"] = dollarBuying;
            dtAllGivenSecurities.Rows[0]["dollarSelling"] = dollarSelling;
            dtAllGivenSecurities.Rows[0]["totalFees"] = totalFees;
            dtAllGivenSecurities.Rows[0]["profitLoss"] = profitLoss;
            return dtAllGivenSecurities;
            
        }



        private void GetAllSecurities()
        {
            DataTable dt = GetAllGivenTypeSecurity("");
            // Bind the GridView to the DataTable.
            SecuritiesAll.DataSource = dt;
            SecuritiesAll.DataBind();

            // Set the visibility of GridView data.
            SecuritiesAll.Visible = true;
        }

        private DataTable GetIndividualSecurity(string securityCode)
        {
            // CHANGE THIS TO THE DYNAMIC ACTUAL NUMBERRRRRR
            string accountNumber = getAccountNumber();         // CHANGE THIS TO THE DYNAMIC ACTUAL NUMBERRRRRR

            string secTypeSql = "select securityType from OrderHistory where status='completed' and securityCode=" + securityCode + " and accountNumber='" + accountNumber + "'";
            DataTable dtSecType = myHKeInvestData.getData(secTypeSql);
            string securityType = dtSecType.Rows[0]["securityType"].ToString();
            string sqlBase = "from OrderHistory inner join Transactions on OrderHistory.referenceNumber = Transactions.referenceNumber where status='completed' and securityCode=" + securityCode + " and OrderHistory.accountNumber='" + accountNumber + "'";
            string buy = " and buyOrSell='Buy'";
            string sell = " and buyOrSell='Sell'";
            string sql = "SELECT distinct securityType, name, securityCode,  " +
                "cast('0.00' AS numeric(12,2)) AS shares, " +
                "cast('0.00' AS numeric(12,2)) AS amount, " +
                "cast('0.00' AS numeric(12,2)) AS dollarBuying, " +
                "cast('0.00' AS numeric(12,2)) AS dollarSelling, " +
                "cast('0.00' AS numeric(12,2)) AS totalFees, " +
                "cast('0.00' AS numeric(12,2)) AS profitLoss " +
                "from OrderHistory where status='completed' and securityCode=" + securityCode + " and accountNumber='" + accountNumber + "'"; // Complete the SQL statement.
            DataTable dtClient = myHKeInvestData.getData(sql);
            if (dtClient == null) { return null; } // If the DataSet is null, a SQL error occurred.
                                              //myHKeInvestData.getAggregateValue(“select count(*) from [Person]”);
                                              // If no result is returned by the SQL statement, then display a message.
            if (dtClient.Rows.Count == 0)
            {
                //lblResultMessage.Text = Context.User.Identity.GetUserName();
                SingleSecurity.Visible = false;
                InvalidCode.Text = "You don't have any transactions with this security";
                return null;
            }
            
            var sharesBuy = myHKeInvestData.getAggregateValue(generateSumSQL("executeShares",accountNumber,securityCode) + " and buyOrSell='Buy'");
            var sharesSell = myHKeInvestData.getAggregateValue(generateSumSQL("executeShares",accountNumber,securityCode) + " and buyOrSell='Sell'");
            var sharesRemaining = sharesBuy - sharesSell;

            var dollarBuying = getDollar(accountNumber, sqlBase, buy); 
            var dollarSelling = getDollar(accountNumber, sqlBase, sell); 

            var totalFees = myHKeInvestData.getAggregateValue(generateSumSQL("feesPaid",accountNumber,securityCode));
            var sharesCurrPrice = myExternalFunctions.getSecuritiesPrice(securityType, securityCode);
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

            return dtClient;

            
        }

       
    }

   
}