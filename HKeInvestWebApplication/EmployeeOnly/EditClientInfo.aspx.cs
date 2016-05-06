using HKeInvestWebApplication.Code_File;
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
    public partial class EditClientInfo : System.Web.UI.Page
    {

        HKeInvestData myHKeInvestData = new HKeInvestData();
        protected void Page_Load(object sender, EventArgs e)
        {
            mainform.Visible = false;
            coAccount3.Visible = false;
            coAccount2.Visible = false;
            coAccount4.Visible = false;
            passport.Visible = false;
            copassport.Visible = false;
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
            string accountNumber = AccountNumber.Text.Trim();

            string sqlAccount = "SELECT * FROM Account WHERE accountNumber = '" + accountNumber + "'";
            string sqlClient = "SELECT * FROM Client WHERE accountNumber = '" + accountNumber + "'";

            DataTable dtClient = myHKeInvestData.getData(sqlClient);
            if(dtClient == null || dtClient.Rows.Count == 0)
            {
                //Error invalid sql
            }   
            else if (dtClient.Rows.Count == 1)
            {
                mainform.Visible = true;
                passport.Visible = dtClient.Rows[0]["passportCountry"].ToString().Trim() != "" ? true : false;
            }   
            else if(dtClient.Rows.Count == 2)
            {
                mainform.Visible = true;
                coAccount2.Visible = true;
                coAccount3.Visible = true;
                coAccount4.Visible = true;

                if(dtClient.Rows[0]["isPrimary"].ToString().Trim() == "Y")
                {
                    passport.Visible = dtClient.Rows[0]["passportCountry"].ToString().Trim() != "" ? true : false;
                    copassport.Visible = dtClient.Rows[1]["passportCountry"].ToString().Trim() != "" ? true : false;

                }
                else
                {
                    passport.Visible = dtClient.Rows[1]["passportCountry"].ToString().Trim() != "" ? true : false;
                    copassport.Visible = dtClient.Rows[0]["passportCountry"].ToString().Trim() != "" ? true : false;

                }
            }     
        }

        protected void CreateClient_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    
                    //Get user information if on client
                    string accountNumber = AccountNumber.Text.Trim();

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
                    
                    if(updateAccount.Length > 56 && mainform.Visible)
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
                    if (FirstName.Text.Trim() != "")
                    {
                        updateClient += "firstName='" + FirstName.Text.Trim() + "',";
                    }
                    if (LastName.Text.Trim() != "")
                    {
                        updateClient += "lastName='" + LastName.Text.Trim() + "',";
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
                    //Execute sql to check if HKID, or passport has been updated

                    //Will fail if there is no regex from the 
                    string sql = "SELECT * FROM Client WHERE accountNumber = '" + accountNumber + "' AND isPrimary = 'Y'";
                    DataTable temp = myHKeInvestData.getData(sql);

                    if (temp.Rows[0]["passportCountry"].ToString() != "")
                    {
                        if (HKID.Text != "")
                        {
                            updateClient += "HKIDPassportNumber='" + HKID.Text + "',";
                        }
                        if (PassportCountry.Text != "")
                        {
                            updateClient += "passportCountry='" + PassportCountry.Text + "',";
                        }
                    }
                    if (CitizenshipCountry.Text != "")
                    {
                        updateClient += "citizenship='" + CitizenshipCountry.Text + "',";
                    }
                    if (ResidenceCountry.Text != "")
                    {
                        updateClient += "legalResidence='" + ResidenceCountry.Text + "',";
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

                    //Checking to see if any fields in client are to be updated (Length 77)
                    if (updateClient.Length > 77 && mainform.Visible)
                    {
                        SqlTransaction trans2 = myHKeInvestData.beginTransaction();
                        myHKeInvestData.setData(updateClient, trans2);
                        myHKeInvestData.commitTransaction(trans2);
                    }


                    //Add some sort of auto postback for the account information that should be displayed
                    updateClient = "UPDATE Client SET ";

                    if (COcbTitle.SelectedValue != "")
                    {
                        updateClient += "title='" + COcbTitle.SelectedValue + "',";
                    }
                    if (COFirstName.Text.Trim() != "")
                    {
                        updateClient += "firstName='" + COFirstName.Text.Trim() + "',";
                    }
                    if (COLastName.Text.Trim() != "")
                    {
                        updateClient += "lastName='" + COLastName.Text.Trim() + "',";
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

                    //Will fail if there is no regex from the 
                    sql = "SELECT * FROM Client WHERE accountNumber = '" + accountNumber + "' AND isPrimary='N'";
                    temp = myHKeInvestData.getData(sql);

                    //No error checking on temp

                    //Only allow for passport information updating
                    if (temp.Rows[0]["passportCountry"].ToString() != "")
                    {
                        if (COHKID.Text != "")
                        {
                            updateClient += "HKIDPassportNumber='" + COHKID.Text + "',";
                        }
                        if (COPassportCountry.Text != "")
                        {
                            updateClient += "passportCountry='" + COPassportCountry.Text + "',";
                        }
                    }
                    if (COCitizenshipCountry.Text != "")
                    {
                        updateClient += "citizenship='" + COCitizenshipCountry.Text + "',";
                    }
                    if (COResidenceCountry.Text != "")
                    {
                        updateClient += "legalResidence='" + COResidenceCountry.Text + "',";
                    }
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

                    if (updateClient.Length > 77 && coAccount2.Visible)
                    {

                        SqlTransaction trans3 = myHKeInvestData.beginTransaction();
                        myHKeInvestData.setData(updateClient, trans3);
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


    }
}