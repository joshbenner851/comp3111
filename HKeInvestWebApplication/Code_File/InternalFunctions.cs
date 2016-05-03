using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace HKeInvestWebApplication.Code_File
{
    public class InternalFunctions
    {

        ExternalFunctions extFunction = new ExternalFunctions();
        HKeInvestData extData = new HKeInvestData();
        private static LinkedList<SecurityAlert> alertsLL = new LinkedList<SecurityAlert>();



        //Calculate the fees for the order (will calculate for the set of transactions)     
        public decimal calculateFees(string referenceNumber, string accountNumber)
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

            decimal assets = accountAssets(accountNumber);

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
        public decimal accountAssets(string accountNumber)
        {

            //Getting value of the current account assets
            string sql = "SELECT balance FROM Account WHERE accountNumber = '" +
                accountNumber + "'";
            DataTable temp = extData.getData(sql);
            //ERROR: Throw in try catch statement
            decimal balance = Decimal.Parse(temp.Rows[0]["balance"].ToString());


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

        //UNTESTED
        //Also updates balance in function
        //To be called through threading
        public void updateLocalOrderStatus()
        {
            string sql = "SELECT referenceNumber, accountNumber, buyOrSell, name, securityType, securityCode, stockOrderType, dateSubmitted, feesPaid FROM OrderHistory WHERE status = 'pending'";
            //string accountNumber = getAccountNumber();
            DataTable orderNums = extData.getData(sql);
            if (orderNums != null && orderNums.Rows != null)
            {
                foreach (DataRow row in orderNums.Rows)
                {
                    string referenceNumber = row["referenceNumber"].ToString().Trim();
                    string orderStatus = extFunction.getOrderStatus(referenceNumber);
                    string accountNumber = row["accountNumber"].ToString().Trim();
                    if (orderStatus != "pending")
                    {

                        //Including calculate fees function

                        decimal fees = calculateFees(referenceNumber, accountNumber);
                        SqlTransaction trans = extData.beginTransaction();
                        sql = "UPDATE OrderHistory SET status ='" + orderStatus + "', feesPaid = '" + fees.ToString() + "' WHERE referenceNumber = '" + referenceNumber + "'";
                        extData.setData(sql, trans);
                        extData.commitTransaction(trans);
                        //Now that the order has been completed fees can be calculated and applied to the balance in account and the feespaid in orderhistory

                        string buyOrSell = row["buyOrSell"].ToString();

                        sql = "SELECT balance FROM Account WHERE accountNumber = '" + accountNumber + "'";

                        decimal balance = Decimal.Parse(extData.getData(sql).Rows[0]["balance"].ToString());

                        //Get transaction information from external table
                        decimal costOfOrder = 0m;

                        decimal totalSharesExecuted = 0m;

                        DataTable transactions = extFunction.getOrderTransaction(referenceNumber);
                        if (transactions != null && transactions.Rows != null)
                        {
                            foreach (DataRow tRow in transactions.Rows)
                            {
                                costOfOrder += decimal.Parse(tRow["executeShares"].ToString()) * decimal.Parse(tRow["executePrice"].ToString());
                                totalSharesExecuted += decimal.Parse(tRow["executeShares"].ToString());
                            }
                        }

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

                        trans = extData.beginTransaction();
                        sql = "UPDATE Account SET balance = '" + balance + "' WHERE accountNumber = '" + accountNumber + "'";
                        extData.commitTransaction(trans);



                        string varSecurityType = row["securityType"].ToString();
                        string varSecurityCode = row["securityCode"].ToString();

                        //Now that the order has been completed. Send email

                        //orderReference, accountNumber, buyorsell, securitycode, securityname, (stock order type), dateSubmited, totalnumSharesBoughtt, executedDollarAmount, feeCharged, (FOREACH transaction) transactionNumber, date executed, quantity of shares, price per share
                        string address = "";//get from sql statement

                        sql = "SELECT email FROM Client WHERE accountNumber = '" + accountNumber + "' AND isPrimary = 'Y'";
                        DataTable tempForAddress = extData.getData(sql);
                        if (tempForAddress != null && tempForAddress.Rows.Count > 0)
                        {
                            address = tempForAddress.Rows[0]["email"].ToString().Trim();
                        }

                        string subject = "Order Number: " + referenceNumber + " has been " + orderStatus;
                        string body = "Order Reference Number: " + referenceNumber + "\n" +
                            "Account Number: " + accountNumber + "\n" +
                            "Buy or Sell Order: " + buyOrSell + "\n" +
                            "Security Code: " + varSecurityCode + "\n" +
                            "Security Name: " + row["name"].ToString() + "\n";
                        if (varSecurityType.Equals("stock"))
                        {
                            body += "Stock Order Type: " + row["stockOrderType"].ToString() + "\n";
                        }
                        body += "Date Submitted: " + row["dateSubmitted"].ToString() + "\n" +
                            "Total Shares Executed: " + totalSharesExecuted.ToString() + "\n" +
                            "Total Executed Dollar Amount: " + costOfOrder + "\n";

                        if (transactions != null && transactions.Rows != null)
                        {
                            foreach (DataRow tRow in transactions.Rows)
                            {
                                body += "Transaction number: " + tRow["transactionNumber"].ToString() + "\n" +
                                    "Execution Date: " + tRow["executeDate"].ToString() + "\n" +
                                    "Quantity of Shares: " + tRow["executeShares"].ToString() + "\n" +
                                    "Price per share: " + tRow["executePrice"].ToString() + "\n";
                            }
                        }
                        body += "\n Sincerely yours, the HKeInvestment Team. Contact our hotline if there is any issues with your invoice.";

                        // Check that the email is not missing a sender's address, a subject, or a body
                        if (address != null && subject != null && body !=null && address!= "")
                        {
                            sendInvoiceEmail(address, subject, body);
                        }
                        else
                        {
                            // TODO: Handle this
                        }


                        //Get base of transaction
                        //Should probably store in the local database as well
                        string currencyBase = "";
                        DataTable currencyBaseTable = extFunction.getSecuritiesByCode(varSecurityType, varSecurityCode);
                        if(currencyBaseTable!=null && currencyBaseTable.Rows.Count > 0)
                        {
                            //All stocks lack a currency base
                            if (varSecurityType.Equals("stock"))
                            {
                               
                                currencyBase = "HKD";
                            }
                            else
                            {
                                currencyBase = currencyBaseTable.Rows[0]["base"].ToString();
                            }
                        }

                        /////////////////////////UPDATE SECURITY HOLDING DETAILS///////////////////////////////////////

                        sql = "SELECT * FROM SecurityHolding WHERE accountNumber = '" + accountNumber + "' AND type = '" + varSecurityType + "' AND code = '" + varSecurityCode + "'";
                        DataTable curSecurityHolding = extData.getData(sql);

                        //Checking the values of the secuityholding
                        if (curSecurityHolding == null || curSecurityHolding.Rows.Count == 0)
                        {
                            if (buyOrSell.Equals("buy"))
                            {
                                trans = extData.beginTransaction();
                                sql = "INSERT INTO SecurityHolding (accountNumber, type, code, name, shares, base) VALUES ('" +
                                    accountNumber + "', '" +
                                    varSecurityType + "', '" +
                                    varSecurityCode + "', '" +
                                    row["name"].ToString() + "', '" +
                                    totalSharesExecuted.ToString() + "', '" +
                                    currencyBase + "')";
                                extData.setData(sql, trans);
                                extData.commitTransaction(trans);
                            }
                            else
                            {
                                //THROW SOME ERROR
                            }
                        }
                        else
                        {

                            //current number of shares held
                            decimal curShares = decimal.Parse(curSecurityHolding.Rows[0]["shares"].ToString());

                            trans = extData.beginTransaction();
                            sql = "";
                            if (buyOrSell.Equals("buy"))
                            {
                                curShares += totalSharesExecuted;
                            }
                            else
                            {
                                curShares -= totalSharesExecuted;
                            }
                            if (curShares <= 0m)
                            {
                                sql = "DELETE FROM SecurityHolding WHERE accountNumber = '" + accountNumber + "' AND type = '" + varSecurityType + "' AND code = '" + varSecurityCode + "'"; 
                            }
                            else
                            {
                                sql = "UPDATE SecurityHolding SET shares ='" + curShares + "' WHERE accountNumber = '" + accountNumber + "' AND type = '" + varSecurityType + "' AND code = '" + varSecurityCode + "'";
                            }
                            extData.setData(sql, trans);
                            extData.commitTransaction(trans);
                        }

                       
                    }
                }
            }

        }

        public void sendInvoiceEmail(string address, string subject, string body)
        {
            //Email setup code

            MailMessage mail = new MailMessage();
            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

            mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest Alerts");
            mail.To.Add(address);
            mail.Subject = subject;
            mail.Body = body;

            emailServer.Send(mail);

        }

        //Inefficent as hell
        //UNTESTED
        //Running on thread for backups
        public void updateLocalTransaction()
        {
            string sql = "SELECT referenceNumber FROM OrderHistory";
            DataTable orderNums = extData.getData(sql);

            foreach (DataRow row in orderNums.Rows)
            {
                string referenceNumber = row["referenceNumber"].ToString().Trim();
                DataTable transactions = extFunction.getOrderTransaction(referenceNumber);
                if (transactions != null && transactions.Rows != null)
                {
                    foreach (DataRow tRow in transactions.Rows)
                    {
                        string tID = tRow["transactionNumber"].ToString();

                        //Check if transaction number already inserted

                        //Check to see that nothing is returned from the prior functiion

                        sql = "SELECT transactionNumber FROM Transactions WHERE transactionNumber = '" + tID + "'";
                        DataTable localTrans = extData.getData(sql);
                        SqlTransaction trans = extData.beginTransaction();
                        if (localTrans == null || localTrans.Rows.Count == 0)
                        {
                            sql = "INSERT INTO Transactions (transactionNumber, executeDate, executeShares, executePrice, referenceNumber) VALUES ('" +
                            tID + "', '" +
                            tRow["executeDate"].ToString() + "', '" +
                            tRow["executeShares"] + "', '" +
                            tRow["executePrice"] + "', '" +
                            referenceNumber + "')";
                            extData.setData(sql, trans);
                        }
                        extData.commitTransaction(trans);

                        //Not sure if this will commit all transactions
                    }
                }
            }

        }

		private class SecurityAlert
		{
			ExternalFunctions myExternalFunctions = new ExternalFunctions();

			private string securityType;
			private string securityCode;
			private string alertType;
			private float alertValue;
			private string email;
			// Use Utc time to keep consistent across time zones and when crossing time zones
			private DateTime lastTimeFired;

			public SecurityAlert(string newSecurityType, string newSecurityCode, string newAlertType, float newAlertValue, string newEmail)
			{
				this.securityType = newSecurityType;
				this.securityCode = newSecurityCode;
				this.alertType = newAlertType;
				this.alertValue = newAlertValue;
				this.email = newEmail;
			}

			public void checkForTrigger()
			{
				// "If the trigger has never been fired or it has been greater than 24 hours since the last time it has fired"
				// This might be able to be reduced to only the statement on the right side of the logical or
				if (lastTimeFired == DateTime.MinValue || lastTimeFired.AddDays(1) < DateTime.UtcNow)
				{
					if (alertType == "High")
					{
						// Get the current price of the security
						DataTable currentSecurity = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
						float price = float.Parse(currentSecurity.Rows[0]["price"].ToString());

						if (alertValue <= price)
						{
                            MailMessage mail = new MailMessage();
                            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

                            mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest Alerts");
                            mail.To.Add(email);
                            mail.Subject = "An Alert You've Set Has Been Triggered";
                            mail.Body = "Hello, an alert you've set on a security has been triggered.\n\n" +
                                "The Security in question is:\n" +
                                "Type - " + securityType + "; Code - " + securityCode + "\n" +
                                "The price of the security has reached or exceeded the specified value of: " + alertValue;

                            emailServer.Send(mail);

                            lastTimeFired = DateTime.UtcNow;
						}
					}
					else // Alert value is either "High" or "Low"
					{
						DataTable currentSecurity = myExternalFunctions.getSecuritiesByCode(securityType, securityCode);
						float price = float.Parse(currentSecurity.Rows[0]["price"].ToString());

						if (alertValue >= price)
						{
                            MailMessage mail = new MailMessage();
                            SmtpClient emailServer = new SmtpClient("smtp.cse.ust.hk");

                            mail.From = new MailAddress("comp3111_team106@cse.ust.hk", "HKeInvest Alerts");
                            mail.To.Add(email);
                            mail.Subject = "An Alert You've Set Has Been Triggered";
                            mail.Body = "Hello, an alert you've set on a security has been triggered.\n\n" +
                                "The Security in question is:\n" +
                                "Type - " + securityType + "; Code - " + securityCode + "\n" +
                                "The price of the security has reached or go\ne below the specified value of: " + alertValue;

                            emailServer.Send(mail);

                            lastTimeFired = DateTime.UtcNow;
						}
					}
				}
			}
		}

        public void createAlert(string securityType, string securityCode, string alertType, float alertValue, string email)
        {
            alertsLL.AddFirst(new SecurityAlert(securityType, securityCode, alertType, alertValue, email));
        }

        public void checkAlerts()
        {
            foreach (SecurityAlert alert in alertsLL)
            {
                alert.checkForTrigger();
            }
        }
    }
}
