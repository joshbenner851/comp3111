<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="GenerateReport.aspx.cs" Inherits="HKeInvestWebApplication.GenerateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Generate Report</h2>
    <div>
        <asp:Label runat="server" Text="Account Number" AssociatedControlID="AccountNumber"></asp:Label><asp:TextBox ID="AccountNumber" runat="server" MaxLength="10" AutoPostBack="True" OnTextChanged="AccountNumber_TextChanged"></asp:TextBox>
    </div>
    <div>
        <asp:Label ID="lblClientName" runat="server" Visible="False"></asp:Label>
    </div>

    <div id="Report">
        <!--Part A-->
        <div id="ClientInfo">
            <h3>Account Summary</h3>
            <hr />
            <asp:Label runat="server" AssociatedControlID="AccountNumber" Text="Account Number: "></asp:Label>
            <asp:Label ID="AccountNum" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="ClientName" Text="Client Name(s): "></asp:Label>
            <asp:Label ID="ClientName" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="TotalValue" Text="Total Monetary Value(HKD): "></asp:Label>
            <asp:Label ID="TotalValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" Text="Free Balance(HKD): " AssociatedControlID="FreeBalance"></asp:Label>
            <asp:Label ID="FreeBalance" runat="server"></asp:Label>

            <br />
            <asp:Label runat="server" AssociatedControlID="StockValue" Text="Total Stock Value(HKD): "></asp:Label>
            <asp:Label ID="StockValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="BondValue" Text="Total Bond Value(HKD):"></asp:Label>
            <asp:Label ID="BondValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="UnitTrustValue" Text="Total Unit Trust Value(HKD): "></asp:Label>
            <asp:Label ID="UnitTrustValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="LastOrderDate" Text="Last Executed Order Date:"></asp:Label>
            <asp:Label ID="LastOrderDate" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="LastOrderValue" Text="Last Executed Order Value:"></asp:Label>
            <asp:Label ID="LastOrderValue" runat="server"></asp:Label>

        </div>

        <!--Part B-->
        <div id="SecuritySummary">
            <h3>Securities Summary</h3>
            <hr />
            <asp:DropDownList ID="securityType" runat="server" OnSelectedIndexChanged="securityType_SelectedIndexChanged" AutoPostBack="True">
                <asp:ListItem Value="stock">Stock</asp:ListItem>
                <asp:ListItem Value="bond">Bond</asp:ListItem>
                <asp:ListItem Value="unitTrust">Unit Trust</asp:ListItem>
            </asp:DropDownList>
            <asp:GridView ID="gvSecurities" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvSecurities_Sorting">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" />
                    <asp:BoundField DataField="shares" ReadOnly="True" HeaderText="Shares Held" SortExpression="shares" />
                    <asp:BoundField DataField="price" HeaderText="Price Per Share" ReadOnly="True" />
                    <asp:BoundField DataField="totalValue" ReadOnly="True" HeaderText="Total Value" SortExpression="totalValue" />
                </Columns>
            </asp:GridView>
            <br />
            <asp:Label ID="SecurityError" runat="server" Text="" Visible="False"></asp:Label>
        </div>

        <!--Part C-->
        <div id="ActiveOrders">
            <h3>Active Orders</h3>
            <hr />
            <asp:GridView ID="gvActiveOrders" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="referenceNo" HeaderText="Reference No." ReadOnly="True" SortExpression="referenceNo" />
                    <asp:BoundField DataField="orderType" HeaderText="Order Type" ReadOnly="True" SortExpression="orderType" />
                    <asp:BoundField DataField="securityType" ReadOnly="True" HeaderText="Security Type" SortExpression="securityType" />
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" ReadOnly="True" HeaderText="Name" />
                    <asp:BoundField DataField="submitted" ReadOnly="True" HeaderText="Date Submitted" />
                    <asp:BoundField DataField="status" ReadOnly="True" HeaderText="Status" />
                    <asp:BoundField DataField="sharesRequested" ReadOnly="True" HeaderText="Shares Requested" />
                    <asp:BoundField DataField="limitPrice" ReadOnly="True" HeaderText="Limit Price" />
                    <asp:BoundField DataField="stopPrice" ReadOnly="True" HeaderText="Stop Price" />
                    <asp:BoundField DataField="Expiry Date" ReadOnly="True" HeaderText="Expiry Date" />
                </Columns>
            </asp:GridView>
        </div>

        <!--Part D-->
        <div id="OrderHistory">
            <h3>Order History</h3>
            <hr />
            <asp:GridView ID="gvOrderHistory" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="referenceNo" HeaderText="Reference No." ReadOnly="True" SortExpression="referenceNo" />
                    <asp:BoundField DataField="orderType" HeaderText="Order Type" ReadOnly="True" SortExpression="orderType" />
                    <asp:BoundField DataField="securityType" ReadOnly="True" HeaderText="Security Type" SortExpression="securityType" />
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" ReadOnly="True" HeaderText="Name" />
                    <asp:BoundField DataField="submitted" ReadOnly="True" HeaderText="Date Submitted" />
                    <asp:BoundField DataField="status" ReadOnly="True" HeaderText="Status" />
                </Columns>
            </asp:GridView>

        </div>

    </div>
</asp:Content>