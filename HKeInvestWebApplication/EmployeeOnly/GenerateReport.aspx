<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" Inherits="HKeInvestWebApplication.GenerateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Generate Report</h2>
    <div>
        <asp:Label runat="server" Text="Account Number" AssociatedControlID="AccountNumber"></asp:Label><asp:TextBox ID="AccountNumber" runat="server" MaxLength="10" AutoPostBack="True" OnTextChanged="AccountNumber_TextChanged"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblClientName" runat="server" Visible="False"></asp:Label>
    </div>
</asp:Content>