<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="WingtipToys.ErrorPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 class="mb-3">Error:</h2>
    <asp:Label ID="FriendlyErrorMsg" runat="server" Text="Label" Font-Size="Large" style="color: red"></asp:Label>

    <asp:Panel ID="DetailedErrorPanel" runat="server" Visible="false">
        <h4 class="mt-3">Detailed Error:</h4>
        
        <p><asp:Label ID="ErrorDetailedMsg" runat="server" Font-Size="Small" /></p>

        <h4 class="mt-3">Error Handler:</h4>
        
        <p><asp:Label ID="ErrorHandler" runat="server" Font-Size="Small" /></p>

        <h4 class="mt-3">Detailed Error Message:</h4>
        
        <p><asp:Label ID="InnerMessage" runat="server" Font-Size="Small" /></p>

        <p><asp:Label ID="InnerTrace" runat="server" /></p>
    </asp:Panel>
</asp:Content>
