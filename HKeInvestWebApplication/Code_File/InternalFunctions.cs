﻿using HKeInvestWebApplication.Code_File;
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


namespace HKeInvestWebApplication.Code_File
{
    public class InternalFunctions
    {

        ExternalFunctions extFunction = new ExternalFunctions();
        HKeInvestData extData = new HKeInvestData();

       

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
            string sql = "SELECT referenceNumber, accountNumber, buyOrSell FROM OrderHistory WHERE status = 'pending'";
            //string accountNumber = getAccountNumber();
            DataTable orderNums = extData.getData(sql);
            if(orderNums != null && orderNums.Rows != null) {
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

                        //Depends on whether function was buy or sell for determining 




                        string buyOrSell = row["buyOrSell"].ToString();

                        sql = "SELECT balance FROM Account WHERE accountNumber = '" + accountNumber + "'";

                        decimal balance = Decimal.Parse(extData.getData(sql).Rows[0]["balance"].ToString());

                        //Get transaction information from external table
                        decimal costOfOrder = 0m;

                        DataTable transactions = extFunction.getOrderTransaction(referenceNumber);
                        if (transactions != null && transactions.Rows != null)
                        {
                            foreach (DataRow tRow in transactions.Rows)
                            {
                                costOfOrder += decimal.Parse(row["executeShares"].ToString()) * decimal.Parse(row["executePrice"].ToString());
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

                    }
                }
            }

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
                    if (transactions != null && transactions.Rows != null) {
                    foreach (DataRow tRow in transactions.Rows)
                    {
                        string tID = tRow["transactionNumber"].ToString();

                        //Check if transaction number already inserted

                        //Check to see that nothing is returned from the prior functiion
                        SqlTransaction trans = extData.beginTransaction();
                        sql = "SELECT transactionNumber FROM Transactions WHERE transactionNumber = '" + tID + "'";
                        DataTable localTrans = extData.getData(sql);
                        if (localTrans == null || localTrans.Rows.Count == 0)
                        {
                            sql = "INSERT INTO Transactions (transactionNumber, executeDate, executeShares, executePrice, referenceNumber) VALUES ('" +
                            tID + "', '" +
                            tRow["executeDate"].ToString() + "', '" +
                            tRow["executeShares"] + "', '" +
                            tRow["executePrice"] + "', '" +
                            referenceNumber + ")";
                            extData.setData(sql, trans);
                        }
                        extData.commitTransaction(trans);

                        //Not sure if this will commit all transactions
                    }
                }
            }
            
        }
    }
}