<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="HKeInvestWebApplication._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h1>Download Form</h1>
        <p class="lead">HKeInvest is the premier investing platform. Trust us with your money</p>
        <asp:Button ID="button" runat="server" Text="Download" OnClick="button_Click" />
    </div>

</asp:Content>
