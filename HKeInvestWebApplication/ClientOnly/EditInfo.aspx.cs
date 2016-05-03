using HKeInvestWebApplication.Code_File;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication.ClientOnly
{
    public partial class EditInfo : System.Web.UI.Page
    {
        HKeInvestData myHKeInvestData = new HKeInvestData();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private string FormatDateToSQL(string date)
        {
            if (date.Equals(""))
                return null;
            string day = date.Substring(0, 2);
            string month = date.Substring(3, 2);
            string year = date.Substring(6, 4);
            return year + "-" + month + "-" + day;
        }



        protected void CreateClient_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    //Get user information if on client
                    string accountNumber = AccountNumber.Text.Trim();
                    decimal balance = Convert.ToDecimal(DepositAmount.Text.Trim());

                    string updateAccount = "UPDATE Account SET VALUES (" + "'" + accountNumber + "','" +
                    RadioButtonList1.SelectedValue + "','" +
                    balance + "','" +
                    PrimarySource.SelectedValue + "','" +
                    OtherInformation.Text + "','" +
                    InvestmentObjective.SelectedValue + "','" +
                    InvestmentKnowledge.SelectedValue + "','" +
                    AnnualIncome.SelectedValue + "','" +
                    LiquidWorth.SelectedValue + "','" +
                    FreeCreditSwee.SelectedValue + "','" +
                    null + "') WHERE accountNumber = '" + accountNumber + "'";

                    SqlTransaction trans1 = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(updateAccount, trans1);
                    myHKeInvestData.commitTransaction(trans1);

                    string updateClient = "INSERT INTO Client VALUES ('" + accountNumber +
                        "','" + cbTitle.SelectedValue +
                        "','" + FirstName.Text + "'" + "," +
                        "'" + LastName.Text + "'" + "," +
                        "'" + FormatDateToSQL(DateOfBirth.Text) + "'" + "," +
                        "'" + Email.Text + "'" + "," +
                        "'" + Building.Text + "'" + "," +
                        "'" + Street.Text + "'" + "," +
                       "'" + District.Text + "'" + "," +
                       "'" + HomePhone.Text + "'" + "," +
                        "'" + HomeFax.Text + "'" + "," +
                        "'" + BusinessPhone.Text + "'" + "," +
                        "'" + MobilePhone.Text + "'" + "," +
                        "'" + CitizenshipCountry.Text + "'" + "," +
                        "'" + ResidenceCountry.Text + "'" + "," +
                        "'" + HKID.Text + "'" + "," +
                        "'" + PassportCountry.Text + "'" + "," +
                         "'" + cbEmploymentStatus.Text + "'" + "," +
                        "'" + SpecificOccupation.Text + "'" + "," +
                        "'" + EmployYears.Text + "'" + "," +
                        "'" + EmployName.Text + "'" + "," +
                        "'" + EmployPhone.Text + "'" + "," +
                        "'" + BusinessNature.Text + "'" + "," +
                         "'" + IsEmployedFinancial.SelectedValue + "'" + "," +
                        "'" + IsInIPO.SelectedValue + "'" + "," +
                         "'" + FormatDateToSQL(SignedOn.Text) + "'," +
                         "'Y')";

                    SqlTransaction trans2 = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(updateClient, trans2);
                    myHKeInvestData.commitTransaction(trans2);


                    if (!RadioButtonList1.SelectedValue.Equals("individual"))
                    {

                        string updateCOClient = "INSERT INTO Client Values ('" + accountNumber + "'," +
                        "'" + COcbTitle.SelectedValue + "'," +
                            "'" + COFirstName.Text + "'" + "," +
                        "'" + COLastName.Text + "'" + "," +
                        "'" + FormatDateToSQL(CODateOfBirth.Text) + "'" + "," +
                        "'" + COEmail.Text + "'" + "," +
                        "'" + COBuilding.Text + "'" + "," +
                        "'" + COStreet.Text + "'" + "," +
                        "'" + CODistrict.Text + "'" + "," +
                        "'" + COHomePhone.Text + "'" + "," +
                        "'" + COHomeFax.Text + "'" + "," +
                        "'" + COBusinessPhone.Text + "'" + "," +
                        "'" + COMobilePhone.Text + "'" + "," +
                        "'" + COCitizenshipCountry.Text + "'" + "," +
                        "'" + COResidenceCountry.Text + "'" + "," +
                        "'" + COHKID.Text + "'" + "," +
                        "'" + COPassportCountry.Text + "'" + "," +
                        "'" + COcbEmploymentStatus.Text + "'" + "," +
                        "'" + COSpecificOccupation.Text + "'" + "," +
                        "'" + COEmployYears.Text + "'" + "," +
                        "'" + COEmployName.Text + "'" + "," +
                        "'" + COEmployPhone.Text + "'" + "," +
                        "'" + COBusinessNature.Text + "'" + "," +
                        "'" + COIsEmployedFinancial.SelectedValue + "'" + "," +
                        "'" + COIsInIPO.SelectedValue + "'" + "," +
                        "'" + FormatDateToSQL(COSignedOn.Text) + "'" +
                        "'N')";

                        SqlTransaction trans3 = myHKeInvestData.beginTransaction();
                        myHKeInvestData.setData(updateCOClient, trans3);
                        myHKeInvestData.commitTransaction(trans3);

                    }
                    Console.WriteLine("Updated Successfully");
                    IdentityHelper.RedirectToReturnUrl(Request.QueryString["ReturnUrl"], Response);
                }
                catch (Exception f)
                {
                    Console.WriteLine("error thrown: " + f);
                }
            }
            else
            {
            }
        }

        protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadioButtonList1.SelectedValue.Equals("Individual"))
            {

            }
            else
            {

            }
        }

        protected void PrimarySource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PrimarySource.SelectedValue.Equals("Other"))
            {

            }
            else
            {

            }
        }
    }
}