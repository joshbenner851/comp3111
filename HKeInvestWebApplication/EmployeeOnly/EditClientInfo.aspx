<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="EditClientInfo.aspx.cs" Inherits="HKeInvestWebApplication.EditClientInfo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container">
        <h4>Account Application</h4>
        <p>To open your HKeInvest account, please complete all sections below.</p>
        <asp:ValidationSummary EnableClientScript="false" runat="server" CssClass="text-danger" />
        <div class="form-horizontal">
            <div class="row">
                <h4>Part 1:</h4>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label"  runat="server" Text="AccountNumber" AssociatedControlID="AccountNumber"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control"  ID="AccountNumber" runat="server" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="AccountNumber" runat="server" ErrorMessage="Please type AccountNumber" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Account Type"></asp:Label>
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" OnSelectedIndexChanged="RadioButtonList1_SelectedIndexChanged">
                            <asp:ListItem Value="individual">Individual</asp:ListItem>
                            <asp:ListItem Value="survivorship">Joint Tenants with Right of Survivorship</asp:ListItem>
                            <asp:ListItem Value="common">Joint Tenants in Common</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <asp:RequiredFieldValidator ControlToValidate="RadioButtonList1" runat="server" ErrorMessage="Please select account type" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                </div>
                <hr />
            </div>

            <div >
                <h4>Part 2:</h4>
                <div class="row">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Title" AssociatedControlID="cbTitle"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList  ID="cbTitle" runat="server" Height="22px" RepeatDirection="Horizontal" Width="353px">
                            <asp:ListItem Value="Mr">Mr.</asp:ListItem>
                            <asp:ListItem Value="Mrs">Mrs.</asp:ListItem>
                            <asp:ListItem Value="Ms">Ms.</asp:ListItem>
                            <asp:ListItem Value="Dr">Dr.</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="cbTitle" runat="server" ErrorMessage="Please select title" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label"  runat="server" Text="First Name" AssociatedControlID="FirstName"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control"  ID="FirstName" runat="server" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="FirstName" runat="server" ErrorMessage="Please type First Name" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Last Name" AssociatedControlID="LastName"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="LastName" runat="server" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="LastName" runat="server" ErrorMessage="Please select Last Name" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="DateOfBirth" Text="Date of Birth"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control" ID="DateOfBirth" runat="server" MaxLength="10"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="DateOfBirth" runat="server" ErrorMessage="Please enter date of birth" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="Email" Text="Email"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="Email" runat="server" MaxLength="30"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="Email" runat="server" ErrorMessage="Please type email" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="Building" Text="Building"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control"  ID="Building" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="Building" runat="server" ErrorMessage="Please enter building" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="Street" Text="Street"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control"  ID="Street" runat="server" MaxLength="35"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="Street" runat="server" ErrorMessage="Please enter street" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="District" Text="District"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control"  ID="District" runat="server" MaxLength="19"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="District" runat="server" ErrorMessage="Please enter district" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Home Phone" AssociatedControlID="HomePhone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="HomePhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="HomePhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="Home phone must contain only digits"></asp:RegularExpressionValidator>
                         </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Home Fax" AssociatedControlID="HomeFax"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="HomeFax" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="HomeFax" Display="Dynamic" EnableClientScript="false" ErrorMessage="Home fax must contain only digits"></asp:RegularExpressionValidator>
                        </div>
                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Business Phone" AssociatedControlID="BusinessPhone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="BusinessPhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="BusinessPhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="Business phone must contain only digits"></asp:RegularExpressionValidator>
                       </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Mobile Phone" AssociatedControlID="MobilePhone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="MobilePhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="MobilePhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="Mobile phone must contain only digits"></asp:RegularExpressionValidator>
                        </div>
                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Country of Citizenship" AssociatedControlID="CitizenshipCountry"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="CitizenshipCountry" runat="server" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="CitizenshipCountry" runat="server" ErrorMessage="Please enter country of citizenship" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Country of Legal Residence" AssociatedControlID="ResidenceCountry"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="ResidenceCountry" runat="server" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="ResidenceCountry" runat="server" ErrorMessage="Please enter country of residence" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="HKID" Text="HKID/Passport No."></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="HKID" runat="server" MaxLength="8"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="HKID" runat="server" ErrorMessage="Please enter HKID/Passport number" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="PassportCountry" Text="Passport Country of Issue"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="PassportCountry" runat="server" MaxLength="70"></asp:TextBox>
                        <asp:RequiredFieldValidator ControlToValidate="PassportCountry" runat="server" ErrorMessage="Please enter passport country of issue" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <hr />


                <h4>Part Co-Account 2:</h4>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Title" AssociatedControlID="COcbTitle"></asp:Label>
                    <div class="col-md-4">
                        <asp:RadioButtonList  ID="COcbTitle" runat="server" Height="22px" RepeatDirection="Horizontal" Width="353px">
                            <asp:ListItem Value="Mr">Mr.</asp:ListItem>
                            <asp:ListItem Value="Mrs">Mrs.</asp:ListItem>
                            <asp:ListItem Value="Ms">Ms.</asp:ListItem>
                            <asp:ListItem Value="Dr">Dr.</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="First Name" AssociatedControlID="COFirstName"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COFirstName" runat="server" MaxLength="35"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Last Name" AssociatedControlID="COLastName"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COLastName" runat="server" MaxLength="35"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="CODateOfBirth" Text="Date of Birth"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="CODateOfBirth" runat="server" MaxLength="10"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COEmail" Text="Email"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COEmail" runat="server" MaxLength="30"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COBuilding" Text="Building"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COBuilding" runat="server" MaxLength="50"></asp:TextBox>
                    </div>

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COStreet" Text="Street"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COStreet" runat="server" MaxLength="35"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="CODistrict" Text="District"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="CODistrict" runat="server" MaxLength="19"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Home Phone" AssociatedControlID="COHomePhone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COHomePhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="COHomePhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="CO: Home phone must contain only digits"></asp:RegularExpressionValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Home Fax" AssociatedControlID="COHomeFax"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COHomeFax" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="COHomeFax" Display="Dynamic" EnableClientScript="false" ErrorMessage="CO: Home fax must contain only digits"></asp:RegularExpressionValidator>
                    </div>

                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Business Phone" AssociatedControlID="COBusinessPhone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control" ID="COBusinessPhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="COBusinessPhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="CO: Business phone must contain only digits"></asp:RegularExpressionValidator>
                    </div>

                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Mobile Phone" AssociatedControlID="COMobilePhone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control" ID="COMobilePhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="COMobilePhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="CO: Mobile phone must contain only digits"></asp:RegularExpressionValidator>
                    </div>

                </div>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Country of Citizenship" AssociatedControlID="COCitizenshipCountry"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COCitizenshipCountry" runat="server" MaxLength="70"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Country of Legal Residence" AssociatedControlID="COResidenceCountry"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COResidenceCountry" runat="server" MaxLength="70"></asp:TextBox>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COHKID" Text="HKID/Passport No."></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COHKID" runat="server" MaxLength="8"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COPassportCountry" Text="Passport Country of Issue"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COPassportCountry" runat="server" MaxLength="70"></asp:TextBox>
                    </div>

                </div>
                <hr />

            </div>

            <div class="row">
                <h4>Part 3:</h4>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Employment Status" AssociatedControlID="cbEmploymentStatus"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList  ID="cbEmploymentStatus" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Employed">Employed</asp:ListItem>
                            <asp:ListItem Value="SelfEmployed">Self-Employed</asp:ListItem>
                            <asp:ListItem Value="Retired">Retired</asp:ListItem>
                            <asp:ListItem Value="Student">Student</asp:ListItem>
                            <asp:ListItem Value="NotEmployed">Not Employed</asp:ListItem>
                            <asp:ListItem Value="Homemaker">Homemaker</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="cbEmploymentStatus" runat="server" ErrorMessage="Please select employment status" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="SpecificOccupation" Text="Specific Occupation"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control" ID="SpecificOccupation" runat="server" MaxLength="20"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="EmployYears" Text="Years With Employer"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="EmployYears" runat="server" MaxLength="2" TextMode="Number"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{0,2}$" ControlToValidate="EmployYears" Display="Dynamic" EnableClientScript="false" ErrorMessage="Employ years must contain only digits"></asp:RegularExpressionValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="EmployName" Text="Employer Name"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="EmployName" runat="server" MaxLength="25"></asp:TextBox>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="EmployPhone" Text="Employer Phone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="EmployPhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="EmployPhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="Employment phone must contain only digits"></asp:RegularExpressionValidator>
                    </div>
                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="BusinessNature" Text="Nature of Business"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="BusinessNature" runat="server" MaxLength="20"></asp:TextBox>
                    </div>

                </div>
                <hr />

                <h4>Part Co-Account 3:</h4>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" Text="Employment Status" AssociatedControlID="COcbEmploymentStatus"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList  ID="COcbEmploymentStatus" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Employed">Employed</asp:ListItem>
                            <asp:ListItem Value="SelfEmployed">Self-Employed</asp:ListItem>
                            <asp:ListItem Value="Retired">Retired</asp:ListItem>
                            <asp:ListItem Value="Student">Student</asp:ListItem>
                            <asp:ListItem Value="NotEmployed">Not Employed</asp:ListItem>
                            <asp:ListItem Value="Homemaker">Homemaker</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COSpecificOccupation" Text="Specific Occupation"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control" ID="COSpecificOccupation" runat="server" MaxLength="20"></asp:TextBox>
                    </div>

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COEmployYears" Text="Years With Employer"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COEmployYears" runat="server" MaxLength="2" TextMode="Number"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{0,2}$" ControlToValidate="COEmployYears" Display="Dynamic" EnableClientScript="false" ErrorMessage="CO: Employ years must contain only digits"></asp:RegularExpressionValidator>
                    </div>


                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COEmployName" Text="Employer Name"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COEmployName" runat="server" MaxLength="25"></asp:TextBox>
                    </div>

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COEmployPhone" Text="Employer Phone"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COEmployPhone" runat="server" MaxLength="8" TextMode="Phone"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{8}$" ControlToValidate="COEmployPhone" Display="Dynamic" EnableClientScript="false" ErrorMessage="CO: Employment phone must contain only digits"></asp:RegularExpressionValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COBusinessNature" Text="Nature of Business"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox CssClass="form-control"  ID="COBusinessNature" runat="server" MaxLength="20"></asp:TextBox>
                    </div>


                </div>
                <hr />

            </div>

            <div class="row">
                <h4>Part 4:</h4>
                <div class="form-group">

                    <asp:Label CssClass="col-md-4 control-label" runat="server" AssociatedControlID="IsEmployedFinancial" Text="Are you employed by a registered securities broker/dealer, investment advisor, bank or other financal institustion?"></asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList   ID="IsEmployedFinancial" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>

                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="IsEmployedFinancial" runat="server" ErrorMessage="Please enter employed in financial sector" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-4 control-label" runat="server" AssociatedControlID="IsInIPO" Text="Are you a director, 10% shareholder or policy-making officer of a publicly traded company?"></asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList   ID="IsInIPO" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>

                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="IsInIPO" runat="server" ErrorMessage="Please enter IPO status" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>

                <div class="form-group">

                    <asp:Label CssClass="col-md-4 control-label" runat="server" AssociatedControlID="PrimarySource" Text="Describe the primary source of funds deposited to this account"></asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList  ID="PrimarySource" runat="server" OnSelectedIndexChanged="PrimarySource_SelectedIndexChanged" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Salary">Salary/Wages/Savings</asp:ListItem>
                            <asp:ListItem Value="Investment">Investment/Capital Gains</asp:ListItem>
                            <asp:ListItem Value="Family">Family/Relatives/Inheritance</asp:ListItem>
                            <asp:ListItem Value="Other">Other</asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="PrimarySource" runat="server" ErrorMessage="Please enter primary source of funds" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="OtherInformation" Text="Other information"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control" ID="OtherInformation" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                </div>
                <hr />

                <h4>Part Co-Account 4:</h4>
                <div class="form-group">

                    <asp:Label CssClass="col-md-4 control-label" runat="server" AssociatedControlID="COIsEmployedFinancial" Text="Are you employed by a registered securities broker/dealer, investment advisor, bank or other financal institustion?"></asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList  ID="COIsEmployedFinancial" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>

                        </asp:RadioButtonList>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-4 control-label" runat="server" AssociatedControlID="COIsInIPO" Text=" Are you a director, 10% shareholder or policy-making officer of a publicly traded company?"></asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList  ID="COIsInIPO" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>

                        </asp:RadioButtonList>
                    </div>

                </div>
                <hr />

            </div>

            <div class="row">
                <h4 class="left">Part 5:</h4>
                <div class="form-group">
                    <asp:Label CssClass="col-md-4 control-label" runat="server" Text="Investment Objective" AssociatedControlID="InvestmentObjective"></asp:Label>
                    <div class="col-md-8">
                        <asp:RadioButtonList ID="InvestmentObjective" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="CapitalPreservation"></asp:ListItem>
                            <asp:ListItem Value="Income"></asp:ListItem>
                            <asp:ListItem Value="Growth"></asp:ListItem>
                            <asp:ListItem Value="Speculation"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="InvestmentObjective" runat="server" ErrorMessage="Please select the investment objective." Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>
                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="InvestmentKnowledge" Text="Investment Knowledge"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList   ID="InvestmentKnowledge" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Novice"></asp:ListItem>
                            <asp:ListItem Value="Limited"></asp:ListItem>
                            <asp:ListItem Value="Good"></asp:ListItem>
                            <asp:ListItem Value="Extensive"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="InvestmentKnowledge" runat="server" ErrorMessage="Please enter the investment knowledge" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="InvestmentExperience" Text="Investment Experience"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList  ID="InvestmentExperience" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Novice"></asp:ListItem>
                            <asp:ListItem Value="Limited"></asp:ListItem>
                            <asp:ListItem Value="Good"></asp:ListItem>
                            <asp:ListItem Value="Extensive"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="InvestmentExperience" runat="server" ErrorMessage="Please select the level of investment experience" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="AnnualIncome" Text="Annual Income"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList ID="AnnualIncome" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Under HK$20,000"></asp:ListItem>
                            <asp:ListItem Value="$20,001-HK$200,000"></asp:ListItem>
                            <asp:ListItem Value="HK$200,001-HK$2,000,000"></asp:ListItem>
                            <asp:ListItem Value="Greater than HK$2,000,000"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="AnnualIncome" runat="server" ErrorMessage="Please select the annual income" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="LiquidWorth" Text="Approximate Liquid Net Worth"></asp:Label>
                    <div class="col-md-10">
                        <asp:RadioButtonList  ID="LiquidWorth" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="Under HK$100,000"></asp:ListItem>
                            <asp:ListItem Value="HK$100,001-HK$1,000,000"></asp:ListItem>
                            <asp:ListItem Value="HK$1,000,001-HK$10,000,000"></asp:ListItem>
                            <asp:ListItem Value="Greater than HK$10,000,000"></asp:ListItem>
                        </asp:RadioButtonList>
                        <asp:RequiredFieldValidator ControlToValidate="LiquidWorth" runat="server" ErrorMessage="Please enter the liquid worth" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <hr />
            </div>

            <div class="row">
                <h4>Part 6:</h4>
                <div class="form-group">

                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="FreeCreditSwee" Text="Earn Income on Cash Balance (Free Credit Sweep)"></asp:Label>
                    <div class="col-md-4">
                        <asp:RadioButtonList ID="FreeCreditSwee" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Value="1">Yes</asp:ListItem>
                            <asp:ListItem Value="0">No</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </div>
                <hr />
            </div>

            <div class="row">
                <h4>Part 7:</h4>
                <div class="form-group">
                    <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="DepositAmount" Text="Initial Cheque Amount"></asp:Label>
                    <div class="col-md-4">
                        <asp:TextBox  CssClass="form-control" runat="server" ID="DepositAmount"></asp:TextBox>
                        <asp:RegularExpressionValidator runat="server" ValidationExpression="^[\d]{0,12}$" ControlToValidate="DepositAmount" Display="Dynamic" EnableClientScript="false" ErrorMessage="Cheque amount must contain only digits"></asp:RegularExpressionValidator>
                    </div>
                    
                </div>
                <hr />
            </div>
        </div>
        <div class="row">
            <h4>Part 8:</h4>
            <div  class="form-group">
            <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="SignedOn" Text="Owner Signed On"></asp:Label>
            <div class="col-md-4">
                <asp:TextBox CssClass="form-control"  runat="server" ID="SignedOn"></asp:TextBox>
                <asp:RequiredFieldValidator ControlToValidate="SignedOn" runat="server" ErrorMessage="Please enter the date which the owner signed" Text="*" Display="Dynamic" CssClass="text-danger" EnableClientScript="false"></asp:RequiredFieldValidator>
            </div>
            
                <asp:Label CssClass="col-md-2 control-label" runat="server" AssociatedControlID="COSignedOn" Text="Co-Owner Signed On"></asp:Label>
            <div class="col-md-4">    
            <asp:TextBox  CssClass="form-control" runat="server" ID="COSignedOn"></asp:TextBox>
            </div>
                </div>
        </div>
        <div class="row">
            <div class="col-md-4">
                <div class="form-group">
                    <div class="col-md-offset-2 col-md-10">
                        <asp:Button runat="server" OnClick="CreateClient_Click" Text="Create Client" CssClass="btn btn-default" />
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>
