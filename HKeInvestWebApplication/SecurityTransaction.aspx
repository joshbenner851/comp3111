<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityTransaction.aspx.cs" Inherits="HKeInvestWebApplication.EmployeeOnly.SecurityTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Transaction</h2>
    <div>
        <div>
            <asp:Label CssClass="control-label" runat="server" Text="Type of Security" AssociatedControlID="SecurityType"></asp:Label>
            <asp:RadioButtonList ID="SecurityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SecurityType_SelectedIndexChanged">
                <asp:ListItem>Bond/Unit Trust</asp:ListItem>
                <asp:ListItem>Stock</asp:ListItem>
            </asp:RadioButtonList>
            <asp:Label AssociatedControlID="TransactionType" runat="server" Text="Type of Security"></asp:Label>
            <asp:RadioButtonList ID="TransactionType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TransactionType_SelectedIndexChanged">
                <asp:ListItem>Buy</asp:ListItem>
                <asp:ListItem>Sell</asp:ListItem>
            </asp:RadioButtonList>

            <div id="bondtrust" runat="server">
                <asp:Label AssociatedControlID="BondTrustCode" runat="server" Text="Bond/Trust Code:"></asp:Label>
                <asp:TextBox ID="BondTrustCode" runat="server"></asp:TextBox>
               
                <div id="buyBondTrust" runat="server">
                    <asp:Label AssociatedControlID="SharesBuying" runat="server" Text="Amount to be bought:"></asp:Label>
                    <asp:TextBox ID="SharesBuying" runat="server"></asp:TextBox>
                </div>
                <div id="sellBondTrust" runat="server">
                    <asp:Label AssociatedControlID="SharesSelling" runat="server" Text="Bond/Trust Code:"></asp:Label>
                    <asp:TextBox ID="SharesSelling" runat="server"></asp:TextBox>
                </div>
            </div>
            
            <div id="stock" runat="server">
                <asp:Label AssociatedControlID="StockCode" runat="server" Text="Stock Code:"></asp:Label>
                <asp:TextBox ID="StockCode" runat="server"></asp:TextBox>
                <asp:Label AssociatedControlID="OrderType" runat="server" Text="Type of Security"></asp:Label>
                <asp:RadioButtonList ID="OrderType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OrderType_SelectedIndexChanged">
                    <asp:ListItem>Market Order</asp:ListItem>
                    <asp:ListItem>Limit Order</asp:ListItem>
                    <asp:ListItem>Stop Order</asp:ListItem>
                    <asp:ListItem>Stop Limit Order</asp:ListItem>
                </asp:RadioButtonList>

                <div id="market" runat="server">

                </div>
               
                 <div id="limit" runat="server">

                </div>
                
                <div id="stop" runat="server">

                </div>
                
                <div id="stoporder" runat="server">

                </div>

                <asp:Label AssociatedControlID="SharesQuantity" runat="server" Text="Shares Quantity:"></asp:Label>
                <asp:TextBox ID="SharesQuantity" runat="server"></asp:TextBox>


                <asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="0">Days until Expiration</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                </asp:DropDownList>
                <asp:RadioButton ID="RadioButton1" runat="server" Text="All or None" />


            </div>
            <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server"  Text="Execute Transaction" CssClass="btn btn-default" />
                    </div>
            </div>
        </div>
    </div>
</asp:Content>
