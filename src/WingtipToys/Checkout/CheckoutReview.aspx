<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CheckoutReview.aspx.cs" Inherits="WingtipToys.Checkout.CheckoutReview" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Order Review</h1>
    <section>
        <div class="my-3">
            <h3 class="mt-2">Products</h3>
            <asp:GridView ID="OrderItemList" runat="server" AutoGenerateColumns="false" GridLines="Both" CellPadding="10"
                          Width="500" BorderColor="#efeeef" BorderWidth="10">
                <Columns>
                    <asp:BoundField DataField="ProductId" HeaderText=" Product ID" />
                    <asp:BoundField DataField="Product.ProductName" HeaderText=" Product Name" />
                    <asp:BoundField DataField="Product.UnitPrice" HeaderText="Price (each)" DataFormatString="{0:c}"/>
                    <asp:BoundField DataField="Quantity" HeaderText="Quantity" />
                </Columns>
            </asp:GridView>
        </div>
        <asp:DetailsView ID="ShipInfo" runat="server" AutoGenerateRows="false" GridLines="None" 
                         BorderStyle="None" CommandRowStyle-BorderStyle="None" CellPadding="0"
                         CssClass="hide-first-column"
        >
            <Fields>
                <asp:TemplateField>
                    <ItemTemplate>
                        <div class="my-3">
                            <h3>Shipping Address:</h3>
                            <address>
                                <asp:Label ID="FirstName" runat="server" Text='<%#:Eval("FirstName") %>'></asp:Label>  
                                <asp:Label ID="LastName" runat="server" Text='<%#:Eval("LastName") %>'></asp:Label>
                                <br />
                                <asp:Label ID="Address" runat="server" Text='<%#:Eval("Address") %>'></asp:Label>
                                <br />
                                <asp:Label ID="City" runat="server" Text='<%#:Eval("City") %>'></asp:Label>
                                <asp:Label ID="State" runat="server" Text='<%#:Eval("State") %>'></asp:Label>
                                <asp:Label ID="PostalCode" runat="server" Text='<%#:Eval("PostalCode") %>'></asp:Label>
                            </address>
                        </div>
                        <div class="my-3">
                            <h3>Order Total:</h3>
                            <asp:Label ID="Total" runat="server" Text='<%#:Eval("Total", "{0:C}") %>'></asp:Label>
                        </div>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Fields>
        </asp:DetailsView>
        <asp:Button ID="CheckoutConfirm" runat="server" Text="Complete Order" OnClick="CheckoutConfirm_Click" CssClass="btn btn-primary" />
    </section>
</asp:Content>
