<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProfitLossTracking.aspx.cs" Inherits="HKeInvestWebApplication.ProfitLossTracking" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Profit Loss</h1>
    <div class="container">
        <div class="form-horizontal">
            <div class="form-group">
                <asp:Label ID="ValueProfitLoss" runat="server" Text="Value Profit/Loss should be in"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Value is required" ControlToValidate="ValueListType">*</asp:RequiredFieldValidator>
                <asp:RadioButtonList ID="ValueListType" runat="server">
                    <asp:ListItem>Dollar Amount</asp:ListItem>
                    <asp:ListItem>Percent</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            
            <div class="form-group">
                <asp:Label ID="SearchType" runat="server" Text="Search type"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Search type is required" ControlToValidate="SearchTypeList">*</asp:RequiredFieldValidator>
                <asp:RadioButtonList ID="SearchTypeList" runat="server" AutoPostBack="True" OnSelectedIndexChanged="SearchTypeList_SelectedIndexChanged">
                    <asp:ListItem Value="individualSecurity">Individual Security</asp:ListItem>
                    <asp:ListItem Value="allSecuritiesOfType">All Securities of Type</asp:ListItem>
                    <asp:ListItem Value="allSecurities">All Securities</asp:ListItem>
                </asp:RadioButtonList>
            </div>

                
            <div class="form-group">
                <asp:Label ID="SecurityType" runat="server" Text="Security Type"></asp:Label>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Value is required" ControlToValidate="SearchTypeList">*</asp:RequiredFieldValidator>
                <br />
                <asp:DropDownList ID="SecurityTypeList" runat="server">
                  <asp:ListItem Value="stock">Stock</asp:ListItem>
                  <asp:ListItem Value="bond">Bond</asp:ListItem>
                  <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
                </asp:DropDownList>
                <br />
            </div>
            
            <div class="form-group">
                <asp:Label ID="SecurityCodeLbl" runat="server" Text="Security Code"></asp:Label>
                <asp:TextBox ID="SecurityCode" runat="server"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="Please enter digits only" ControlToValidate="SecurityCode">*</asp:RegularExpressionValidator>
                
                <asp:CustomValidator ID="CustomValidator1" runat="server" CssClass="text-danger" EnableClientScript="False" ErrorMessage="*" OnServerValidate="CustomValidator1_ServerValidate"></asp:CustomValidator>
                <asp:Label ID="InvalidCode" runat="server" CssClass="text-danger"></asp:Label>
                
            </div>

            <div class="form-group">
                <asp:GridView ID="SingleSecurity" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Security Type" ReadOnly="True" />
                        <asp:BoundField HeaderText="Security Name" ReadOnly="True" />
                        <asp:BoundField HeaderText="Security Code" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Shares Held" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="$ for Buying" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="$ from Selling" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Total Fees Paid" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Profit/Loss" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="form-group">
                <asp:GridView ID="SecuritiesGivenType" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Shares Held" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="$ for Buying" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="$ from Selling" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Total Fees Paid" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Profit/Loss" ReadOnly="True" />
                    </Columns>
                </asp:GridView>
            </div>

            <div class="form-group">
                <asp:GridView ID="SecuritiesAll" runat="server" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Shares Held" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="$ for Buying" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="$ from Selling" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Total Fees Paid" ReadOnly="True" />
                        <asp:BoundField DataFormatString="{0:n2}" HeaderText="Profit/Loss" ReadOnly="True" />
                    </Columns>
                </asp:GridView>

                <div>
                    <asp:Button ID="ShowProfitLoss" CssClass="btn btn-default" runat="server" OnClick="ShowProfitLoss_Click" Text="Show Profit/Loss" />
                </div>
                
            </div>
            
            
        </div>
    </div>


</asp:Content>
