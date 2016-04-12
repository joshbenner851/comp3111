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
            }
            else if (TransactionType.SelectedValue == "Sell")
            {
                buyBondTrust.Style.Add("display", "none");
                sellBondTrust.Style.Add("display", "");
            }
        }

        protected void OrderType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (OrderType.SelectedValue == "market")
            {
                market.Style.Add("display", "");
                limit.Style.Add("display", "none");
                stop.Style.Add("display", "none");
                stoporder.Style.Add("display", "none");
            }
            else if (OrderType.SelectedValue == "limit")
            {
                market.Style.Add("display", "none");
                limit.Style.Add("display", "");
                stop.Style.Add("display", "none");
                stoporder.Style.Add("display", "none");
            }
            else if (OrderType.SelectedValue == "stop")
            {
                market.Style.Add("display", "none");
                limit.Style.Add("display", "none");
                stop.Style.Add("display", "");
                stoporder.Style.Add("display", "none");
            }
            else if (OrderType.SelectedValue == "stoporder")
            {
                market.Style.Add("display", "none");
                limit.Style.Add("display", "none");
                stop.Style.Add("display", "none");
                stoporder.Style.Add("display", "");
            }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}