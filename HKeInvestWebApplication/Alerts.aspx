﻿<%@ Page Title="Alerts" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Alerts.aspx.cs" Inherits="HKeInvestWebApplication.Alerts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Set an Alert</h1>
    <div class="container">
        <div class="form-group">
            <asp:Label ID="lblAlertType" runat="server" Text="Alert Type" AssociatedControlID="rblAlertType"></asp:Label>
            <asp:RadioButtonList ID="rblAlertType" runat="server">
                <asp:ListItem>High</asp:ListItem>
                <asp:ListItem>Low</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvAlertType" runat="server" CssClass="text-danger" ErrorMessage="Alert Type is required." ControlToValidate="rblAlertType" Display="Dynamic"></asp:RequiredFieldValidator>
            <br />
                <asp:Label ID="lblSecurityTypeInput" runat="server" Text="Security Type:" AssociatedControlID="rblSecurityTypeInput"></asp:Label>
            <asp:RadioButtonList ID="rblSecurityTypeInput" runat="server">
                <asp:ListItem>Stock</asp:ListItem>
                <asp:ListItem>Bond</asp:ListItem>
                <asp:ListItem>Unit Trust</asp:ListItem>
            </asp:RadioButtonList>
            <asp:RequiredFieldValidator ID="rfvSecurityTypeInput" runat="server" CssClass="text-danger" Display="Dynamic" ErrorMessage="Security Type is required." ControlToValidate="rblSecurityTypeInput"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblSecurityCodeInput" runat="server" Text="Security Code:" AssociatedControlID="tbxSecurityCodeInput"></asp:Label>
            <br />
            <asp:TextBox ID="tbxSecurityCodeInput" runat="server"></asp:TextBox>
            <asp:CustomValidator ID="cuvSecurityCodeInput" runat="server" ControlToValidate="tbxSecurityCodeInput" CssClass="text-danger" Display="Dynamic" ErrorMessage="Security Code does not correspond to a security owned by the client." OnServerValidate="cuvSecurityCodeInput_ServerValidate"></asp:CustomValidator>
            <asp:RegularExpressionValidator ID="revSecurityCodeInput" runat="server" ControlToValidate="tbxSecurityCodeInput" CssClass="text-danger" Display="Dynamic" ErrorMessage="Security Code must be only numberic characters." ValidationExpression="[\d]+"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="rfvSecurityCodeInput" runat="server" ControlToValidate="tbxSecurityCodeInput" CssClass="text-danger" ErrorMessage="Security Code is required." Display="Dynamic"></asp:RequiredFieldValidator>
            <br />
            <asp:Label ID="lblAlertValue" runat="server" Text="Alert Value:" AssociatedControlID="tbxAlertValue"></asp:Label>s
            <br />
            <asp:TextBox ID="tbxAlertValue" runat="server"></asp:TextBox>
            <asp:RegularExpressionValidator ID="revAlertValue" runat="server" ControlToValidate="tbxAlertValue" CssClass="text-danger" Display="Dynamic" ErrorMessage="Alert Value must have at least one digit left and exactly two digits right of the decimal point." ValidationExpression="[\d]+[.][\d]{2}"></asp:RegularExpressionValidator>
            <asp:RequiredFieldValidator ID="rfvAlertValue" runat="server" ControlToValidate="tbxAlertValue" CssClass="text-danger" ErrorMessage="Alert Value is required." Display="Dynamic"></asp:RequiredFieldValidator>
            <br />
            <asp:Button ID="btnCreateAlert" runat="server" Text="Create Alert" OnClick="CreateAlertClick" />
        </div>
    </div>
</asp:Content>