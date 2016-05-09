using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication.EmployeeOnly
{
    public partial class SecurityTransaction : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void SecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SecurityType.SelectedValue == "Bond" || SecurityType.SelectedValue == "Unit Trust")
            {
                stock.Style.Add("display", "none");
                bondtrust.Style.Add("display", "");
            }
            else if (SecurityType.SelectedValue == "Stock")
            {
                bondtrust.Style.Add("display", "none");
                stock.Style.Add("display", "");
            }
        }

        protected void TransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (TransactionType.SelectedValue == "Buy")
            {
                buyBondTrust.Style.Add("display", "");
                sellBondTrust.Style.Add("display", "none");
                if (SecurityType.SelectedValue == "Bond/Unit Trust")
                {
                    buyBondTrust.Style.Add("display", "");
                }
            }
            else if (TransactionType.SelectedValue == "Sell")
            {
                buyBondTrust.Style.Add("display", "none");
                sellBondTrust.Style.Add("display", "");
                if (SecurityType.SelectedValue == "Bond/Unit Trust")
                {
                    sellBondTrust.Style.Add("display", "");
                }
            }
        }

        protected void OrderType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (OrderType.SelectedValue == "Market Order")
            {
                market.Style.Add("display", "");
                limit.Style.Add("display", "none");
                stop.Style.Add("display", "none");
                stoplimit.Style.Add("display", "none");
            }
            else if (OrderType.SelectedValue == "Limit Order")
            {
                market.Style.Add("display", "none");
                limit.Style.Add("display", "");
                stop.Style.Add("display", "none");
                stoplimit.Style.Add("display", "none");
            }
            else if (OrderType.SelectedValue == "Stop Order")
            {
                market.Style.Add("display", "none");
                limit.Style.Add("display", "none");
                stop.Style.Add("display", "");
                stoplimit.Style.Add("display", "none");
            }
            else if (OrderType.SelectedValue == "Stop Limit Order")
            {
                market.Style.Add("display", "none");
                limit.Style.Add("display", "");
                stop.Style.Add("display", "");
                stoplimit.Style.Add("display", "");
            }
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            //SCHEDULED FOR DELETION 
            //Should be something to handle validation before finally passing the variable

            if (SecurityType.SelectedValue == "Stock" && TransactionType.SelectedValue == "Buy")
            {

                InvalidStockSharesQuantity.Text = stockSharesAmountIsValid(StockSharesQuantity.Text, TransactionType.Text, StockCode.Text.Trim());
            }
            else if (SecurityType.SelectedValue == "Stock" && TransactionType.SelectedValue == "Sell")
            {
                InvalidStockSharesQuantity.Text = stockSharesAmountIsValid(StockSharesQuantity.Text, TransactionType.Text, StockCode.Text.Trim());
            }
            }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TransactionType.SelectedValue == "Sell")
            {
                int shares;
                Int32.TryParse(BondTrustSharesSelling.Text, out shares);
                if (shares <= 0)
                {
                    InvalidBondTrustSharesSelling.Text = "Please a enter a positive number of shares to sell";
                }
                var type = SecurityType.SelectedValue == "Bond" ? "bond" : "unit trust";
                InvalidBondTrustSharesSelling.Text = bondSharesAmountIsValid(type, BondTrustCode.Text.Trim(), BondTrustSharesSelling.Text, "sell");
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TransactionType.SelectedValue == "Buy")
            {
                int shares;
                Int32.TryParse(BondTrustSharesQuantity.Text, out shares);
                if (shares <= 0)
                {
                    InvalidBondTrustSharesQuantity.Text = "Please a enter a positive dollar amount";
                }
                var type = SecurityType.SelectedValue == "Bond" ? "bond" : "unit trust";
                //TODO Check the actual availability
                InvalidBondTrustSharesQuantity.Text = amountIsValid(type, BondTrustCode.Text.Trim(), BondTrustSharesQuantity.Text);
            }
        }

        ExternalFunctions extFunction = new ExternalFunctions();
        HKeInvestData extData = new HKeInvestData();
        InternalFunctions intFunction = new InternalFunctions();

        

        protected void ExecuteOrderClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string varSecurityType = SecurityType.SelectedValue.ToString().Trim().ToLower();
                string varTransactionType = TransactionType.SelectedValue.ToString().Trim().ToLower();
                string accountNumber = getAccountNumber();

                string name = "";

                if (varSecurityType.Equals("stock"))
                {
                    //declare all relevant variables for placing a stock order
                    //Sorry for bad naming convention
                    string varStockCode = StockCode.Text.ToString();
                    string varShares = StockSharesQuantity.Text.ToString();
                    string varOrderType = "";
                    string varExpiryDate = DaysUntilExpiration.SelectedValue;
                    string varAllOrNone = AllOrNone.Checked == true ? "Y" : "N";
                    string varStopPrice = StopPrice.Text.ToString();
                    string varLimitPrice = "";
                    name = extFunction.getSecuritiesByCode("stock", varStockCode).Rows[0]["name"].ToString().Trim();

                    //typeorder
                    if (OrderType.SelectedValue.Equals("Market Order"))
                    {
                        varOrderType = "market";
                    }
                    else if (OrderType.SelectedValue.Equals("Limit Order"))
                    {
                        varOrderType = "limit";
                        varLimitPrice = LimitPrice.Text;
                    }
                    else if (OrderType.SelectedValue.Equals("Stop Order"))
                    {
                        varOrderType = "stop";
                    }
                    else if (OrderType.SelectedValue.Equals("Stop Limit Order"))
                    {
                        varOrderType = "stop limit";
                        varLimitPrice = LimitPrice.Text;
                    }
                    //Check to see if the code exists
                    var validSecurity = extFunction.getSecuritiesByCode("stock", varStockCode);
                    if (validSecurity == null)
                    {
                        //Sell order was not succesfully submitted
                        InvalidStockCode.Text = "The code given does not exist";
                    }
                    else if (varTransactionType.Equals("buy"))
                    {

                        InvalidStockSharesQuantity.Text = stockSharesAmountIsValid(varShares, TransactionType.Text, varStockCode);
                        if (InvalidStockSharesQuantity.Text != "")
                        {
                            return;
                        }

                        //Limit price = high price here
                        string result = extFunction.submitStockBuyOrder(varStockCode, varShares, varOrderType, varExpiryDate, varAllOrNone, varLimitPrice, varStopPrice);

                        if (result != null)
                        {

                            //Figure out how to query with a value that should be zero
                            string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [stockOrderType], [expiryDay], [allOrNone],";


                                if (varLimitPrice != "")
                            {
                                sql += "[limitPrice],";
                            }
                                if(varStopPrice != "")
                            {
                                sql += "[stopPrice],";
                            }
                            string timeNow = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt");

                            sql += "[accountNumber], [name]) VALUES ('" +
                            result + "', '" +
                            varTransactionType.Trim() + "', '" +
                            varSecurityType.Trim() + "', '" +
                            varStockCode + "', '" +
                            timeNow + "', cast('" +
                            varShares + "' as decimal(18,2)), '" +
                            varOrderType + "', '" +
                            varExpiryDate + "', '" +
                            varAllOrNone + "', '";


                                if (varLimitPrice != "")
                            {
                                sql += varLimitPrice + "', '";
                            }
                            if (varStopPrice != "")
                            {
                                sql += varStopPrice + "', '";
                            }
                           sql += accountNumber + "', '" +
                                name + "')";

                            SqlTransaction trans = extData.beginTransaction();
                            extData.setData(sql, trans);
                            extData.commitTransaction(trans);

                            // Get the client's email
                            sql = "SELECT  "
                                + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                                + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                            DataTable clientInfo = extData.getData(sql);
                            if (clientInfo == null)
                            {
                                throw new System.ArgumentNullException("Query for client info returned null.");
                            }
                            string clientEmail = clientInfo.Rows[0]["email"].ToString();

                            // Calculate invoice values
                            DataTable stockInfo = extFunction.getSecuritiesByCode("stock", varStockCode);
                            if (stockInfo == null)
                            {
                                throw new System.ArgumentNullException("Query for stock info returned null.");
                            }

                            float stockPrice = (float)stockInfo.Rows[0]["close"];
                            float totalDollarAmount = stockPrice * float.Parse(varShares);
                            float stockFee;
                            float clientBalance = (float)clientInfo.Rows[0]["balance"];
                            

                            if (OrderType.SelectedValue.Equals("Market Order"))
                            {
                                stockFee = clientBalance < 1000000 ? 150 + (0.004f * totalDollarAmount): 100 + (0.002f * totalDollarAmount);
                            }
                            else if (OrderType.SelectedValue.Equals("Limit Order"))
                            {
                                stockFee = clientBalance < 1000000 ? 150 + (0.006f * totalDollarAmount) : 100 + (0.004f * totalDollarAmount);
                            }
                            else if (OrderType.SelectedValue.Equals("Stop Order"))
                            {
                                stockFee = clientBalance < 1000000 ? 150 + (0.006f * totalDollarAmount) : 100 + (0.004f * totalDollarAmount);
                            }
                            else // Stop Limit Order
                            {
                                stockFee = clientBalance < 1000000 ? 150 + (0.008f * totalDollarAmount) : 100 + (0.006f * totalDollarAmount);
                            }

                            // Calculate transaction invoice values
                            string body = "Hello, you have recently placed an order through the HKeInvest System.\n\n" +
                                "Reference Number: " + result + "\n" +
                                "Account Number: " + accountNumber + "\n" +
                                "Order Type: Stock " + OrderType.SelectedValue + "\n" +
                                "Date Submitted: " + timeNow + "\n" +
                                "Shares Bought: " + varShares + "\n" +
                                "Total Executed Dollar Amount: " + totalDollarAmount.ToString() + "\n" +
                                "Fee Charged: " + stockFee.ToString() + "\n\n" +
                                "Transactions: \n";

                            DataTable transactions = extFunction.getOrderTransaction(result);
                            for (int i = 0; i < transactions.Rows.Count; i++)
                            {
                                body += "Transaction: " + (i + 1).ToString() + "/" + transactions.Rows.Count.ToString() + "\n" +
                                    "\tTransaction Number: " + transactions.Rows[i]["transactionNumber"].ToString() + "\n" +
                                    "\tDate Executed: " + transactions.Rows[i]["executeDate"].ToString() + "\n" +
                                    "\tQuantity of Shares Executed: " + transactions.Rows[i]["executeShares"].ToString() + "\n" +
                                    "\tPrice per Share: " + transactions.Rows[i]["executePrice"].ToString() + "\n";
                            }

                            // Email invoice
                            intFunction.sendInvoiceEmail(clientEmail, "Stock Buy Order Invoice", body);
                        }
                    }
                    else if (TransactionType.SelectedValue.Equals("Sell"))
                    {

                        //Check sell price to see if stock is avlid

                        InvalidStockSharesQuantity.Text = stockSharesAmountIsValid(varShares, TransactionType.Text, varStockCode);
                        //Basically what the stock shares amount is validatesd as (recipe for bad code)
                        if (InvalidStockSharesQuantity.Text != "")
                        {
                            return;
                        }
                        // varLimitPrice = lowPrice
                        string result = extFunction.submitStockSellOrder(varStockCode, varShares, varOrderType, varExpiryDate, varAllOrNone, varLimitPrice, varStopPrice);
                        if (result != null)
                        {
                            //Code to write result into order history table

                            //Tested and properly replicates in the bonds
                            //Testing sql for Sell stock
                            string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [stockOrderType], [expiryDay], [allOrNone],";


                            if (varLimitPrice != "")
                            {
                                sql += "[limitPrice],";
                            }
                            if (varStopPrice != "")
                            {
                                sql += "[stopPrice],";
                            }
                            sql += "[accountNumber], [name]) VALUES ('" +
                            result + "', '" +
                            varTransactionType.Trim() + "', '" +
                            varSecurityType.Trim() + "', '" +
                            varStockCode + "', '" +
                            DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', cast('" +
                            varShares + "' as decimal(18,2)), '" +
                            varOrderType + "', '" +
                            varExpiryDate + "', '" +
                            varAllOrNone + "', '";


                            if (varLimitPrice != "")
                            {
                                sql += varLimitPrice + "', '";
                            }
                            if (varStopPrice != "")
                            {
                                sql += varStopPrice + "', '";
                            }
                            sql += accountNumber + "', '" +
                                 name + "')";

                            SqlTransaction trans = extData.beginTransaction();
                            extData.setData(sql, trans);
                            extData.commitTransaction(trans);                            //Return URL
                        }

                        // Get the client's email
                        string sellSql = "SELECT  "
                            + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                            + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                        DataTable clientInfo = extData.getData(sellSql);
                        if (clientInfo == null)
                        {
                            throw new System.ArgumentNullException("Query for client info returned null.");
                        }
                        string clientEmail = clientInfo.Rows[0]["email"].ToString();

                        // Calculate invoice values
                        DataTable stockInfo = extFunction.getSecuritiesByCode("stock", varStockCode);
                        if (stockInfo == null)
                        {
                            throw new System.ArgumentNullException("Query for stock info returned null.");
                        }

                        float stockPrice = (float)stockInfo.Rows[0]["close"];
                        float totalDollarAmount = stockPrice * float.Parse(varShares);
                        float stockFee;
                        float clientBalance = (float)clientInfo.Rows[0]["balance"];

                        if (OrderType.SelectedValue.Equals("Market Order"))
                        {
                            stockFee = clientBalance < 1000000 ? 150 + (0.004f * totalDollarAmount) : 100 + (0.002f * totalDollarAmount);
                        }
                        else if (OrderType.SelectedValue.Equals("Limit Order"))
                        {
                            stockFee = clientBalance < 1000000 ? 150 + (0.006f * totalDollarAmount) : 100 + (0.004f * totalDollarAmount);
                        }
                        else if (OrderType.SelectedValue.Equals("Stop Order"))
                        {
                            stockFee = clientBalance < 1000000 ? 150 + (0.006f * totalDollarAmount) : 100 + (0.004f * totalDollarAmount);
                        }
                        else // Stop Limit Order
                        {
                            stockFee = clientBalance < 1000000 ? 150 + (0.008f * totalDollarAmount) : 100 + (0.006f * totalDollarAmount);
                        }

                        // Calculate transaction invoice values
                        string body = "Hello, you have recently placed an order through the HKeInvest System.\n\n" +
                            "Reference Number: " + result + "\n" +
                            "Account Number: " + accountNumber + "\n" +
                            "Order Type: Sell Stock " + OrderType.SelectedValue + "\n" +
                            "Date Submitted: " + DateTime.Now.ToString() + "\n" +
                            "Shares Bought: " + varShares + "\n" +
                            "Total Executed Dollar Amount: " + totalDollarAmount.ToString() + "\n" +
                            "Fee Charged: " + stockFee.ToString() + "\n\n" +
                            "Transactions: \n";

                        DataTable transactions = extFunction.getOrderTransaction(result);
                        for (int i = 0; i < transactions.Rows.Count; i++)
                        {
                            body += "Transaction: " + (i + 1).ToString() + "/" + transactions.Rows.Count.ToString() + "\n" +
                                "\tTransaction Number: " + transactions.Rows[i]["transactionNumber"].ToString() + "\n" +
                                "\tDate Executed: " + transactions.Rows[i]["executeDate"].ToString() + "\n" +
                                "\tQuantity of Shares Executed: " + transactions.Rows[i]["executeShares"].ToString() + "\n" +
                                "\tPrice per Share: " + transactions.Rows[i]["executePrice"].ToString() + "\n";
                        }

                        // Email invoice
                        intFunction.sendInvoiceEmail(clientEmail, "Stock Sell Order Invoice", body);
                    }
                }
                else
                {
                    string varBondTrustCode = BondTrustCode.Text.ToString();
                    if (TransactionType.SelectedValue.Equals("Buy"))
                    {

                        string varBondTrustSharesAmount = BondTrustSharesQuantity.Text.ToString();
                        if (SecurityType.SelectedValue.Equals("Bond"))
                        {
                            var validSecurity = extFunction.getSecuritiesByCode("bond", varBondTrustCode);
                            if (validSecurity == null)
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                            else
                            {


                                string result = extFunction.submitBondBuyOrder(varBondTrustCode, varBondTrustSharesAmount);

                                if (result != null)
                                {

                                    //Yes. This is a redundant execution
                                    name = extFunction.getSecuritiesByCode("bond", varBondTrustCode).Rows[0]["name"].ToString().Trim();

                                    string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [amount], [accountNumber], [name]) VALUES ('" +
                                       result + "', '" +
                                       varTransactionType + "', '" +
                                       varSecurityType + "', '" +
                                       varBondTrustCode + "', '" +
                                       DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                       varBondTrustSharesAmount + "', '" +

                                       accountNumber + "', '"+
                                       name + "')";

                                    SqlTransaction trans = extData.beginTransaction();
                                    extData.setData(sql, trans);
                                    extData.commitTransaction(trans);

                                    // Get the client's email
                                    string sellSql = "SELECT  "
                                        + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                                        + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                                    DataTable clientInfo = extData.getData(sellSql);
                                    if (clientInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for client info returned null.");
                                    }
                                    string clientEmail = clientInfo.Rows[0]["email"].ToString();

                                    // Calculate invoice values
                                    DataTable stockInfo = extFunction.getSecuritiesByCode("bond", varBondTrustCode);
                                    if (stockInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for stock info returned null.");
                                    }

                                    float stockPrice = (float)stockInfo.Rows[0]["close"];
                                    float totalDollarAmount = stockPrice * float.Parse(varBondTrustSharesAmount);
                                    float clientBalance = (float)clientInfo.Rows[0]["balance"];
                                    float stockFee = clientBalance < 500000 ? (0.05f * totalDollarAmount) : (0.03f * totalDollarAmount);

                                    // Calculate transaction invoice values
                                    string body = "Hello, you have recently placed an order through the HKeInvest System.\n\n" +
                                        "Reference Number: " + result + "\n" +
                                        "Account Number: " + accountNumber + "\n" +
                                        "Order Type: Buy Bond " + OrderType.SelectedValue + "\n" +
                                        "Date Submitted: " + DateTime.Now.ToString() + "\n" +
                                        "Shares Bought: " + varBondTrustSharesAmount + "\n" +
                                        "Total Executed Dollar Amount: " + totalDollarAmount.ToString() + "\n" +
                                        "Fee Charged: " + stockFee.ToString() + "\n\n" +
                                        "Transactions: \n";

                                    DataTable transactions = extFunction.getOrderTransaction(result);
                                    for (int i = 0; i < transactions.Rows.Count; i++)
                                    {
                                        body += "Transaction: " + (i + 1).ToString() + "/" + transactions.Rows.Count.ToString() + "\n" +
                                            "\tTransaction Number: " + transactions.Rows[i]["transactionNumber"].ToString() + "\n" +
                                            "\tDate Executed: " + transactions.Rows[i]["executeDate"].ToString() + "\n" +
                                            "\tQuantity of Shares Executed: " + transactions.Rows[i]["executeShares"].ToString() + "\n" +
                                            "\tPrice per Share: " + transactions.Rows[i]["executePrice"].ToString() + "\n";
                                    }

                                    // Email invoice
                                    intFunction.sendInvoiceEmail(clientEmail, "Stock Sell Order Invoice", body);
                                }

                            }
                        }
                        else if (SecurityType.SelectedValue.Equals("Unit Trust"))
                        {

                            var validSecurity = extFunction.getSecuritiesByCode("unit trust", varBondTrustCode);
                            if (validSecurity == null)
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                            else
                            {
                                string result = extFunction.submitUnitTrustBuyOrder(varBondTrustCode, varBondTrustSharesAmount);

                                if (result != null)
                                {

                                    name = extFunction.getSecuritiesByCode("unit trust", varBondTrustCode).Rows[0]["name"].ToString().Trim();

                                    string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [amount], [accountNumber], [name]) VALUES ('" +
                                      result + "', '" +
                                      varTransactionType + "', '" +
                                      varSecurityType + "', '" +
                                      varBondTrustCode + "', '" +
                                      DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                      varBondTrustSharesAmount + "', '" +

                                      accountNumber + "', '"+
                                      name +"')";

                                    SqlTransaction trans = extData.beginTransaction();
                                    extData.setData(sql, trans);
                                    extData.commitTransaction(trans);

                                    // Get the client's email
                                    string sellSql = "SELECT  "
                                        + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                                        + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                                    DataTable clientInfo = extData.getData(sellSql);
                                    if (clientInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for client info returned null.");
                                    }
                                    string clientEmail = clientInfo.Rows[0]["email"].ToString();

                                    // Calculate invoice values
                                    DataTable stockInfo = extFunction.getSecuritiesByCode("stock", varBondTrustCode);
                                    if (stockInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for stock info returned null.");
                                    }

                                    float stockPrice = (float)stockInfo.Rows[0]["close"];
                                    float totalDollarAmount = stockPrice * float.Parse(varBondTrustSharesAmount);
                                    float clientBalance = (float)clientInfo.Rows[0]["balance"];
                                    float stockFee = clientBalance < 500000 ? (0.05f * totalDollarAmount) : (0.03f * totalDollarAmount);

                                    // Calculate transaction invoice values
                                    string body = "Hello, you have recently placed an order through the HKeInvest System.\n\n" +
                                        "Reference Number: " + result + "\n" +
                                        "Account Number: " + accountNumber + "\n" +
                                        "Order Type: Buy Unit Trust " + OrderType.SelectedValue + "\n" +
                                        "Date Submitted: " + DateTime.Now.ToString() + "\n" +
                                        "Shares Bought: " + varBondTrustSharesAmount + "\n" +
                                        "Total Executed Dollar Amount: " + totalDollarAmount.ToString() + "\n" +
                                        "Fee Charged: " + stockFee.ToString() + "\n\n" +
                                        "Transactions: \n";

                                    DataTable transactions = extFunction.getOrderTransaction(result);
                                    for (int i = 0; i < transactions.Rows.Count; i++)
                                    {
                                        body += "Transaction: " + (i + 1).ToString() + "/" + transactions.Rows.Count.ToString() + "\n" +
                                            "\tTransaction Number: " + transactions.Rows[i]["transactionNumber"].ToString() + "\n" +
                                            "\tDate Executed: " + transactions.Rows[i]["executeDate"].ToString() + "\n" +
                                            "\tQuantity of Shares Executed: " + transactions.Rows[i]["executeShares"].ToString() + "\n" +
                                            "\tPrice per Share: " + transactions.Rows[i]["executePrice"].ToString() + "\n";
                                    }

                                    // Email invoice
                                    intFunction.sendInvoiceEmail(clientEmail, "Stock Sell Order Invoice", body);
                                }
                            }
                        }

                    }
                    else if (TransactionType.SelectedValue.Equals("Sell"))
                    {
                        string varBondTrustShares = BondTrustSharesSelling.Text.ToString();

                        

                            if (SecurityType.SelectedValue.Equals("Bond"))
                            {
                                var validSecurity = extFunction.getSecuritiesByCode("bond", varBondTrustCode);

                            string validSharesAmount = bondSharesAmountIsValid("bond", varBondTrustCode, varBondTrustShares, "sell");
                                if (validSecurity == null || validSharesAmount != "")
                                {
                                    //Buy order was not succesfully submitted
                                    InvalidBondTrustCode.Text = "The code given does not exist";
                                }
                                else
                                {
                                    string result = extFunction.submitBondSellOrder(varBondTrustCode, varBondTrustShares);

                                    if (result != null)
                                    {

                                        //Yes. This is a redundant execution
                                        name = extFunction.getSecuritiesByCode("bond", varBondTrustCode).Rows[0]["name"].ToString().Trim();

                                        string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [accountNumber], [name]) VALUES ('" +
                                        result + "', '" +
                                        varTransactionType + "', '" +
                                        varSecurityType + "', '" +
                                        varBondTrustCode + "', '" +
                                        DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                        varBondTrustShares + "', '" +

                                        accountNumber + "', '" +
                                        name + "')" ;

                                        SqlTransaction trans = extData.beginTransaction();
                                        extData.setData(sql, trans);
                                        extData.commitTransaction(trans);

                                    // Get the client's email
                                    string sellSql = "SELECT  "
                                        + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                                        + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                                    DataTable clientInfo = extData.getData(sellSql);
                                    if (clientInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for client info returned null.");
                                    }
                                    string clientEmail = clientInfo.Rows[0]["email"].ToString();

                                    // Calculate invoice values
                                    DataTable stockInfo = extFunction.getSecuritiesByCode("stock", varBondTrustCode);
                                    if (stockInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for stock info returned null.");
                                    }

                                    float stockPrice = (float)stockInfo.Rows[0]["close"];
                                    float totalDollarAmount = stockPrice * float.Parse(varBondTrustShares);
                                    float clientBalance = (float)clientInfo.Rows[0]["balance"];
                                    float stockFee = clientBalance < 500000 ? 100 : 50;

                                    // Calculate transaction invoice values
                                    string body = "Hello, you have recently placed an order through the HKeInvest System.\n\n" +
                                        "Reference Number: " + result + "\n" +
                                        "Account Number: " + accountNumber + "\n" +
                                        "Order Type: Sell Bond " + OrderType.SelectedValue + "\n" +
                                        "Date Submitted: " + DateTime.Now.ToString() + "\n" +
                                        "Shares Bought: " + varBondTrustShares + "\n" +
                                        "Total Executed Dollar Amount: " + totalDollarAmount.ToString() + "\n" +
                                        "Fee Charged: " + stockFee.ToString() + "\n\n" +
                                        "Transactions: \n";

                                    DataTable transactions = extFunction.getOrderTransaction(result);
                                    for (int i = 0; i < transactions.Rows.Count; i++)
                                    {
                                        body += "Transaction: " + (i + 1).ToString() + "/" + transactions.Rows.Count.ToString() + "\n" +
                                            "\tTransaction Number: " + transactions.Rows[i]["transactionNumber"].ToString() + "\n" +
                                            "\tDate Executed: " + transactions.Rows[i]["executeDate"].ToString() + "\n" +
                                            "\tQuantity of Shares Executed: " + transactions.Rows[i]["executeShares"].ToString() + "\n" +
                                            "\tPrice per Share: " + transactions.Rows[i]["executePrice"].ToString() + "\n";
                                    }

                                    // Email invoice
                                    intFunction.sendInvoiceEmail(clientEmail, "Stock Sell Order Invoice", body);
                                }
                                }
                            
                        }
                        else if (SecurityType.SelectedValue.Equals("Unit Trust"))
                        {
                            var validSecurity = extFunction.getSecuritiesByCode("unit trust", varBondTrustCode);

                            string validSharesAmount = bondSharesAmountIsValid("unit trust", varBondTrustCode, varBondTrustShares, "sell");
                            if (validSecurity == null || validSharesAmount!="")
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                            else
                            {
                                string result = extFunction.submitUnitTrustSellOrder(varBondTrustCode, varBondTrustShares);

                                if (result != null)
                                {

                                    //Yes. This is a redundant execution
                                    name = extFunction.getSecuritiesByCode("unit trust", varBondTrustCode).Rows[0]["name"].ToString().Trim();

                                    string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [accountNumber], [name]) VALUES ('" +
                                        result + "', '" +
                                        varTransactionType + "', '" +
                                        varSecurityType + "', '" +
                                        varBondTrustCode + "', '" +
                                        DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                        varBondTrustShares + "', '" +

                                        accountNumber + "', '"+
                                        name+"')";

                                    SqlTransaction trans = extData.beginTransaction();
                                    extData.setData(sql, trans);
                                    extData.commitTransaction(trans);

                                    // Get the client's email
                                    string sellSql = "SELECT  "
                                        + "FROM Account AS a FULL JOIN Client as c ON a.accountNumber=c.accountNumber "
                                        + "WHERE userName='" + Context.User.Identity.GetUserName() + "' AND isPrimary='Y'";
                                    DataTable clientInfo = extData.getData(sellSql);
                                    if (clientInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for client info returned null.");
                                    }
                                    string clientEmail = clientInfo.Rows[0]["email"].ToString();

                                    // Calculate invoice values
                                    DataTable stockInfo = extFunction.getSecuritiesByCode("stock", varBondTrustCode);
                                    if (stockInfo == null)
                                    {
                                        throw new System.ArgumentNullException("Query for stock info returned null.");
                                    }

                                    float stockPrice = (float)stockInfo.Rows[0]["close"];
                                    float totalDollarAmount = stockPrice * float.Parse(varBondTrustShares);
                                    float clientBalance = (float)clientInfo.Rows[0]["balance"];
                                    float stockFee = clientBalance < 500000 ? 100 : 50;

                                    // Calculate transaction invoice values
                                    string body = "Hello, you have recently placed an order through the HKeInvest System.\n\n" +
                                        "Reference Number: " + result + "\n" +
                                        "Account Number: " + accountNumber + "\n" +
                                        "Order Type: Sell Unit Trust " + OrderType.SelectedValue + "\n" +
                                        "Date Submitted: " + DateTime.Now.ToString() + "\n" +
                                        "Shares Bought: " + varBondTrustShares + "\n" +
                                        "Total Executed Dollar Amount: " + totalDollarAmount.ToString() + "\n" +
                                        "Fee Charged: " + stockFee.ToString() + "\n\n" +
                                        "Transactions: \n";

                                    DataTable transactions = extFunction.getOrderTransaction(result);
                                    for (int i = 0; i < transactions.Rows.Count; i++)
                                    {
                                        body += "Transaction: " + (i + 1).ToString() + "/" + transactions.Rows.Count.ToString() + "\n" +
                                            "\tTransaction Number: " + transactions.Rows[i]["transactionNumber"].ToString() + "\n" +
                                            "\tDate Executed: " + transactions.Rows[i]["executeDate"].ToString() + "\n" +
                                            "\tQuantity of Shares Executed: " + transactions.Rows[i]["executeShares"].ToString() + "\n" +
                                            "\tPrice per Share: " + transactions.Rows[i]["executePrice"].ToString() + "\n";
                                    }

                                    // Email invoice
                                    intFunction.sendInvoiceEmail(clientEmail, "Stock Sell Order Invoice", body);
                                }
                            }
                        }
                    }
                }

            }
        }

        private string amountIsValid(string securityType, string code, string amount)
        {
            decimal number;
            if (!decimal.TryParse(amount, out number) || number <= 0)
            {
                return ("Invalid or missing dollar amount of " + securityType + " to buy.\nValue is '" + amount + "'.");
            }

            //SQL query to get quantity of bonds or unit trusts owned
            //Querying over the security holding's details

            string accountNumber = getAccountNumber();

            string sql = "SELECT shares FROM SecurityHolding WHERE accountNumber = '" +
                accountNumber + "' AND type = '" +
                securityType + "' AND code = '" +
                code + "'";
            DataTable qty = extData.getData(sql);

            //Bonds or Unit trusts only

            if (qty.Rows == null || qty.Rows.Count == 0)
            {

            }
            else
            {
                int availSecurities = Convert.ToInt32(qty.Rows[0]["shares"]);

                if (availSecurities > number)
                {
                    return "Not enough securites to sell";
                }
            }
           

            return "";
        }

        /**
        For Checking selling of bonds
        */
        private string bondSharesAmountIsValid(string securityType, string code, string shares, string typeOfTransaction)
        {
            decimal numShares;

            decimal.TryParse(shares,out numShares);

            if (numShares <= 0)
            {
                return ("Invalid or missing number of " + " shares to buy.\nValue is '" + shares + "'.");

            }
            else
            {
                if (typeOfTransaction == "sell")
                {
                    if (code != "")
                    {
                        decimal availShares = numOfSharesAbleToSell(securityType, code);
                        if (numShares > availShares)
                        {
                            return ("Shares to Sell is not valid. There are not that many shares to sell.\nValue is '" + shares + "'.");
                        }
                    }
                }
            }
            return "";
        }

        public string getAccountNumber()
        {
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" +
               HttpContext.Current.User.Identity.GetUserName() + "'";

            DataTable temp = extData.getData(sql);

            //ERROR: no error catching for not having the account number

            return temp.Rows[0]["accountNumber"].ToString();

        }

        //Calculate number of shares that can be sold
        public int numOfSharesAbleToSell(string type, string code)
        {
            string accountNumber = getAccountNumber();

            string sql = "SELECT shares FROM SecurityHolding WHERE accountNumber = '" +
                accountNumber + "' AND type = '" +
                type + "' AND code = '" +
                code + "'";

            DataTable temp = extData.getData(sql);

            if (temp == null || temp.Rows.Count == 0)
            {
                return -1;
            }


            int sharesHeld = Int32.Parse(temp.Rows[0].ToString());

            //References order history to check if there are pending orders
            if (type.Equals("stock"))
            {
                //Get all current processed sell statements for Stocks
                sql = "SELECT orderReference FROM OrderHistory WHERE accountNumber = '" +
                    accountNumber + "'";

                temp = extData.getData(sql);

                if (temp == null || temp.Rows.Count == 0)
                {
                    return sharesHeld;
                }
                else
                {
                    foreach (DataRow row in temp.Rows)
                    {
                        //Some functionality to get order status 
                        if (row["buyOrSell"].ToString().Equals("sell") && extFunction.getOrderStatus(row["orderReference"].ToString()).Equals("pending"))
                        {
                            //Subtract number of pending shares for the stock. When a stock is processed, the order number and qty is stored in the DB
                            sharesHeld -= Int32.Parse(row["shares"].ToString());
                        }
                    }
                }
            }

            return sharesHeld;

        }

        /**
        For checking multiple of 100 for buying stock
        */
        private string stockSharesAmountIsValid(string shares, string typeOfTransaction, string stockCode)
        {
            decimal numShares;
            decimal.TryParse(shares, out numShares);
            if (numShares <= 0)
            {
                return "Shares is not a positive number";
            }
            else {

                if (stockCode == "")
                {
                    return "Invalid stock code";
                }

                else {
                    if (typeOfTransaction == "Sell")
                    {
                        //Passing in empty stock code

                        decimal availShares = numOfSharesAbleToSell("stock", stockCode);
                        if (numShares > availShares)
                        {
                            return ("Shares to Sell is not valid. There are not that many shares to sell.\nValue is '" + shares + "'.");
                        }

                    }
                    else
                    {
                        if ((numShares % 100) != 0)
                        {
                            return ("Shares to buy is not a multiple of 100. \nValue is '" + shares + "'.");
                        }
                    }
                }
            }
            return "";

        }

        private string orderTypeIsValid(string buyOrSell, string orderType, string expiryDay, string allOrNone, string limitPrice, string stopPrice)
        {
            int intNumber;
            decimal decLimitPrice = 0;
            decimal decStopPrice = 0;

            // Check if order type is valid.
            if (!(orderType == "market" || orderType == "limit" || orderType == "stop" || orderType == "stop limit"))
            {
                return "Invalid or missing stock order type.\nValue is '" + orderType + "'.";

            }

            // Check if expiry day is valid.
            if (!int.TryParse(expiryDay, out intNumber) || intNumber < 1 || intNumber > 7)
            {
                return ("Invalid or missing expiry day.\nValue is '" + expiryDay + "'.");

            }

            // Check if all or none is valid.
            if (!(allOrNone.ToUpper() == "Y" || allOrNone.ToUpper() == "N"))
            {
                return ("Invalid or missing all or none.\nValue is '" + allOrNone + "'.");

            }

            // Check if limit price is valid.
            if (orderType == "limit" || orderType == "stop limit")
            {
                if (!decimal.TryParse(limitPrice, out decLimitPrice) || decLimitPrice <= 0)
                {
                    return ("Invalid or missing limit price.\nValue is '" + limitPrice + "'.");

                }
            }

            // Check if stop price is valid.
            if (orderType == "stop" || orderType == "stop limit")
            {
                if (!decimal.TryParse(stopPrice, out decStopPrice) || decStopPrice <= 0)
                {
                    return ("Invalid or missing stop price.\nValue is '" + stopPrice + "'.");

                }

            }

            // Check if stop and limit prices are in correct relationship to each other.
            if (orderType == "stop limit")
            {
                if (buyOrSell == "buy")
                {
                    if (decStopPrice > decLimitPrice)
                    {
                        return ("Stock buy order:\nstop price must be <= limit price.");

                    }
                }
                else // Sell order.
                {
                    if (decStopPrice < decLimitPrice)
                    {
                        return ("Stock sell order:\n stop price must be >= limit price.");

                    }
                }
            }
            return "";
        }
    }
}