using HKeInvestWebApplication.ExternalSystems.Code_File;
using System;
using System.Collections.Generic;
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
                if(SecurityType.SelectedValue == "Bond/Unit Trust")
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
            if(SecurityType.SelectedValue == "Stock" && TransactionType.SelectedValue == "Buy")
            {
                
                InvalidStockSharesQuantity.Text = sharesAmountIsValid(StockSharesQuantity.Text,TransactionType.Text);
            }
            else if(SecurityType.SelectedValue == "Stock" && TransactionType.SelectedValue == "Sell")
            {
                InvalidStockSharesQuantity.Text = sharesAmountIsValid(StockSharesQuantity.Text, TransactionType.Text);
            }
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (TransactionType.SelectedValue == "Sell")
            {
                //int shares;
                //Int32.TryParse(BondTrustSharesSelling.Text, out shares);
                //if (shares <= 0)
                //{
                //    InvalidBondTrustSharesSelling.Text = "Please a enter a postivie number of shares to sell";
                //}
                var type = SecurityType.SelectedValue == "Bond" ? "bond" : "unit trust"; 
                InvalidBondTrustSharesSelling.Text = sharesIsValid(type,BondTrustSharesSelling.Text);
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
                InvalidBondTrustSharesQuantity.Text = amountIsValid(type, BondTrustSharesQuantity.Text);
            }
        }

        ExternalFunctions extFunction = new ExternalFunctions();

        protected void ExecuteTransactionClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (SecurityType.SelectedValue.Equals("Stock"))
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

                    var validSecurity = extFunction.getSecuritiesByCode("stock", varStockCode);
                    if (validSecurity == null)
                    {
                        //Sell order was not succesfully submitted
                        InvalidStockCode.Text = "The code given does not exist";
                    }
                    else if (TransactionType.SelectedValue.Equals("Buy"))
                    {
                        string highPrice = "";
                        
                        InvalidStockSharesQuantity.Text = sharesAmountIsValid(varShares, TransactionType.Text);
                        if(InvalidStockSharesQuantity.Text != "")
                        {
                            return;
                        }
                        
                        //Limit price = low price here
                        extFunction.submitStockBuyOrder(varStockCode, varShares, varOrderType, varExpiryDate, varAllOrNone, varLimitPrice, varStopPrice);
                    }
                    else if (TransactionType.SelectedValue.Equals("Sell"))
                    {
                        string lowPrice = "";
                        extFunction.submitStockSellOrder(varStockCode, varShares, varOrderType, varExpiryDate, varAllOrNone, varLimitPrice, varStopPrice);
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
                                extFunction.submitBondBuyOrder(varBondTrustCode, varBondTrustSharesAmount);
                            }
                        }
                        else if(SecurityType.SelectedValue.Equals("Unit Trust"))
                        {
                            
                            var validSecurity = extFunction.getSecuritiesByCode("unit trust", varBondTrustCode);
                            if (validSecurity == null)
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                            else
                            {
                                extFunction.submitUnitTrustBuyOrder(varBondTrustCode, varBondTrustSharesAmount);
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
                                    extFunction.submitBondSellOrder(varBondTrustCode, varBondTrustShares);
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
                                extFunction.submitUnitTrustSellOrder(varBondTrustCode, varBondTrustShares);
                            }
                        }
                    }
                }
               
            }
        }

        private string amountIsValid(string securityType, string amount)
        {
            decimal number;
            if (!decimal.TryParse(amount, out number) || number <= 0)
            {
                return("Invalid or missing dollar amount of " + securityType + " to buy.\nValue is '" + amount + "'.");
               
            }
            return "";
        }

        /**
        For Checking selling of bonds
        */
        private string sharesIsValid(string securityType, string shares)
        {
            decimal number;
            if (!decimal.TryParse(shares, out number) || number <= 0)
            {
                return("Invalid or missing number of " + securityType + " shares to sell.\nValue is '" + shares + "'.");
               
            }
            return "";
        }

        /**
        For checking multiple of 100 for buying stock
        */
        private string sharesAmountIsValid(string shares, string typeOfTransaction)
        {
            float number;
            float.TryParse(shares,out number);
            if (number == 0)
            {
                return "Shares is an invalid number";
            }
            if(typeOfTransaction == "Sell" && number <= 0)
            {
                return("Shares to Sell is not a positive number.\nValue is '" + shares + "'.");
            }
            else if ((number % 100) != 0)
            {
                return("Shares to buy is not a multiple of 100 or is not a positive number.\nValue is '" + shares + "'.");
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
                return("Invalid or missing expiry day.\nValue is '" + expiryDay + "'.");
               
            }

            // Check if all or none is valid.
            if (!(allOrNone.ToUpper() == "Y" || allOrNone.ToUpper() == "N"))
            {
                return("Invalid or missing all or none.\nValue is '" + allOrNone + "'.");
               
            }

            // Check if limit price is valid.
            if (orderType == "limit" || orderType == "stop limit")
            {
                if (!decimal.TryParse(limitPrice, out decLimitPrice) || decLimitPrice <= 0)
                {
                    return("Invalid or missing limit price.\nValue is '" + limitPrice + "'.");
                    
                }
            }

            // Check if stop price is valid.
            if (orderType == "stop" || orderType == "stop limit")
            {
                if (!decimal.TryParse(stopPrice, out decStopPrice) || decStopPrice <= 0)
                {
                    return("Invalid or missing stop price.\nValue is '" + stopPrice + "'.");
                   
                }

            }

            // Check if stop and limit prices are in correct relationship to each other.
            if (orderType == "stop limit")
            {
                if (buyOrSell == "buy")
                {
                    if (decStopPrice > decLimitPrice)
                    {
                        return("Stock buy order:\nstop price must be <= limit price.");
                        
                    }
                }
                else // Sell order.
                {
                    if (decStopPrice < decLimitPrice)
                    {
                        return("Stock sell order:\n stop price must be >= limit price.");
                        
                    }
                }
            }
            return "";
        }
    }
}