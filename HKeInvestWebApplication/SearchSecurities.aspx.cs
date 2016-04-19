using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication
{
    public partial class SearchSecurities : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ddlSecurityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            string name = SecurityName.Text.Trim();
            string code = SecurityCode.Text.Trim();
            DataTable data;

            //Reset all charts
            gvSearchBond.Visible = false;
            gvSearchStock.Visible = false;
            gvSearchUnitTrust.Visible = false;
            ErrorLabel.Visible = false;

            //Do stuff here
            if (ddlSecurityType.SelectedValue.Equals("stock"))
            {
                sql += "select * from Stock where code = '" + code + "' or name like '" + name + "%'";
                data = myHKeInvestData.getData(sql);
                if (data == null) { return; } // If the DataSet is null, a SQL error occurred.
                // If no result is returned, then display a message that the account does not hold this type of security.
                if (data.Rows.Count == 0)
                {
                    ErrorLabel.Text = "No matches found.";
                    ErrorLabel.Visible = true;
                    return;
                }
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
                gvSearchStock.DataSource = data;
                gvSearchStock.DataBind();
                gvSearchStock.Visible = true;
            } else if (ddlSecurityType.SelectedValue.Equals("bond"))
            {
                sql += "select * from Bond where code = '" + code + "' or name like '" + name + "%'";
                data = myHKeInvestData.getData(sql);
                if (data == null) { return; } // If the DataSet is null, a SQL error occurred.
                // If no result is returned, then display a message that the account does not hold this type of security.
                if (data.Rows.Count == 0)
                {
                    ErrorLabel.Text = "No matches found.";
                    ErrorLabel.Visible = true;
                    return;
                }
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
                gvSearchBond.DataSource = data;
                gvSearchBond.DataBind();
                gvSearchBond.Visible = true;
            } else if (ddlSecurityType.SelectedValue.Equals("unit trust"))
            {
                sql += "select * from UnitTrust where code = '" + code + "' or name like '" + name + "%'";
                data = myHKeInvestData.getData(sql);
                if (data == null) { return; } // If the DataSet is null, a SQL error occurred.
                // If no result is returned, then display a message that the account does not hold this type of security.
                if (data.Rows.Count == 0)
                {
                    ErrorLabel.Text = "No matches found.";
                    ErrorLabel.Visible = true;
                    return;
                }
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
                gvSearchUnitTrust.DataSource = data;
                gvSearchUnitTrust.DataBind();
                gvSearchUnitTrust.Visible = true;
            }
        }
    }
}