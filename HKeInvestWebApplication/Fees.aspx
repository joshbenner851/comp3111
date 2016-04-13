<%@ Page Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Fees.aspx.cs" Inherits="HKeInvestWebApplication.Fees" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h2>HKeInvest Service Fees</h2>
    <hr />
    <table class="table table-striped table-bordered">
        <tr>
            <th>Stock Order</th>
            <th>Less than HK$1,000,000</th>
            <th>Greater than HK$1,000,000</th>
        </tr>
        <tr>
            <td>Minimum Fee</td>
            <td>$150</td>
            <td>$100</td>
        </tr>
        <tr>
            <td>Market Order</td>
            <td>0.4%</td>
            <td>0.2%</td>
        </tr>
        <tr>
            <td>Limit or Stop Order</td>
            <td>0.6%</td>
            <td>0.4%</td>
        </tr>
        <tr>
            <td>Stop Limit Order</td>
            <td>0.8%</td>
            <td>0.6%</td>
        </tr>
    </table>
    <hr />
    <table class="table table-striped table-bordered">
        <tr>
            <th>Bond/Unit Trust Order</th>
            <th>Less than HK$500,000</th>
            <th>Greater than HK$500,000</th>
        </tr>
        <tr>
            <td>Buying Fee</td>
            <td>5%</td>
            <td>3%</td>
        </tr>
        <tr>
            <td>Selling Fee</td>
            <td>$100</td>
            <td>$50</td>
        </tr>
    </table>
</asp:Content>
