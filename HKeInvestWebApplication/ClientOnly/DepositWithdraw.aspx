<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepositWithdraw.aspx.cs" Inherits="HKeInvestWebApplication.DepositWithdraw" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h4>Deposit or Withdraw</h4>
        <asp:ValidationSummary EnableClientScript="false" runat="server" CssClass="text-danger" />
        <asp:Label ID="IncorrectAmount" runat="server" CssClass="text-danger"></asp:Label>
        <div class="form-horizontal">
            <div class="row">
                <asp:RadioButtonList ID="RadioButtonList1" runat="server" AutoPostBack="True" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                    <asp:ListItem>Deposit</asp:ListItem>
                    <asp:ListItem>Withdraw</asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="row">
                <!--Add in form stuff -->
                <div class="col-md-6" id="depositInfo" style="display:none;" runat="server">
                    <asp:Label CssClass="control-label" AssociatedID="Deposit" runat="server" Text="Amount you wish to Deposit"></asp:Label>
                    <asp:TextBox CssClass="form-control" ID="Deposit" runat="server"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator1" runat="server" ValidateEmptyText="True" ControlToValidate="Deposit" OnServerValidate="CustomValidator1_ServerValidate" EnableClientScript="False"></asp:CustomValidator>
                </div>
                <div class="col-md-6" id="withdrawInfo" style="display:none;" runat="server">
                    <asp:Label CssClass="control-label" AssociatedID="Withdraw" runat="server" Text="Amount you wish to withdraw"></asp:Label>
                    <asp:TextBox CssClass="form-control" ID="Withdraw" runat="server"></asp:TextBox>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ValidateEmptyText="True" ControlToValidate="Withdraw" OnServerValidate="CustomValidator2_ServerValidate" EnableClientScript="False"></asp:CustomValidator>
                </div>
            </div>
        </div>
        <div>
            <asp:Button ID="ExecuteDepositWithdraw" CssClass="btn btn-default" runat="server"  Text="Deposit or Withdraw" OnClick="ExecuteDepositWithdraw_Click" />
        </div>
    </div>

</asp:Content>
