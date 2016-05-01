using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;
using System.Data;

namespace HKeInvestWebApplication
{
    public partial class GenerateReport : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AccountNumber_TextChanged(object sender, EventArgs e)
        {
            lblClientName.Visible = false;
            string sql = "";
            string accountNumber = AccountNumber.Text.Trim(); // Set the account number from a web form control!
            DataTable data;
            decimal price = 0;

            //PART A
            AccountNum.Text = accountNumber;

            if (accountNumber == "")
            {
                lblClientName.Text = "Please specify an account number.";
                lblClientName.Visible = true;
                return;
            }

            sql = "SELECT firstName, lastName FROM Client " +
                "WHERE Client.accountNumber = '" + accountNumber + "'";

            data = myHKeInvestData.getData(sql);
            if (data == null) { return; }
            if (data.Rows.Count == 0)
            {
                lblClientName.Text = "No such account number.";
                return;
            }

            // Show the client name(s) on the web page.
            string clientName = "";
            int i = 1;
            foreach (DataRow row in data.Rows)
            {
                clientName = clientName + row["lastName"] + ", " + row["firstName"];
                if (data.Rows.Count != i)
                {
                    clientName = clientName + "and ";
                }
                i++;
            }

            ClientName.Text = clientName;

            sql = "select shares from SecurityHolding where accountNumber='" + accountNumber + "'";
            data = myHKeInvestData.getData(sql);

            sql = "select balance from Account where accountNumber='" + accountNumber + "'";
            data = myHKeInvestData.getData(sql);
            FreeBalance.Text = data.Rows[0][0].ToString();

            sql = "select code, shares from SecurityHolding where accountNumber='" + accountNumber + "' and type='stock'";
            data = myHKeInvestData.getData(sql);
            foreach (DataRow row in data.Rows)
            {
                price += myExternalFunctions.getSecuritiesPrice("stock", row[0].ToString()) * (decimal)row[1];
            }
            StockValue.Text = price.ToString();

            price = 0;

            sql = "select code, shares from SecurityHolding where accountNumber='" + accountNumber + "' and type='bond'";
            data = myHKeInvestData.getData(sql);
            foreach (DataRow row in data.Rows)
            {
                price += myExternalFunctions.getSecuritiesPrice("bond", row[0].ToString()) * (decimal)row[1];
            }
            BondValue.Text = price.ToString();

            price = 0;

            sql = "select code, shares from SecurityHolding where accountNumber='" + accountNumber + "' and type='unit trust'";
            data = myHKeInvestData.getData(sql);
            foreach (DataRow row in data.Rows)
            {
                price += myExternalFunctions.getSecuritiesPrice("unit trust", row[0].ToString()) * (decimal)row[1];
            }
            UnitTrustValue.Text = price.ToString();

            sql = "select dateSubmitted, amount from OrderHistory where accountNumber='" + accountNumber + "' order by dateSubmitted asc";
            data = myHKeInvestData.getData(sql);
            if (data.Rows.Count == 0)
            {
                LastOrderDate.Text = "No orders submitted";
                LastOrderValue.Text = "No orders submitted";
            } else
            {
                LastOrderDate.Text = data.Rows[0][0].ToString();
                LastOrderValue.Text = data.Rows[0][1].ToString();
            }

            //PART B
            sql = "select code, name, shares from SecurityHolding where accountNumber='" + accountNumber + "'";
            data = myHKeInvestData.getData(sql);

            //PART C

            //PART D
        }

        private DataTable JoinDataTables(DataTable t1, DataTable t2, params Func<DataRow, DataRow, bool>[] joinOn)
        {
            DataTable result = new DataTable();
            foreach (DataColumn col in t1.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataColumn col in t2.Columns)
            {
                if (result.Columns[col.ColumnName] == null)
                    result.Columns.Add(col.ColumnName, col.DataType);
            }
            foreach (DataRow row1 in t1.Rows)
            {
                var joinRows = t2.AsEnumerable().Where(row2 =>
                {
                    foreach (var parameter in joinOn)
                    {
                        if (!parameter(row1, row2)) return false;
                    }
                    return true;
                });
                foreach (DataRow fromRow in joinRows)
                {
                    DataRow insertRow = result.NewRow();
                    foreach (DataColumn col1 in t1.Columns)
                    {
                        insertRow[col1.ColumnName] = row1[col1.ColumnName];
                    }
                    foreach (DataColumn col2 in t2.Columns)
                    {
                        insertRow[col2.ColumnName] = fromRow[col2.ColumnName];
                    }
                    result.Rows.Add(insertRow);
                }
            }
            return result;
        }
    }
}