<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityHoldingDetails.aspx.cs" Inherits="HKeInvestWebApplication.SecurityHoldingDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Holding Details</h2>
    <div>
        <div>
            <asp:Label AssociatedControlID="txtAccountNumber" runat="server" Text="Account Number:"></asp:Label>
            <asp:TextBox ID="txtAccountNumber" runat="server" OnTextChanged="txtAccountNumber_TextChanged"></asp:TextBox>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                <asp:ListItem Value="0">Security Type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddlCurrency" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCurrency_SelectedIndexChanged">
                <asp:ListItem Value="0">Currency</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div>
            <asp:Label ID="lblClientName" runat="server" Visible="false"></asp:Label>
            <asp:Label ID="lblResultMessage" runat="server" Visible="false"></asp:Label>
            
        </div>
        <div>
            <asp:GridView ID="gvSecurityHolding" Visible="False" runat="server" AutoGenerateColumns="False" OnSorting="gvSecurityHolding_Sorting" Width="815px" OnSelectedIndexChanged="gvSecurityHolding_SelectedIndexChanged">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="shares" DataFormatString="{0:n2}" HeaderText="Shares" ReadOnly="True" SortExpression="shares" />
                    <asp:BoundField DataField="base" HeaderText="Base" ReadOnly="True" />
                    <asp:BoundField DataField="price" DataFormatString="{0:n2}" HeaderText="Price" ReadOnly="True" />
                    <asp:BoundField DataField="value" DataFormatString="{0:n2}" HeaderText="Value" ReadOnly="True" SortExpression="value" />
                    <asp:BoundField DataField="convertedValue" DataFormatString="{0:n2}" HeaderText="Value in" ReadOnly="True" SortExpression="convertedValue" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
