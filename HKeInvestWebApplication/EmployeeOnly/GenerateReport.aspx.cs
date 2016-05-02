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
            if (!IsPostBack)
            {
                myHKeInvestCode.retrieveCurrency(Session);
            }
            securityType.Visible = false;
        }

        protected void AccountNumber_TextChanged(object sender, EventArgs e)
        {
            lblClientName.Visible = false;
            string sql = "";
            string accountNumber = AccountNumber.Text.Trim(); // Set the account number from a web form control!
            DataTable data;
            decimal price = 0;
            decimal total = 0;

            //PART A

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

            sql = "select balance from Account where accountNumber='" + accountNumber + "'";
            data = myHKeInvestData.getData(sql);
            FreeBalance.Text = data.Rows[0][0].ToString();
            total += (decimal)data.Rows[0][0];

            sql = "select code, shares, base from SecurityHolding where accountNumber='" + accountNumber + "' and type='stock'";
            data = myHKeInvestData.getData(sql);
            foreach (DataRow row in data.Rows)
            {

                price += convertCurrency(row[2].ToString(), "HKD", myExternalFunctions.getSecuritiesPrice("stock", row[0].ToString()) * (decimal)row[1]);
            }
            StockValue.Text = price.ToString("F");
            total += price;
            price = 0;

            sql = "select code, shares, base from SecurityHolding where accountNumber='" + accountNumber + "' and type='bond'";
            data = myHKeInvestData.getData(sql);
            foreach (DataRow row in data.Rows)
            {
                price += convertCurrency(row[2].ToString(), "HKD", myExternalFunctions.getSecuritiesPrice("bond", row[0].ToString()) * (decimal)row[1]);
            }
            BondValue.Text = price.ToString("F");
            total += price;
            price = 0;

            sql = "select code, shares, base from SecurityHolding where accountNumber='" + accountNumber + "' and type='unit trust'";
            data = myHKeInvestData.getData(sql);
            foreach (DataRow row in data.Rows)
            {
                price += convertCurrency(row[2].ToString(), "HKD", myExternalFunctions.getSecuritiesPrice("unit trust", row[0].ToString()) * (decimal)row[1]);
            }
            UnitTrustValue.Text = price.ToString("F");
            total += price;
            price = 0;

            TotalValue.Text = total.ToString("F");

            sql = "select dateSubmitted, amount from OrderHistory where accountNumber='" + accountNumber + "' and status='completed' order by dateSubmitted asc";
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
            securityType.Visible = true;
            gvSecurities.Visible = true;
            sql = "select code, name, shares, '0.00' as price, '0.00' as totalValue, 'HKD' as base from SecurityHolding where accountNumber='" + accountNumber + "' and type='stock'";
            data = myHKeInvestData.getData(sql);
            if (data.Rows.Count != 0)
            {
                gvSecurities.DataSource = data;
                gvSecurities.DataBind();
                DataTable dtSecurities = myHKeInvestCode.unloadGridView(gvSecurities);
                foreach (DataRow row in dtSecurities.Rows)
                {
                    decimal prix = myExternalFunctions.getSecuritiesPrice("stock", row[0].ToString());
                    row[3] = prix;
                    row[4] = ((decimal)(prix * (decimal)row[2])).ToString("F");
                }
                gvSecurities.DataSource = dtSecurities;
                gvSecurities.DataBind();
            } else
            {
                SecurityError.Text = "No stocks found";
                SecurityError.Visible = true;
                gvSecurities.Visible = false;
            }


            //PART C
            sql = "select referenceNumber, buyOrSell, securityType, securityCode";
            
            //PART D
        }

        /*
        Usage:
        var test = JoinDataTables(transactionInfo, transactionItems,
               (row1, row2) =>
               row1.Field<int>("TransactionID") == row2.Field<int>("TransactionID"));
        */
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

        protected decimal convertCurrency(string from, string to, decimal value)
        {
            decimal fromRate = 1, toRate = 1;
            DataTable dtCurrency = (DataTable)Session["currency"];
            foreach (DataRow row in dtCurrency.Rows)
            {
                if (row[0].Equals(from))
                {
                    fromRate = (decimal)row[1];
                }
                if (row[0].Equals(to))
                {
                    toRate = (decimal)row[1];
                }
            }
            return fromRate / toRate * value;
        }

        protected void securityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = "";
            DataTable data;
            string accountNumber = AccountNum.Text;
            gvSecurities.Visible = true;
            securityType.Visible = true;
            if (securityType.SelectedValue.Equals("stock"))
            {
                sql = "select code, name, shares, '0.00' as price, '0.00' as totalValue, 'HKD' as base from SecurityHolding where accountNumber='" + accountNumber + "' and type='stock'";
                data = myHKeInvestData.getData(sql);
                if (data.Rows.Count != 0)
                {
                    gvSecurities.DataSource = data;
                    gvSecurities.DataBind();
                    DataTable dtSecurities = myHKeInvestCode.unloadGridView(gvSecurities);
                    foreach (DataRow row in dtSecurities.Rows)
                    {
                        decimal prix = myExternalFunctions.getSecuritiesPrice("stock", row[0].ToString());
                        row[3] = prix;
                        row[4] = ((decimal)(prix * (decimal)row[2])).ToString("F");
                    }
                    gvSecurities.DataSource = dtSecurities;
                    gvSecurities.DataBind();
                } else
                {
                    SecurityError.Text = "No stocks found";
                    SecurityError.Visible = true;
                    gvSecurities.Visible = false;
                }

            } else if (securityType.SelectedValue.Equals("bond"))
            {
                sql = "select code, name, shares, '0.00' as price, '0.00' as totalValue, 'HKD' as base from SecurityHolding where accountNumber='" + accountNumber + "' and type='bond'";
                data = myHKeInvestData.getData(sql);
                if (data.Rows.Count != 0)
                {
                    gvSecurities.DataSource = data;
                    gvSecurities.DataBind();
                    DataTable dtSecurities = myHKeInvestCode.unloadGridView(gvSecurities);
                    foreach (DataRow row in dtSecurities.Rows)
                    {
                        decimal prix = myExternalFunctions.getSecuritiesPrice("bond", row[0].ToString());
                        row[3] = prix;
                        row[4] = ((decimal)(prix * (decimal)row[2])).ToString("F");
                    }
                    gvSecurities.DataSource = dtSecurities;
                    gvSecurities.DataBind();
                } else
                {
                    SecurityError.Text = "No bonds found";
                    SecurityError.Visible = true;
                    gvSecurities.Visible = false;
                }

            } else
            {
                sql = "select code, name, shares, '0.00' as price, '0.00' as totalValue, 'HKD' as base from SecurityHolding where accountNumber='" + accountNumber + "' and type='unit trust'";
                data = myHKeInvestData.getData(sql);
                gvSecurities.DataSource = data;
                if (data.Rows.Count != 0)
                {
                    gvSecurities.DataBind();
                    DataTable dtSecurities = myHKeInvestCode.unloadGridView(gvSecurities);
                    foreach (DataRow row in dtSecurities.Rows)
                    {
                        decimal prix = myExternalFunctions.getSecuritiesPrice("unit trust", row[0].ToString());
                        row[3] = prix;
                        row[4] = ((decimal)(prix * (decimal)row[2])).ToString("F");
                    }
                    gvSecurities.DataSource = dtSecurities;
                    gvSecurities.DataBind();
                } else
                {
                    SecurityError.Text = "No unit trusts found";
                    SecurityError.Visible = true;
                    gvSecurities.Visible = false;
                }

            }
        }
    }
}