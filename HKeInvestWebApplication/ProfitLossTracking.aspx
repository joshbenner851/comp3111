<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfitLossTracking.aspx.cs" Inherits="HKeInvestWebApplication.ProfitLossTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Profit Loss</h1>
    <div class="container">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label ID="Label1" runat="server" Text="Value Profit/Loss should be in"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Value is required">*</asp:RequiredFieldValidator>
                <asp:RadioButtonList ID="RadioButtonList1" runat="server">
                    <asp:ListItem>Dollar Amount</asp:ListItem>
                    <asp:ListItem>Percent</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            
            <div class="form-group">
                <asp:Label ID="Label2" runat="server" Text="Search type"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Search type is required">*</asp:RequiredFieldValidator>
                <asp:RadioButtonList ID="RadioButtonList2" runat="server" AutoPostBack="True">
                    <asp:ListItem>Individual Security</asp:ListItem>
                    <asp:ListItem>All Securities of Type</asp:ListItem>
                    <asp:ListItem>All Securities</asp:ListItem>
                </asp:RadioButtonList>
            </div>

                
            <div class="form-group">
                <asp:Label ID="Label3" runat="server" Text="Security Type"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Value is required">*</asp:RequiredFieldValidator>
                <br />
                <asp:DropDownList ID="DropDownList1" runat="server">
                  <asp:ListItem Value="stock">Stock</asp:ListItem>
                  <asp:ListItem Value="bond">Bond</asp:ListItem>
                  <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
                <br />
            </div>
            
            <div class="form-group">
                <asp:Label ID="Label4" runat="server" Text="Security Code"></asp:Label>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Please enter digits only"></asp:RegularExpressionValidator>
                
            </div>

            <div class="form-group">
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>
            </div>

            <div class="form-group">
                <asp:GridView ID="GridView2" runat="server">
                </asp:GridView>
            </div>

            <div class="form-group">
                <asp:GridView ID="GridView3" runat="server">
                </asp:GridView>
            </div>
            
            
        </div>
    </div>


</asp:Content>
