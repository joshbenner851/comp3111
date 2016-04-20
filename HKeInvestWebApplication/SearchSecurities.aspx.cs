using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Text.RegularExpressions;

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
            string input = SecurityInput.Text.Trim();

            DataTable data = new DataTable();

            //Reset all charts
            gvSearchBond.Visible = false;
            gvSearchStock.Visible = false;
            gvSearchUnitTrust.Visible = false;
            ErrorLabel.Visible = false;

            if (ddlSecurityType.SelectedValue.Equals("stock"))
            {
                if (Regex.IsMatch(input, @"^\d+$"))
                {
                    if (input.Length > 4)
                    {
                        ErrorLabel.Text = "Please enter a valid code.";
                        ErrorLabel.Visible = true;
                        return;
                    }
                    data = myExternalFunctions.getSecuritiesByCode("stock", input);
                } else
                {
                    data = myExternalFunctions.getSecuritiesByName("stock", input);
                }

                if (data == null)
                {
                    return;
                }
                if (data.Rows.Count == 0)
                {
                    ErrorLabel.Text = "No matches found.";
                    ErrorLabel.Visible = true;
                    return;
                }
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
                gvSearchStock.DataSource = data;
                Session["TaskTable"] = data;
                gvSearchStock.DataBind();
                gvSearchStock.Visible = true;
            } else if (ddlSecurityType.SelectedValue.Equals("bond"))
            {
                if (Regex.IsMatch(input, @"^\d+$"))
                {
                    if (input.Length > 4)
                    {
                        ErrorLabel.Text = "Please enter a valid code.";
                        ErrorLabel.Visible = true;
                        return;
                    }
                    data = myExternalFunctions.getSecuritiesByCode("bond", input);
                }
                else
                {
                    data = myExternalFunctions.getSecuritiesByName("bond", input);
                }

                if (data == null)
                {
                    return;
                }
                if (data.Rows.Count == 0)
                {
                    ErrorLabel.Text = "No matches found.";
                    ErrorLabel.Visible = true;
                    return;
                }
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
                gvSearchBond.DataSource = data;
                Session["TaskTable"] = data;
                gvSearchBond.DataBind();
                gvSearchBond.Visible = true;
            } else if (ddlSecurityType.SelectedValue.Equals("unit trust"))
            {
                if (Regex.IsMatch(input, @"^\d+$"))
                {
                    if (input.Length > 4)
                    {
                        ErrorLabel.Text = "Please enter a valid code.";
                        ErrorLabel.Visible = true;
                        return;
                    }
                    data = myExternalFunctions.getSecuritiesByCode("unit trust", input);
                }
                else
                {
                    data = myExternalFunctions.getSecuritiesByName("unit trust", input);
                }

                if (data == null)
                {
                    return;
                }
                if (data.Rows.Count == 0)
                {
                    ErrorLabel.Text = "No matches found.";
                    ErrorLabel.Visible = true;
                    return;
                }
                ViewState["SortExpression"] = "name";
                ViewState["SortDirection"] = "ASC";
                gvSearchUnitTrust.DataSource = data;
                Session["TaskTable"] = data;
                gvSearchUnitTrust.DataBind();
                gvSearchUnitTrust.Visible = true;
            }
        }

        protected void SecurityInput_TextChanged(object sender, EventArgs e)
        {
            ddlSecurityType_SelectedIndexChanged(sender, e);
        }

        private string GetSortDirection(string column)
        {

            // By default, set the sort direction to ascending.
            string sortDirection = "ASC";

            // Retrieve the last column that was sorted.
            string sortExpression = ViewState["SortExpression"] as string;

            if (sortExpression != null)
            {
                // Check if the same column is being sorted.
                // Otherwise, the default value can be returned.
                if (sortExpression == column)
                {
                    string lastDirection = ViewState["SortDirection"] as string;
                    if ((lastDirection != null) && (lastDirection == "ASC"))
                    {
                        sortDirection = "DESC";
                    }
                }
            }

            // Save new values in ViewState.
            ViewState["SortDirection"] = sortDirection;
            ViewState["SortExpression"] = column;

            return sortDirection;
        }

        protected void gvSearchStock_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt != null)
            {
                //Sort the data.
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvSearchStock.DataSource = Session["TaskTable"];
                gvSearchStock.DataBind();
            }
        }

        protected void gvSearchBond_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvSearchBond.DataSource = Session["TaskTable"];
                gvSearchBond.DataBind();
            }
        }

        protected void gvSearchUnitTrust_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataTable dt = Session["TaskTable"] as DataTable;

            if (dt != null)
            {
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection(e.SortExpression);
                gvSearchUnitTrust.DataSource = Session["TaskTable"];
                gvSearchUnitTrust.DataBind();
            }
        }
    }
}