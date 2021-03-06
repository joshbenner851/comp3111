﻿using HKeInvestWebApplication.Code_File;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HKeInvestWebApplication
{
    public partial class AccountApplication : System.Web.UI.Page
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

        private string GenerateNextKey(string lastname)
        {
            string lastletters = "";
            if (lastname.Length == 1)
            {
                lastletters = lastname.Substring(0, 1) + lastname.Substring(0, 1);
            }
            else
            {
                lastletters = lastname.Substring(0, 2);
            }
            lastletters = lastletters.ToUpper();
            string sql = "SELECT MAX(accountNumber) as max FROM Account WHERE AccountNumber LIKE " + "'" + lastletters + "%'";
            DataTable dtClient = myHKeInvestData.getData(sql);
            string accountNumber = "";
            if (dtClient == null || dtClient.Rows.Count == 0)
            {
                accountNumber = lastletters + "00000001";
            }
            else
            {
                string prevAccountNumber = dtClient.Rows[0]["max"].ToString();
                if (prevAccountNumber.Length != 10)
                {
                    accountNumber = lastletters + "00000001";
                }
                else
                {
                    int nextNumber = Int32.Parse(prevAccountNumber.Substring(2, 8));
                    nextNumber++;
                    string tempNumber = nextNumber.ToString();
                    accountNumber = lastletters;
                    for (int i = 8; i > tempNumber.Length; i--)
                    {
                        accountNumber += "0";
                    }
                    accountNumber += tempNumber;
                }
            }
            return accountNumber;
        }

        protected void CreateClient_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    string accountNumber = GenerateNextKey(LastName.Text.Trim());
                    decimal balance = Convert.ToDecimal(DepositAmount.Text.Trim());
                    string hkidPassNum = HKID.Text == "" ? PassportNumber.Text : HKID.Text;
                    string updateAccount = "INSERT INTO Account VALUES (" + "'" + accountNumber + "','" +
                    RadioButtonList1.SelectedValue + "','" +
                    balance + "','" +
                    PrimarySource.SelectedValue + "','" +
                    OtherInformation.Text + "','" +
                    InvestmentObjective.SelectedValue + "','" +
                    InvestmentKnowledge.SelectedValue + "','" +
                    AnnualIncome.SelectedValue + "','" +
                    LiquidWorth.SelectedValue + "','" +
                    FreeCreditSwee.SelectedValue + "','" +
                    null + "','" + 
                    RoutingNumber.Text + "','" +
                    BankAccountNumber.Text + "')";

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
                        "'" + hkidPassNum + "'" + "," +
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

                        string updateCOClient = "INSERT INTO Client VALUES ('" + accountNumber + "'," +
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
                        "'" + FormatDateToSQL(COSignedOn.Text) + "'," +
                        "'N')";


                        //Error is with this insertion
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

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(HomePhone.Text == "" && MobilePhone.Text == "" && BusinessPhone.Text == "")
            {
                CustomValidator1.ErrorMessage = "At least one of Home Phone/Business Phone/Mobile Phones";
                args.IsValid = false;
            }
        }

        protected void CustomValidator2_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if(HKID.Text == "" && ( PassportCountry.Text == "" || PassportNumber.Text == ""))
            {
                CustomValidator2.ErrorMessage = "Either HKID or Passport # and Passport Country of Issue are required";
                args.IsValid = false;
            }
        }

        protected void CustomValidator3_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (RadioButton1.Checked)
            {
                if(RoutingNumber.Text == "")
                {
                    CustomValidator3.ErrorMessage = "Routing number cannot be blank";
                    args.IsValid = false;
                }
                else if(RoutingNumber.Text.Substring(0,1) != "1")
                {
                    CustomValidator3.ErrorMessage = "Routing number must start with a 1";
                    args.IsValid = false;
                }
                else if(RoutingNumber.Text.Length < 9)
                {
                    CustomValidator3.ErrorMessage = "Routing number must be 9 digits long";
                    args.IsValid = false;
                }
      
                
            }
        }

        protected void CustomValidator4_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (RadioButton1.Checked)
            {
                if (BankAccountNumber.Text == "")
                {
                    CustomValidator4.ErrorMessage = "Bank Account number cannot be blank";
                    args.IsValid = false;
                }
                //Complete abritrary bullshit validation because can't find bank account standards
                else if (BankAccountNumber.Text.Substring(0, 3) != "333")
                {
                    CustomValidator4.ErrorMessage = "Bank account number must start with 333";
                    args.IsValid = false;
                }
                else if (BankAccountNumber.Text.Length < 10)
                {
                    CustomValidator4.ErrorMessage = "Bank account number must be at least 10 digits long";
                    args.IsValid = false;
                }


            }
        }
    }
}