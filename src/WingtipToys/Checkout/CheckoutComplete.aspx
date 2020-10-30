<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutComplete.aspx.cs" Inherits="WingtipToys.Checkout.CheckoutComplete" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1 class="mb-3">Checkout Complete</h1>
    <h3>Payment Transaction ID:</h3>
    <asp:Label ID="TransactionId" runat="server"></asp:Label>
    <h3 class="my-3">Thank You!</h3>
    <asp:Button ID="Continue" runat="server" Text="Continue Shopping" OnClick="Continue_Click" CssClass="btn btn-success" />
</asp:Content>
