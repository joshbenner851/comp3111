﻿using HKeInvestWebApplication.Code_File;
using Microsoft.AspNet.Identity;
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
    public partial class ClientEditClientInfo : System.Web.UI.Page
    {

        HKeInvestData myHKeInvestData = new HKeInvestData();
        protected void Page_Load(object sender, EventArgs e)
        {
            coAccount3.Visible = false;
            coAccount2.Visible = false;
            coAccount4.Visible = false;
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

        //Error checking on display of program
        protected void AccountNumberChanged(object sender, EventArgs e)
        {
            string accountNumber = getAccountNumber();

            string sqlAccount = "SELECT * FROM Account WHERE accountNumber = '" + accountNumber + "'";
            string sqlClient = "SELECT * FROM Client WHERE accountNumber = '" + accountNumber + "'";

            DataTable dtClient = myHKeInvestData.getData(sqlClient);
            if(dtClient == null || dtClient.Rows.Count == 0)
            {
                //Error invalid sql
            }   
            else if (dtClient.Rows.Count == 1)
            {
            }   
            else if(dtClient.Rows.Count == 2)
            {
                coAccount2.Visible = true;
                coAccount3.Visible = true;
                coAccount4.Visible = true;
            }     
        }

        public string getAccountNumber()
        {
            string sql = "SELECT accountNumber FROM Account WHERE userName = '" +
               HttpContext.Current.User.Identity.GetUserName() + "'";

            DataTable temp = myHKeInvestData.getData(sql);

            //ERROR: no error catching for not having the account number

            return temp.Rows[0]["accountNumber"].ToString();

        }

        protected void CreateClient_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    //Get user information if on client
                    string accountNumber = getAccountNumber();

                    //TODO figure out some way to bind data

                   

                    //Update account

                    string updateAccount = "UPDATE Account SET ";

                    //If statement to update the sql commands
                    if (PrimarySource.SelectedValue != "")
                    {
                        updateAccount += "sourceOfFunds='"+PrimarySource.SelectedValue + "',";
                    }
                    if (OtherInformation.Text.Trim() != "")
                    {
                        updateAccount += "otherSource='"+OtherInformation.Text.Trim() + "',";
                    }
                    //Code for investment objective
                    if(InvestmentObjective.SelectedValue != "")
                    {
                        updateAccount += "investmentObjective='" + InvestmentObjective.SelectedValue + "',";
                    }
                    if(InvestmentKnowledge.SelectedValue != "")
                    {
                        updateAccount += "investmentKnowledge='"+InvestmentKnowledge.SelectedValue + "',";
                    }
                    if(AnnualIncome.SelectedValue != "")
                    {
                        updateAccount += "annualIncome='"+AnnualIncome.SelectedValue + "',";
                    }
                    if(LiquidWorth.SelectedValue != "")
                    {
                        updateAccount += "approxLiquidNetWorth='"+LiquidWorth.SelectedValue + "',";
                    }
                    if(FreeCreditSwee.SelectedValue != "")
                    {
                        updateAccount += "sweepFreeCreditBalance='"+FreeCreditSwee + "',";
                    }

                    //Code to delete last comma
                    updateAccount = updateAccount.Remove(updateAccount.Length - 1);

                    updateAccount = " WHERE accountNumber = '" + accountNumber + "'";

                    if (updateAccount.Length > 56)
                    {
                        SqlTransaction trans1 = myHKeInvestData.beginTransaction();
                        myHKeInvestData.setData(updateAccount, trans1);
                        myHKeInvestData.commitTransaction(trans1);
                    }

                    string updateClient = "UPDATE Client SET ";

                    if(cbTitle.SelectedValue != "")
                    {
                        updateClient += "title='" + cbTitle.SelectedValue + "',";
                    }
                    if (Email.Text != "")
                    {
                        updateClient += "email" + Email.Text + "',";
                    }
                    if (Building.Text != "")
                    {
                        updateClient += "building='" + Building.Text.Trim() + "',";
                    }
                    if(Street.Text != "")
                    {
                        updateClient += "street='" + Street.Text + "',";
                    }
                    if(District.Text != "")
                    {
                        updateClient += "district=" + District.Text + "',";
                    }
                    if(HomePhone.Text != "")
                    {
                        updateClient += "homePhone='" + HomePhone.Text + "',";
                    }
                    if(HomeFax.Text != "")
                    {
                        updateClient += "homeFax='" + HomeFax.Text + "',";
                    }
                    if (BusinessPhone.Text != "")
                    {
                        updateClient += "businessPhone='" + BusinessPhone.Text + "',";
                    }
                    if (MobilePhone.Text != "")
                    {
                        updateClient += "mobilePhone='" + MobilePhone.Text + "',";
                    }
                    if (cbEmploymentStatus.Text != "")
                    {
                        updateClient += "employeeStatus'" + cbEmploymentStatus.Text + "',";
                    }
                    if (SpecificOccupation.Text != "")
                    {
                        updateClient += "occupation='"+SpecificOccupation.Text + "',";
                    }
                    if (EmployYears.Text != "")
                    {
                        updateClient += "years='"+EmployYears.Text + "',";
                    }
                    if (EmployName.Text != "")
                    {
                        updateClient += "employerName='"+EmployName.Text + "',";
                    }
                    if (EmployPhone.Text != "")
                    {
                        updateClient += "employerPhone='"+EmployPhone.Text + "',";
                    }
                    if (BusinessNature.Text != "")
                    {
                        updateClient += "natureBusiness='"+BusinessNature + "','";
                    }
                    if (IsEmployedFinancial.SelectedValue != "")
                    {
                        updateClient += "isEmployedFinance'="+IsEmployedFinancial.SelectedValue + "',";
                    }
                    if (IsInIPO.SelectedValue != "")
                    {
                        updateClient += "isPubliclyTraded'="+IsInIPO.SelectedValue + "',";
                    }
                    
                    updateClient = updateAccount.Remove(updateAccount.Length - 1);

                    updateClient = " WHERE accountNumber = '" + accountNumber + "' AND isPrimary = 'Y'";


                    if (updateAccount.Length > 77)
                    {
                        SqlTransaction trans1 = myHKeInvestData.beginTransaction();
                        myHKeInvestData.setData(updateAccount, trans1);
                        myHKeInvestData.commitTransaction(trans1);
                    }


                    //Add some sort of auto postback for the account information that should be displayed
                    updateClient = "UPDATE Client SET ";

                    if (COcbTitle.SelectedValue != "")
                    {
                        updateClient += "title='" + COcbTitle.SelectedValue + "',";
                    }
                  
                    if (COEmail.Text != "")
                    {
                        updateClient += "email" + COEmail.Text + "',";
                    }
                    if (COBuilding.Text != "")
                    {
                        updateClient += "building='" + COBuilding.Text.Trim() + "',";
                    }
                    if (COStreet.Text != "")
                    {
                        updateClient += "street='" + COStreet.Text + "',";
                    }
                    if (CODistrict.Text != "")
                    {
                        updateClient += "district=" + CODistrict.Text + "',";
                    }
                    if (COHomePhone.Text != "")
                    {
                        updateClient += "homePhone='" + COHomePhone.Text + "',";
                    }
                    if (COHomeFax.Text != "")
                    {
                        updateClient += "homeFax='" + COHomeFax.Text + "',";
                    }
                    if (COBusinessPhone.Text != "")
                    {
                        updateClient += "businessPhone='" + COBusinessPhone.Text + "',";
                    }
                    if (COMobilePhone.Text != "")
                    {
                        updateClient += "mobilePhone='" + COMobilePhone.Text + "',";
                    }
                    //Execute sql to check if HKID, or passport has been updated

                    
                    if (COcbEmploymentStatus.Text != "")
                    {
                        updateClient += "employeeStatus'" + COcbEmploymentStatus.Text + "',";
                    }
                    if (COSpecificOccupation.Text != "")
                    {
                        updateClient += "occupation='" + COSpecificOccupation.Text + "',";
                    }
                    if (COEmployYears.Text != "")
                    {
                        updateClient += "years='" + COEmployYears.Text + "',";
                    }
                    if (COEmployName.Text != "")
                    {
                        updateClient += "employerName='" + COEmployName.Text + "',";
                    }
                    if (COEmployPhone.Text != "")
                    {
                        updateClient += "employerPhone='" + COEmployPhone.Text + "',";
                    }
                    if (COBusinessNature.Text != "")
                    {
                        updateClient += "natureBusiness='" + COBusinessNature + "','";
                    }
                    if (COIsEmployedFinancial.SelectedValue != "")
                    {
                        updateClient += "isEmployedFinance'=" + COIsEmployedFinancial.SelectedValue + "',";
                    }
                    if (COIsInIPO.SelectedValue != "")
                    {
                        updateClient += "isPubliclyTraded'=" + COIsInIPO.SelectedValue + "',";
                    }
                    //Removce final comma
                    updateClient = updateAccount.Remove(updateAccount.Length - 1);

                    updateClient = " WHERE accountNumber = '" + accountNumber + "' AND isPrimary = 'N'";



                    if (updateAccount.Length > 77 && coAccount2.Visible)
                    {
                        SqlTransaction trans1 = myHKeInvestData.beginTransaction();
                        myHKeInvestData.setData(updateAccount, trans1);
                        myHKeInvestData.commitTransaction(trans1);
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


    }
}