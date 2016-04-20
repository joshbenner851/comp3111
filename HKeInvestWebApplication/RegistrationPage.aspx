<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegistrationPage.aspx.cs" Inherits="HKeInvestWebApplication.RegistrationPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Create a new client login account</h2>
    <div class="form-horizontal">
        <asp:ValidationSummary ID="ValidateForm" ShowSummary="true" runat="server" CssClass="text-danger" EnableClientScript="False" />
        <div class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" Text="First Name" AssociatedControlID="FirstName"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="FirstName" runat="server" MaxLength="35"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="FirstName" CssClass="text-danger" runat="server" ErrorMessage="First Name is required." EnableClientScript="false"></asp:RequiredFieldValidator>
            </div>
            <asp:Label CssClass="col-md-2 control-label" AssociatedControlID="LastName" runat="server" Text="Last Name"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="LastName" runat="server" MaxLength="35" OnTextChanged="LastName_TextChanged"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="LastName" CssClass="text-danger" runat="server" ErrorMessage="Last Name is required." EnableClientScript="false"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Account #" AssociatedControlID="AccountNumber"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="AccountNumber" runat="server" MaxLength="10"></asp:TextBox>
                <asp:CustomValidator Text="*" ID="AccountValidator" runat="server" Display="Dynamic" ErrorMessage="Last name is incorrect" EnableClientScript="False" OnServerValidate="AccountValidator_ServerValidate1" CssClass="text-danger" ControlToValidate="AccountNumber"></asp:CustomValidator>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="AccountNumber" CssClass="text-danger" runat="server" ErrorMessage="Account Number is required." EnableClientScript="false"></asp:RequiredFieldValidator>

            </div>
            <asp:Label CssClass="col-md-2 control-label" AssociatedControlID="HKID" runat="server" Text="HKID/Passport#"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="HKID" runat="server" MaxLength="8"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="HKID" CssClass="text-danger" runat="server" ErrorMessage="HKID is required." EnableClientScript="false"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Date of Birth" AssociatedControlID="DateOfBirth"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="DateOfBirth" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="DateOfBirth" CssClass="text-danger" runat="server" ErrorMessage="Date of Birth is required." EnableClientScript="false"></asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator EnableClientScript="false" Display="Dynamic" Text="*" ID="RegularExpressionValidator1" runat="server" ControlToValidate="DateOfBirth" CssClass="text-danger" ErrorMessage="Date of Birth is not valid." ValidationExpression="((0[13578]|1[02])[\/.](0[1-9]|[12][0-9]|3[01])[\/.](18|19|20)[0-9]{2})|((0[469]|11)[\/.](0[1-9]|[12][0-9]|30)[\/.](18|19|20)[0-9]{2})|((02)[\/.](0[1-9]|1[0-9]|2[0-8])[\/.](18|19|20)[0-9]{2})|((02)[\/.]29[\/.](((18|19|20)(04|08|[2468][048]|[13579][26]))|2000))"></asp:RegularExpressionValidator>

            </div>
            <asp:Label CssClass="col-md-2 control-label" AssociatedControlID="Email" runat="server" Text="Email"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="Email" TextMode="Email" runat="server" MaxLength="30"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="Email" CssClass="text-danger" runat="server" ErrorMessage="Email address is required." EnableClientScript="false"></asp:RequiredFieldValidator>

            </div>
        </div>
        <div class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" Text="User Name" AssociatedControlID="UserName"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="UserName" runat="server" MaxLength="10"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="UserName" CssClass="text-danger" runat="server" ErrorMessage="UserName is required." EnableClientScript="false"></asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator EnableClientScript="false" Display="Dynamic" Text="*" CssClass="text-danger" ID="RegularExpressionValidator2" runat="server" ControlToValidate="UserName" ErrorMessage="Username is not valid" ValidationExpression="^([a-zA-Z0-9]){6,10}$"></asp:RegularExpressionValidator>

            </div>
        </div>
        <div class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Password" AssociatedControlID="Password"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="Password" TextMode="Password" runat="server" MaxLength="15"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="Password" CssClass="text-danger" runat="server" ErrorMessage="Password is required." EnableClientScript="false"></asp:RequiredFieldValidator>

                <asp:RegularExpressionValidator Display="Dynamic" Text="*" ID="RegularExpressionValidator3" runat="server" ControlToValidate="Password" ErrorMessage="Password is not valid" ValidationExpression="^(?=\w*[^\w]\w*[^\w]\w*)(.){8,15}$" CssClass="text-danger"></asp:RegularExpressionValidator>

            </div>
        </div>
        <div class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Confirm Password" AssociatedControlID="ConfirmPassword"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control" ID="ConfirmPassword" TextMode="Password" runat="server" MaxLength="15"></asp:TextBox>
                <asp:RequiredFieldValidator Display="Dynamic" Text="*" ControlToValidate="ConfirmPassword" CssClass="text-danger" runat="server" ErrorMessage="Confirm Password is required." EnableClientScript="false"></asp:RequiredFieldValidator>

                <asp:CompareValidator Display="Dynamic" Text="*" ID="CompareValidator1" runat="server" ControlToCompare="Password" ControlToValidate="ConfirmPassword" ErrorMessage="Password and confirm password do not match." CssClass="text-danger"></asp:CompareValidator>

            </div>
        </div>
        <div class="form-group">
            <div class="col-sm-2 col-sm-offset-5">
                <asp:Button CssClass="form-control" ID="Register" runat="server" Text="Register" />
            </div>
        </div>
    </div>
</asp:Content>
