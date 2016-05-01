using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

            //if(SecurityType.SelectedValue == "Stock" && TransactionType.SelectedValue == "Buy")
            //{

            //    InvalidStockSharesQuantity.Text = sharesAmountIsValid(StockSharesQuantity.Text,TransactionType.Text, StockCode.Text.Trim());
            //}
            //else if(SecurityType.SelectedValue == "Stock" && TransactionType.SelectedValue == "Sell")
            //{
            //    InvalidStockSharesQuantity.Text = sharesAmountIsValid(StockSharesQuantity.Text, TransactionType.Text);
            //}
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
                InvalidBondTrustSharesSelling.Text = bondSharesAmountIsValid(type, BondTrustCode.Text.Trim(), BondTrustSharesSelling.Text);
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TransactionType.SelectedValue == "Buy")
            {
                //int shares;
                //Int32.TryParse(BondTrustSharesQuantity.Text, out shares);
                //if (shares <= 0)
                //{
                //    InvalidBondTrustSharesQuantity.Text = "Please a enter a positive dollar amount";
                //}
                var type = SecurityType.SelectedValue == "Bond" ? "bond" : "unit trust";
                //TODO Check the actual availability
                InvalidBondTrustSharesQuantity.Text = amountIsValid(type, BondTrustCode.Text.Trim(), BondTrustSharesQuantity.Text);
            }
        }

        ExternalFunctions extFunction = new ExternalFunctions();
        HKeInvestData extData = new HKeInvestData();

        //Calculate number of shares that can be sold
        private int numOfSharesAbleToSell(string type, string code)
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

        //Calculate the fees for the order (will calculate for the set of transactions)     
        public decimal calculateFees(string referenceNumber)
        {

            decimal fee = -1m;
            //ONLY QUERYING LOCAL DATABASE
            //Query to get order type
            //Query to get order buyOrSell status
            string sql = "SELECT * FROM OrderHistory WHERE referenceNumber = '" +
                referenceNumber + "'";
            DataTable orderH = extData.getData(sql);
            string type = orderH.Rows[0]["securityType"].ToString();
            string buyOrSell = orderH.Rows[0]["buyOrSell"].ToString();


            //Query to get completed transactions 
            //TODO: throw error if query not connected to external database
            DataTable extTransaction = extFunction.getOrderTransaction(referenceNumber);

            decimal assets = accountAssets();

            if (type.Equals("stock"))
            {
                //Query to get the order type (from local history maybe)
                string orderType = orderH.Rows[0]["stockOrderType"].ToString();

                //variable for checking if the assessed fee is greater than the minimmum fee
                decimal minFeeCheck = 0m;

                if (assets < 1000000m)
                {
                    if (orderType.Equals("market"))
                    {
                        minFeeCheck = calculateFeesAtRate(extTransaction, .004m);
                    }
                    else if (orderType.Equals("stop") || orderType.Equals("limit"))
                    {
                        minFeeCheck = calculateFeesAtRate(extTransaction, .006m);
                    }
                    else
                    {
                        minFeeCheck = calculateFeesAtRate(extTransaction, .008m);
                    }

                    fee = minFeeCheck > 150m ? minFeeCheck : 150m;
                }
                else
                {
                    if (orderType.Equals("market"))
                    {
                        minFeeCheck = calculateFeesAtRate(extTransaction, .002m);
                    }
                    else if (orderType.Equals("stop") || orderType.Equals("limit"))
                    {
                        minFeeCheck = calculateFeesAtRate(extTransaction, .004m);
                    }
                    else
                    {
                        minFeeCheck = calculateFeesAtRate(extTransaction, .006m);
                    }

                }
            }
            else
            {
                if (assets < 500000m)
                {
                    if (buyOrSell.Equals("buy"))
                    {
                        //Query execute shares and execute price from transactions
                        fee = calculateFeesAtRate(extTransaction, .05m);
                    }
                    else
                    {
                        fee = 100m;
                    }
                }
                else
                {
                    if (buyOrSell.Equals("buy"))
                    {
                        fee = calculateFeesAtRate(extTransaction, .03m);
                    }
                    else
                    {
                        fee = 500m;
                    }
                }
            }

            return fee;
        }

        //Helper fucntion to calculate fees (HARDCODED FEES)
        private decimal calculateFeesAtRate(DataTable transactions, decimal percentage)
        {
            decimal feeSum = 0;
            foreach (DataRow row in transactions.Rows)
            {
                feeSum += Decimal.Parse(row["executeShares"].ToString()) * Decimal.Parse(row["executePrice"].ToString());
            }

            return percentage * feeSum;
        }

        //Calculate current account assets UNTESTED
        public decimal accountAssets()
        {

            string accountNumber = getAccountNumber();

            //Getting value of the current account assets
            string sql = "SELECT balance FROM Account WHERE userName = '" +
                Context.User.Identity.GetUserName() + "'";
            DataTable temp = extData.getData(sql);
            //ERROR: Throw in try catch statement
            decimal balance = Decimal.Parse(temp.Rows[0].ToString());


            //Getting current value of the stocks
            sql = "SELECT type, code, shares, base FROM SecurityHolding WHERE accountNumber = '" +
                accountNumber + "'";
            DataTable securities = extData.getData(sql);

            decimal securitySum = 0;

            foreach (DataRow Row in securities.Rows)
            {
                //Assuming that the securities stored in the external database are returned in HKD

                //Iterate and get the value of the currency based on the type and code
                decimal currentPrice = extFunction.getSecuritiesPrice(Row["type"].ToString(), Row["code"].ToString());
                if (currentPrice == -1)
                {
                    //Throw some error;
                    break;
                }
                else
                {
                    //ASK: Is the current price always reflected in HKD
                    decimal priceTemp = currentPrice * Decimal.Parse(Row["shares"].ToString());
                    string bases = Row["base"].ToString();
                    //Check this convert currency function
                    securitySum += priceTemp * extFunction.getCurrencyRate(bases);
                }
            }
            //Should return net balance of the accounts
            return balance + securitySum;
        }

        //TODO set variable as a global variable, so sql not reexecuted every time
        private string getAccountNumber()
        {
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" +
               Context.User.Identity.GetUserName() + "'";

            DataTable temp = extData.getData(sql);

            //ERROR: no error catching for not having the account number

            return temp.Rows[0]["accountNumber"].ToString();

        }


        //UNTESTED
        //Also updates balance in function
        //To be called through threading
        public void updateLocalOrderStatus()
        {
            string sql = "SELECT referenceNumber FROM OrderHistory WHERE status = 'pending'";
            string accountNumber = getAccountNumber();
            DataTable orderNums = extData.getData(sql);
            SqlTransaction trans = extData.beginTransaction();
            foreach (DataRow row in orderNums.Rows)
            {
                string referenceNumber = row["referenceNumber"].ToString().Trim();
                string orderStatus = extFunction.getOrderStatus(referenceNumber);
                if(orderStatus != "pending")
                {

                    //Including calculate fees function

                    decimal fees = calculateFees(referenceNumber);

                    sql = "UPDATE OrderHistory SET status ='" + orderStatus +"', feesPaid = '" + fees.ToString() +"' WHERE referenceNumber = '" + referenceNumber + "'";
                    extData.setData(sql, trans);

                    //Now that the order has been completed fees can be calculated and applied to the balance in account and the feespaid in orderhistory

                    //Depends on whether function was buy or sell for determining 

                    string buyOrSell = row["buyOrSell"].ToString();

                    sql = "SELECT balance FROM Account WHERE accountNumber = '" + accountNumber + "'";

                    decimal balance = Decimal.Parse(extData.getData(sql).Rows[0]["balance"].ToString());
                    decimal costOfOrder = decimal.Parse(row["executeShares"].ToString()) * decimal.Parse(row["executePrice"].ToString());

                    if (buyOrSell.Equals("buy"))
                    {
                        balance -= fees;
                        balance -= costOfOrder;
                    }
                    else
                    {
                        balance -= fees;
                        balance += costOfOrder;
                    }
                    sql = "UPDATE Account SET balance = '"+balance+"' WHERE accountNumber = '" + accountNumber + "'";


                }
               
            }
            extData.commitTransaction(trans);
        }

        //Inefficent as hell
        //UNTESTED
        //Running on thread for backups
        public void updateLocalTransaction()
        {
            string sql = "SELECT referenceNumber FROM OrderHistory";
            DataTable orderNums = extData.getData(sql);
            SqlTransaction trans = extData.beginTransaction();
            foreach (DataRow row in orderNums.Rows)
            {
                string referenceNumber = row["referenceNumber"].ToString().Trim();
                DataTable transactions = extFunction.getOrderTransaction(referenceNumber);
                foreach(DataRow tRow in transactions.Rows)
                {
                    string tID = tRow["transactionNumber"].ToString();

                    //Check if transaction number already inserted

                    //Check to see that nothing is returned from the prior functiion
                    sql = "SELECT transactionNumber FROM Transactions WHERE transactionNumber = '" + tID + "'";
                    DataTable localTrans = extData.getData(sql);
                    if(localTrans == null || localTrans.Rows.Count == 0)
                    {
                        sql = "INSERT INTO Transactions (transactionNumber, executeDate, executeShares, executePrice, referenceNumber) VALUES ('" +
                        tID + "', '" +
                        tRow["executeDate"].ToString() + "', '" +
                        tRow["executeShares"] + "', '" +
                        tRow["executePrice"] + "', '" +
                        referenceNumber + ")";
                        extData.setData(sql, trans);
                    }

                    //Not sure if this will commit all transactions
                }

            }
            extData.commitTransaction(trans);
        }

        protected void ExecuteOrderClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string varSecurityType = SecurityType.SelectedValue.ToString().Trim();
                string varTransactionType = TransactionType.SelectedValue.ToString().Trim();
                string accountNumber = getAccountNumber();

                if (varSecurityType.Equals("Stock"))
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
                    else if (varTransactionType.Equals("Buy"))
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
                            string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [stockOrderType], [expiryDay], [allOrNone], [limitPrice], [stopPrice], [accountNumber]) VALUES ('" +
                                result + "', '" +
                                varTransactionType + "', '" +
                                varSecurityType + "', '" +
                                varStockCode + "', '" +
                                DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                varShares + "', '" +
                                varOrderType + "', '" +
                                varExpiryDate + "', '" +
                                varAllOrNone + "', '" +
                                varLimitPrice + "', '" +
                                varStopPrice + "', '" +
                                accountNumber + "')";

                            SqlTransaction trans = extData.beginTransaction();
                            extData.setData(sql, trans);
                            extData.commitTransaction(trans);
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
                            string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [stockOrderType], [expiryDay], [allOrNone], [limitPrice], [stopPrice], [accountNumber]) VALUES ('" +
                                 result + "', '" +
                                 varTransactionType + "', '" +
                                 varSecurityType + "', '" +
                                 varStockCode + "', '" +
                                 DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                 varShares + "', '" +
                                 varOrderType + "', '" +
                                 varExpiryDate + "', '" +
                                 varAllOrNone + "', '" +
                                 varLimitPrice + "', '" +
                                 varStopPrice + "', '" +
                                 accountNumber + "')";

                            SqlTransaction trans = extData.beginTransaction();
                            extData.setData(sql, trans);
                            extData.commitTransaction(trans);
                            //Return URL
                        }
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
                                    string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [amount], [accountNumber]) VALUES ('" +
                                       result + "', '" +
                                       varTransactionType + "', '" +
                                       varSecurityType + "', '" +
                                       varBondTrustCode + "', '" +
                                       DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                       varBondTrustSharesAmount + "', '" +

                                       accountNumber + "')";

                                    SqlTransaction trans = extData.beginTransaction();
                                    extData.setData(sql, trans);
                                    extData.commitTransaction(trans);
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
                                    string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [amount], [accountNumber]) VALUES ('" +
                                      result + "', '" +
                                      varTransactionType + "', '" +
                                      varSecurityType + "', '" +
                                      varBondTrustCode + "', '" +
                                      DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                      varBondTrustSharesAmount + "', '" +

                                      accountNumber + "')";

                                    SqlTransaction trans = extData.beginTransaction();
                                    extData.setData(sql, trans);
                                    extData.commitTransaction(trans);
                                }
                            }
                        }

                    }
                    else if (TransactionType.SelectedValue.Equals("Sell"))
                    {
                        string varBondTrustShares = BondTrustSharesSelling.Text.ToString();
                        if (SecurityType.SelectedValue.Equals("Bond"))
                        {
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
                                    string result = extFunction.submitBondSellOrder(varBondTrustCode, varBondTrustShares);

                                    if (result != null)
                                    {
                                        string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [accountNumber]) VALUES ('" +
                                        result + "', '" +
                                        varTransactionType + "', '" +
                                        varSecurityType + "', '" +
                                        varBondTrustCode + "', '" +
                                        DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                        varBondTrustShares + "', '" +

                                        accountNumber + "')";

                                        SqlTransaction trans = extData.beginTransaction();
                                        extData.setData(sql, trans);
                                        extData.commitTransaction(trans);
                                    }
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
                                string result = extFunction.submitUnitTrustSellOrder(varBondTrustCode, varBondTrustShares);

                                if (result != null)
                                {
                                    string sql = "INSERT INTO OrderHistory ([referenceNumber], [buyOrSell], [securityType], [securityCode], [dateSubmitted], [shares], [accountNumber]) VALUES ('" +
                                        result + "', '" +
                                        varTransactionType + "', '" +
                                        varSecurityType + "', '" +
                                        varBondTrustCode + "', '" +
                                        DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt") + "', '" +
                                        varBondTrustShares + "', '" +

                                        accountNumber + "')";

                                    SqlTransaction trans = extData.beginTransaction();
                                    extData.setData(sql, trans);
                                    extData.commitTransaction(trans);
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
        private string bondSharesAmountIsValid(string securityType, string code, string shares)
        {
            decimal numShares;

            decimal.TryParse(shares,out numShares);

            if (numShares <= 0)
            {
                return ("Invalid or missing number of " + " shares to buy.\nValue is '" + shares + "'.");

            }
            return "";
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
                return "Shares is not a positive number number";
            }
            else {
                if (typeOfTransaction == "Sell")
                {
                    //Passing in empty stock code
                    if (stockCode != "")
                    {
                        decimal availShares = numOfSharesAbleToSell("stock", stockCode);
                        if (numShares > availShares)
                        {
                            return ("Shares to Sell is not valid. There are not that many shares to sell.\nValue is '" + shares + "'.");
                        }
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