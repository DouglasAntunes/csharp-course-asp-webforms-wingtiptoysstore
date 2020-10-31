<%@ Page Title="Welcome" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WingtipToys._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col">
        <h1><%: Title %>.</h1>
        <h2>Wingtip Toys can help you find the perfect gift.</h2>
        <p class="lead mt-3 mb-5">
            We're all about transportation toys. You can order 
            any of our toys today. Each toy listing has detailed 
            information to help you choose the right toy.
        </p>
    </div>
</asp:Content>
