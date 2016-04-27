<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientGenerateReport.aspx.cs" Inherits="HKeInvestWebApplication.ClientGenerateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Generate Report</h2>

    <div id="Report">
        <div id="ClientInfo">

            <h3>Account Summary</h3>
            <hr />
            <asp:Label runat="server" AssociatedControlID="AccountNumber" Text="Account Number: "></asp:Label>
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="ClientName" Text="Client Name(s): "></asp:Label>
            <asp:Label ID="ClientName" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="TotalValue" Text="Total Monetary Value: "></asp:Label>
            <asp:Label ID="TotalValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" Text="Free Balance: " AssociatedControlID="FreeBalance"></asp:Label>
            <asp:Label ID="FreeBalance" runat="server"></asp:Label>

            <br />
            <asp:Label runat="server" AssociatedControlID="StockValue" Text="Total Stock Value: "></asp:Label>
            <asp:Label ID="StockValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="BondValue" Text="Total Bond Value:"></asp:Label>
            <asp:Label ID="BondValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="UnitTrustValue" Text="Total Unit Trust Value: "></asp:Label>
            <asp:Label ID="UnitTrustValue" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="LastOrderDate" Text="Last Executed Order Date:"></asp:Label>
            <asp:Label ID="LastOrderDate" runat="server"></asp:Label>
            <br />
            <asp:Label runat="server" AssociatedControlID="LastOrderValue" Text="Last Executed Order Value:"></asp:Label>
            <asp:Label ID="LastOrderValue" runat="server"></asp:Label>

        </div>

        <div id="SecuritySummary">
            <h3>Securities Summary</h3>
            <hr />
            <asp:GridView ID="gvSecurities" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="code" HeaderText="Code" ReadOnly="True" SortExpression="code" />
                    <asp:BoundField DataField="name" HeaderText="Name" ReadOnly="True" SortExpression="name" />
                    <asp:BoundField DataField="shares" ReadOnly="True" HeaderText="Shares Held" SortExpression="sharesHeld" />
                    <asp:BoundField DataField="price" HeaderText="Price Per Share" ReadOnly="True" />
                    <asp:BoundField DataField="totalValue" ReadOnly="True" HeaderText="Total Value" SortExpression="totalValue" />
                </Columns>
            </asp:GridView>
        </div>

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