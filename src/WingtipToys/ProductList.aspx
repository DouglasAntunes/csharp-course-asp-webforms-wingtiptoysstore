<%@ Page Title="Products" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductList.aspx.cs" Inherits="WingtipToys.ProductList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <section class="col">
        <h2><%: Page.Title %></h2>

        <asp:ListView ID="productList" runat="server" DataKeyNames="ProductID" GroupItemCount="4"
            ItemType="WingtipToys.Models.Product" SelectMethod="GetProducts">
            <EmptyDataTemplate>
                <div class="alert alert-danger" role="alert">
                    No data was returned
                </div>
            </EmptyDataTemplate>
            <EmptyItemTemplate><td/></EmptyItemTemplate>
            <GroupTemplate>
                <div id="itemPlaceholderContainer" runat="server" class="col d-flex justify-content-between">
                    <div id="itemPlaceholder" runat="server"></div>
                </div>
            </GroupTemplate>
            <ItemTemplate>
                <div runat="server" class="card mx-2 my-4">
                    <a href="<%#:GetRouteUrl("ProductByNameRoute", new { productName = Item.ProductName }) %>">
                        <img src="/Catalog/Images/Thumbs/<%#:Item.ImagePath %>" class="card-img-top" alt="<%#: Item.ProductName %>"/>
                    </a>
                    <div class="card-body">
                        <a href="<%#:GetRouteUrl("ProductByNameRoute", new { productName = Item.ProductName }) %>">
                            <h5 class="card-title"><%#:Item.ProductName %></h5>
                        </a>
                        <p class="card-text"><strong>Price: </strong><%#: String.Format("{0:c}", Item.UnitPrice) %></p>
                        <a href="../AddToCart.aspx?productID=<%#: Item.ProductID %>" class="btn btn-primary">Add to Cart</a>
                    </div>
                </div>
            </ItemTemplate>
            <LayoutTemplate>
                <div id="groupPlaceHolderContainer" runat="server">
                    <div id="groupPlaceholder" runat="server"></div>
                </div>
            </LayoutTemplate>
        </asp:ListView>
    </section>
</asp:Content>
