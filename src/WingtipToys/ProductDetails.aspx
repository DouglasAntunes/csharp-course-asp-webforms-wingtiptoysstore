<%@ Page Title="Product Details" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="WingtipToys.ProductDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FormView ID="productDetail" runat="server" ItemType="WingtipToys.Models.Product" SelectMethod="GetProduct" RenderOuterTable="false">
        <ItemTemplate>
            <div class="col">
                <h1><%#:Item.ProductName %></h1>
                <div class="d-flex flex-column flex-md-row">
                    <div class="col col-md-4 mr-2 pl-0">
                        <img src="/Catalog/Images/<%#:Item.ImagePath %>" class="img-fluid w-100 border border-dark" alt="<%#:Item.ProductName %>" />
                    </div>
                    <div class="col col-md-8 text-left mt-4 mt-md-0">
                        <strong>Description:</strong>
                        <p><%#:Item.Description %></p>
                        <p><strong>Price: </strong><%#:String.Format("{0:c}", Item.UnitPrice) %></p>
                        <p><strong>Product Number:</strong>&nbsp;<%#:Item.ProductID %></p>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:FormView>
</asp:Content>
