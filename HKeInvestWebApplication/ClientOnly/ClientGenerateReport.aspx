<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ClientGenerateReport.aspx.cs" Inherits="HKeInvestWebApplication.ClientOnly.ClientGenerateReport" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Generate Report</h2>

    <div id="Report">
        <!--Part A-->
        <div id="ClientInfo">
            <h3>Account Summary</h3>
            <hr />
            <asp:Label runat="server" AssociatedControlID="AccountNum" Text="Account Number: "></asp:Label>
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
                    <asp:BoundField DataField="referenceNumber" HeaderText="Reference No." ReadOnly="True" SortExpression="referenceNumber" />
                    <asp:BoundField DataField="buyOrSell" HeaderText="Order Type" ReadOnly="True" SortExpression="orderType" />
                    <asp:BoundField DataField="securityType" ReadOnly="True" HeaderText="Security Type" SortExpression="securityType" />
                    <asp:BoundField DataField="securityCode" HeaderText="Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" ReadOnly="True" HeaderText="Name" />
                    <asp:BoundField DataField="dateSubmitted" ReadOnly="True" HeaderText="Date Submitted" />
                    <asp:BoundField DataField="status" ReadOnly="True" HeaderText="Status" />
                    <asp:BoundField DataField="shares" ReadOnly="True" HeaderText="Shares Requested" />
                    <asp:BoundField DataField="limitPrice" ReadOnly="True" HeaderText="Limit Price" />
                    <asp:BoundField DataField="stopPrice" ReadOnly="True" HeaderText="Stop Price" />
                    <asp:BoundField DataField="expiryDay" ReadOnly="True" HeaderText="Expiry Date" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="ActiveError" runat="server" Visible="False"></asp:Label>
        </div>

        <!--Part D-->
        <div id="OrderHistory">
            <h3>Order History</h3>
            <hr />
            <asp:Label runat="server" Text="Begin Date: " AssociatedControlID="BeginDate"></asp:Label><asp:TextBox ID="BeginDate" runat="server" TextMode="Date"></asp:TextBox><asp:Label runat="server" Text="End Date: " AssociatedControlID="EndDate"></asp:Label><asp:TextBox ID="EndDate" runat="server" TextMode="Date"></asp:TextBox>
            <asp:GridView ID="gvOrderHistory" runat="server" AutoGenerateColumns="False" AllowSorting="True" OnSorting="gvOrderHistory_Sorting">
                <Columns>
                    <asp:BoundField DataField="referenceNumber" HeaderText="Reference No." ReadOnly="True" />
                    <asp:BoundField DataField="buyOrSell" HeaderText="Order Type" ReadOnly="True" />
                    <asp:BoundField DataField="securityType" ReadOnly="True" HeaderText="Security Type" SortExpression="securityType" />
                    <asp:BoundField DataField="securityCode" HeaderText="Code" ReadOnly="True" />
                    <asp:BoundField DataField="name" ReadOnly="True" HeaderText="Name" SortExpression="name" />
                    <asp:BoundField DataField="dateSubmitted" ReadOnly="True" HeaderText="Date Submitted" />
                    <asp:BoundField DataField="status" ReadOnly="True" HeaderText="Status" SortExpression="status" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="HistoryError" runat="server">Please select a time interval</asp:Label>
        </div>

        <!--Part E-->
        <div>
            <h3>Transactions</h3>
            <asp:Label runat="server" Text="Reference Number" AssociatedControlID="RefNumber"></asp:Label><asp:TextBox ID="RefNumber" runat="server"></asp:TextBox>
            <asp:GridView ID="gvTransactions" runat="server" AutoGenerateColumns="false">
                <Columns>
                     <asp:BoundField DataField="transactionNumber" ReadOnly="True" HeaderText="Transaction No." />
                     <asp:BoundField DataField="executeDate" ReadOnly="True" HeaderText="Date Executed" />
                     <asp:BoundField DataField="executeShares" ReadOnly="True" HeaderText="Quantity Executed" />
                     <asp:BoundField DataField="executePrice" ReadOnly="True" HeaderText="Price Per Share" />
                </Columns>
            </asp:GridView>
            <asp:Label ID="TransactionError" runat="server" Text="" Visible="False"></asp:Label>
        </div>
    </div>
</asp:Content>