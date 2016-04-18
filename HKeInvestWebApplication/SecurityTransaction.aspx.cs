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
            if (SecurityType.SelectedValue == "Bond/Unit Trust")
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
                Int32.TryParse(SharesQuantity.Text,out shares);
                if (shares % 100 != 0)
                {
                    Label1.Text = "Not a multiple of 100";
                    //Not sure why this errormessage isn't working
                    //CustomValidator1.ErrorMessage = "Not a multiple of 100";

                    //Do you want it to be realtime or just run at the server?
                    //maybe that's part of the problem
                }
            }
        }

        protected void ExecuteTransactionClick(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //do work
            }
        }
    }
}