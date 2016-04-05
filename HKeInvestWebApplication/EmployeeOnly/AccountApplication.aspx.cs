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
                lastletters = lastname.Substring(0,1)+lastname.Substring(0,1);
            }
            else
            {
                lastletters = lastname.Substring(0, 2);
            }
            lastletters = lastletters.ToUpper();
            string sql = "SELECT MAX(accountNumber) FROM Account WHERE AccountNumber LIKE " + "'"+lastletters+"%'";
            DataTable dtClient = myHKeInvestData.getData(sql);
            string accountNumber = "";
            if(dtClient.Rows.Count == 0)
            {
                accountNumber = lastletters + "00000000";
            }
            else
            {
                string prevAccountNumber = dtClient.Rows[0].ToString();
                int nextNumber = Int32.Parse(prevAccountNumber);
                nextNumber++;
                accountNumber = lastletters + nextNumber;
            }
            return accountNumber;
        }

        protected void CreateClient_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {

                string accountNumber = GenerateNextKey(LastName.Text.Trim());
                var balance1 = Convert.ToDecimal(ChequeAmount.Text);
                var balance2 = Convert.ToDecimal(TransferForm.Text);
                balance1 = balance1 + balance2;

                string updateAccount = "INSERT INTO Account VALUES (" + "'" + accountNumber + "','" +
                RadioButtonList1.SelectedValue + "','" +
                balance1 +"','"+
                PrimarySource.SelectedValue + "','" +
                OtherInformation.Text+"','"+
                InvestmentObjective.SelectedValue + "','" +
                InvestmentKnowledge.SelectedValue + "','" +
                AnnualIncome.SelectedValue + "','" +
                LiquidWorth.SelectedValue + "','" +
                FreeCreditSwee.SelectedValue + "')";

                SqlTransaction trans1 = myHKeInvestData.beginTransaction();
                myHKeInvestData.setData(updateAccount, trans1);
                myHKeInvestData.commitTransaction(trans1);

                string updateClient = "INSERT INTO Client VALUES ('" +accountNumber +
                    "'" + FirstName.Text + "'" + "," +
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
                     "'" + FormatDateToSQL(SignedOn.Text) + "'" + ")";

                SqlTransaction trans2 = myHKeInvestData.beginTransaction();
                myHKeInvestData.setData(updateClient, trans2);
                myHKeInvestData.commitTransaction(trans2);


                if(!CODateOfBirth.Text.Equals("") && !COSignedOn.Text.Equals("")){

                    string updateCOClient = "INSERT INTO Client Values (" +
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
                    ")";

                    SqlTransaction trans3 = myHKeInvestData.beginTransaction();
                    myHKeInvestData.setData(updateCOClient, trans3);
                    myHKeInvestData.commitTransaction(trans3);
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