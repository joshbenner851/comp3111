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
                int shares;
                Int32.TryParse(StockSharesQuantity.Text,out shares);
                if (shares <= 0)
                {

                    InvalidStockSharesQuantity.Text = "Please a enter a postive number of shares to buy";
                }
                else if (shares % 100 != 0)
                {
                    InvalidStockSharesQuantity.Text = "Not a multiple of 100";
                    //Not sure why this errormessage isn't working
                    //CustomValidator1.ErrorMessage = "Not a multiple of 100";

                    //Do you want it to be realtime or just run at the server?
                    //maybe that's part of the problem
                }
            }
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if ((SecurityType.SelectedValue == "Bond" || SecurityType.SelectedValue == "Unit Trust") && TransactionType.SelectedValue == "Sell")
            {
                int shares;
                Int32.TryParse(BondTrustSharesSelling.Text, out shares);
                if (shares <= 0)
                {
                    InvalidBondTrustSharesSelling.Text = "Please a enter a postivie number of shares to sell";
                }
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if ((SecurityType.SelectedValue == "Bond" || SecurityType.SelectedValue == "Unit Trust") && TransactionType.SelectedValue == "Buy")
            {
                int shares;
                Int32.TryParse(BondTrustSharesQuantity.Text, out shares);
                if (shares <= 0)
                {
                    InvalidBondTrustSharesQuantity.Text = "Please a enter a positive dollar amount";
                }
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
                    string varCode = StockCode.Text.ToString();
                    string varShares = StockSharesQuantity.Text.ToString();
                    string varOrderType = "";
                    string varExpiryDate = DaysUntilExpiration.SelectedValue;
                    string varAllOrNone = AllOrNone.Checked == true ? "Y" : "N";
                    string varStopPrice = StopPrice.Text.ToString();

                    //typeorder
                    if (OrderType.SelectedValue.Equals("Market Order"))
                    {
                        varOrderType = "market";
                    }
                    else if (OrderType.SelectedValue.Equals("Limit Order"))
                    {
                        varOrderType = "limit";
                    }
                    else if (OrderType.SelectedValue.Equals("Stop Order"))
                    {
                        varOrderType = "stop";
                    }
                    else if (OrderType.SelectedValue.Equals("Stop Limit Order"))
                    {
                        varOrderType = "stop limit";
                    }


                    if (TransactionType.SelectedValue.Equals("Buy"))
                    {
                        string highPrice = LimitPrice.Text.ToString();
                        string result = extFunction.submitStockBuyOrder(varCode, varShares, varOrderType, varExpiryDate, varAllOrNone, highPrice, varStopPrice);
                    }
                    else if (TransactionType.SelectedValue.Equals("Sell"))
                    {
                        string lowPrice = LimitPrice.Text.ToString();
                        string result = extFunction.submitStockSellOrder(varCode, varShares, varOrderType, varExpiryDate, varAllOrNone, lowPrice, varStopPrice);
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
                            
                            if (extFunction.submitBondBuyOrder(varBondTrustCode, varBondTrustSharesAmount) == null)
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                        }
                        else if(SecurityType.SelectedValue.Equals("Unit Trust"))
                        {
                            string success = extFunction.submitUnitTrustBuyOrder(varBondTrustCode, varBondTrustSharesAmount);
                            if (success.Equals("null"))
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                        }

                    }
                    else if (TransactionType.SelectedValue.Equals("Sell"))
                    {
                        string varBondTrustShares = BondTrustSharesSelling.Text.ToString();
                        if (SecurityType.SelectedValue.Equals("Bond"))
                        {

                            if (extFunction.submitBondSellOrder(varBondTrustCode, varBondTrustShares) == null)
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                        }
                        else if (SecurityType.SelectedValue.Equals("Unit Trust"))
                        {
                            string success = extFunction.submitUnitTrustSellOrder(varBondTrustCode, varBondTrustShares);
                            if (success.Equals("null"))
                            {
                                //Buy order was not succesfully submitted
                                InvalidBondTrustCode.Text = "The code given does not exist";
                            }
                        }
                    }
                }
               
            }
        }
    }
}