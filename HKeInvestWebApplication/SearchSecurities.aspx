<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SearchSecurities.aspx.cs" Inherits="HKeInvestWebApplication.SearchSecurities" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Search Securities</h2>

    <div>
        <div>
            <asp:DropDownList ID="ddlSecurityType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSecurityType_SelectedIndexChanged">
                <asp:ListItem Value="0">Security Type</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="unit trust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
            <asp:Label runat="server" AssociatedControlID="SecurityInput" Text="Security Name/Code"></asp:Label>
            <asp:TextBox ID="SecurityInput" runat="server" AutoPostBack="True" OnTextChanged="SecurityInput_TextChanged"></asp:TextBox>
            <br />
        </div>
        <div>
            <asp:GridView ID="gvSearchStock" runat="server" Visible="False" AllowSorting="True" AutoGenerateColumns="False" OnSorting="gvSearchStock_Sorting">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code"/>
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="close" HeaderText="Close(HKD)" ReadOnly="True" />
                    <asp:BoundField DataField="changeDollar" HeaderText="Change(HKD)" ReadOnly="True" />
                    <asp:BoundField DataField="changePercent" HeaderText="Change(%)" ReadOnly="True" />
                    <asp:BoundField DataField="volume" HeaderText="Volume" ReadOnly="True" />
                    <asp:BoundField DataField="high" HeaderText="High" ReadOnly="True" />
                    <asp:BoundField DataField="low" HeaderText="Low" ReadOnly="True" />
                    <asp:BoundField DataField="peRatio" HeaderText="P/E Ratio" ReadOnly="True" />
                    <asp:BoundField DataField="yield" HeaderText="Yield" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvSearchBond" runat="server" Visible="False" AllowSorting="true" AutoGenerateColumns="False" OnSorting="gvSearchBond_Sorting">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="launchDate" HeaderText="Launch Date" ReadOnly="True" />
                    <asp:BoundField DataField="base" HeaderText="Base Currency" ReadOnly="True" />
                    <asp:BoundField DataField="price" HeaderText="Value(Base)" ReadOnly="True" />
                    <asp:BoundField DataField="sixMonths" HeaderText="6 Months" ReadOnly="True" />
                    <asp:BoundField DataField="oneYear" HeaderText="1 Year" ReadOnly="True" />
                    <asp:BoundField DataField="threeYears" HeaderText="3 Years" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <asp:GridView ID="gvSearchUnitTrust" runat="server" Visible="False" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvSearchUnitTrust_Sorting">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="launchDate" HeaderText="Launch Date" ReadOnly="True" />
                    <asp:BoundField DataField="base" HeaderText="Base Currency" ReadOnly="True" />
                    <asp:BoundField DataField="size" HeaderText="Size" ReadOnly="True" />
                    <asp:BoundField DataField="price" HeaderText="Price" ReadOnly="True" />
                    <asp:BoundField DataField="riskReturn" HeaderText="Risk/Return Rating" ReadOnly="True" />
                    <asp:BoundField DataField="sixMonths" HeaderText="6 Months" ReadOnly="True" />
                    <asp:BoundField DataField="oneYear" HeaderText="1 Year" ReadOnly="True" />
                    <asp:BoundField DataField="threeYears" HeaderText="3 Years" ReadOnly="True" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="ErrorLabel" runat="server" Visible="False" CssClass="text-danger"></asp:Label>
        </div>
    </div>
</asp:Content>