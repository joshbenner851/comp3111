using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using HKeInvestWebApplication.Code_File;
using HKeInvestWebApplication.ExternalSystems.Code_File;

namespace HKeInvestWebApplication
{
    public partial class ProfitLossTracking : System.Web.UI.Page
    {
        HKeInvestCode myHKeInvestCode = new HKeInvestCode();
        ExternalFunctions myExternalFunctions = new ExternalFunctions();

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
    }
}