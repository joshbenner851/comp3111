<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SecurityTransaction.aspx.cs" Inherits="HKeInvestWebApplication.EmployeeOnly.SecurityTransaction" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Security Transaction</h2>
    <div class="container">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label CssClass="control-label" runat="server" Text="Type of Security" AssociatedControlID="SecurityType"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="SecurityType" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Type of Security is required">*</asp:RequiredFieldValidator>
                <asp:RadioButtonList ID="SecurityType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SecurityType_SelectedIndexChanged">
                    <asp:ListItem>Bond</asp:ListItem>
                    <asp:ListItem>Unit Trust</asp:ListItem>
                    <asp:ListItem>Stock</asp:ListItem>
                </asp:RadioButtonList>
                

            </div>

            
            <div class="form-group">
                <asp:Label AssociatedControlID="TransactionType" runat="server" Text="Type of Transaction" EnableTheming="True"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TransactionType" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Type of Transaction is required">*</asp:RequiredFieldValidator>
                
            <asp:RadioButtonList ID="TransactionType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TransactionType_SelectedIndexChanged">
                <asp:ListItem>Buy</asp:ListItem>
                <asp:ListItem>Sell</asp:ListItem>
            </asp:RadioButtonList>
                
            </div>
            
           
            
           
            <div id="bondtrust" runat="server" style="display:none;"> 
                 <div class="form-group">
                     <asp:Label AssociatedControlID="BondTrustCode" runat="server" Text="Bond/Trust Code:"></asp:Label>
                    <asp:TextBox ID="BondTrustCode" runat="server"></asp:TextBox>
                    <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="BondTrustCode" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits" CssClass="text-danger"></asp:RegularExpressionValidator>
                    <asp:Label ID="InvalidBondTrustCode" runat="server" CssClass="text-danger"></asp:Label>
                 </div>
                
               <div class="form-group" >
                   <div id="buyBondTrust" runat="server" style="display:none;">
                        <asp:Label AssociatedControlID="BondTrustSharesQuantity" runat="server" Text="Dollar amount to be bought(HKD):"></asp:Label>
                        <asp:TextBox ID="BondTrustSharesQuantity" runat="server" TextMode="Number"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="BondTrustSharesQuantity" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits" CssClass="text-danger"></asp:RegularExpressionValidator>
                         <asp:CustomValidator ID="CustomValidator3" runat="server" ControlToValidate="BondTrustSharesQuantity" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Must be a positive number" OnServerValidate="CustomValidator3_ServerValidate" ValidateEmptyText="True">*</asp:CustomValidator>
                        <asp:Label ID="InvalidBondTrustSharesQuantity" runat="server" CssClass="text-danger"></asp:Label>
                   </div>
               </div>
                
                <div class="form-group">
                    <div id="sellBondTrust" runat="server" style="display:none;">
                        <asp:Label AssociatedControlID="BondTrustSharesSelling" runat="server" Text="Quantity of shares to sell:"></asp:Label>
                     <asp:TextBox ID="BondTrustSharesSelling" runat="server" TextMode="Number"></asp:TextBox>
                         <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="BondTrustSharesSelling" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits" CssClass="text-danger"></asp:RegularExpressionValidator>
                         <asp:CustomValidator ID="CustomValidator2" runat="server" ControlToValidate="BondTrustSharesSelling" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Must be a positive number" OnServerValidate="CustomValidator2_ServerValidate" ValidateEmptyText="True">*</asp:CustomValidator>
                        <asp:Label ID="InvalidBondTrustSharesSelling" runat="server" CssClass="text-danger"></asp:Label>
                    </div>
                </div>
                
                </div>
            
                <div id="stock" runat="server" style="display:none;">
               
                <div class="form-group">
                    <asp:Label AssociatedControlID="StockCode" runat="server" Text="Stock Code:"></asp:Label>
                    <asp:TextBox ID="StockCode" runat="server" TextMode="Number"></asp:TextBox>
                     <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="StockCode" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits" CssClass="text-danger"></asp:RegularExpressionValidator>
                    <asp:Label ID="InvalidStockCode" runat="server" Text="" CssClass="text-danger"></asp:Label>
                </div>
               
                <div class="form-group">
                    <asp:Label AssociatedControlID="OrderType" runat="server" Text="Type of Security"></asp:Label>
                    <asp:RadioButtonList ID="OrderType" runat="server" AutoPostBack="True" OnSelectedIndexChanged="OrderType_SelectedIndexChanged">
                        <asp:ListItem>Market Order</asp:ListItem>
                        <asp:ListItem>Limit Order</asp:ListItem>
                        <asp:ListItem>Stop Order</asp:ListItem>
                        <asp:ListItem>Stop Limit Order</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
               

                <div id="market" runat="server" style="display:none;">

                </div>
                <div class="form-group">
                    <div id="limit" runat="server" style="display:none;">
                        <asp:Label AssociatedControlID="LimitPrice" runat="server" Text="Price to buy/sell at:"></asp:Label>
                        <asp:TextBox ID="LimitPrice" runat="server" TextMode="Number"></asp:TextBox>
                     <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="LimitPrice" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits"></asp:RegularExpressionValidator>
                        
                    </div>
                </div>
                 
                <div class="form-group">
                    <div id="stop" runat="server" style="display:none;">
                        <asp:Label AssociatedControlID="StopPrice" runat="server" Text="Price to stop at:"></asp:Label>
                        <asp:TextBox ID="StopPrice" runat="server" TextMode="Number"></asp:TextBox>
                     <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="StopPrice" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits"></asp:RegularExpressionValidator>
                        
                    </div>
                </div>
                
                
                <div id="stoplimit" runat="server" style="display:none;">
        
                </div>
                <div class="form-group">
                     <asp:Label AssociatedControlID="StockSharesQuantity" runat="server" Text="Shares Quantity:"></asp:Label>
                     <asp:TextBox ID="StockSharesQuantity" runat="server" TextMode="Number"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ControlToValidate="StockSharesQuantity" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Must be in multiples of 100" OnServerValidate="CustomValidator1_ServerValidate" ValidateEmptyText="True">*</asp:CustomValidator>
                     <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]+$" ControlToValidate="StockSharesQuantity" Display="Dynamic" EnableClientScript="false" ErrorMessage="Must contain only digits"></asp:RegularExpressionValidator>
                    <asp:Label ID="InvalidStockSharesQuantity" runat="server" CssClass="text-danger"></asp:Label>
                </div>
               


                <asp:DropDownList ID="DaysUntilExpiration" runat="server" AutoPostBack="True">
                    <asp:ListItem Value="0">Days until Expiration</asp:ListItem>
                    <asp:ListItem>1</asp:ListItem>
                    <asp:ListItem>2</asp:ListItem>
                    <asp:ListItem>3</asp:ListItem>
                    <asp:ListItem>4</asp:ListItem>
                    <asp:ListItem>5</asp:ListItem>
                    <asp:ListItem>6</asp:ListItem>
                    <asp:ListItem>7</asp:ListItem>
                </asp:DropDownList>
                <asp:RadioButton ID="AllOrNone" runat="server" Text="All or None" />


            </div>
            <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        
                        <asp:Button runat="server"  Text="Execute Transaction" CssClass="btn btn-default" OnClick="ExecuteOrderClick" />
                    </div>
            </div>
        </div>
    </div>
</asp:Content>
