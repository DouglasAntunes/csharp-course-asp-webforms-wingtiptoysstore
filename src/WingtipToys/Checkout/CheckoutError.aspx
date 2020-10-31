<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutError.aspx.cs" Inherits="WingtipToys.Checkout.CheckoutError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
        <h1>Checkout Error</h1>
        <div class="my-3">
            <%=Request.QueryString.Get("ErrorCode")%>
        </div>
        <div class="mb-3">
            <%=Request.QueryString.Get("Desc")%>
        </div>
        <div class="mb-3">
            <%=Request.QueryString.Get("Desc2")%>
        </div>
</asp:Content>
