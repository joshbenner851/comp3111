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

        private void GetIndividualSecurity()
        {
            string sql = "SELECT distinct securityType, name, securityCode, shares, amount,  " +
                "cast('0.00' AS numeric(12,2)) AS dollarBuying, " +
                "cast('0.00' AS numeric(12,2)) AS dollarSelling, " +
                "cast('0.00' AS numeric(12,2)) AS totalFees, " +
                "cast('0.00' AS numeric(12,2)) AS profitLoss " +
                "from OrderHistory where securityCode=" + SecurityCode.Text; // Complete the SQL statement.
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
            //var shares = 
            var amount = myHKeInvestData.getAggregateValue("select sum(amount) from OrderHistory where securityCode=" + SecurityCode.Text);

            dtClient.Rows[0]["amount"] = amount;

            // Bind the GridView to the DataTable.
            SingleSecurity.DataSource = dtClient;
            SingleSecurity.DataBind();

            // Set the visibility of GridView data.
            SingleSecurity.Visible = true;
        }
    }

   
}