<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="WingtipToys.Admin.AdminPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Administration</h1>
    <hr />
    <div id="AlertBox" class="alert alert-success alert-dismissible fade show" role="alert" visible="false" runat="server">
        <asp:Label ID="LabelAlertStatus" runat="server" Text=""></asp:Label>
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div>
    <div class="row">
        <div class="col-md my-5">
            <h3 class="mb-4">Add Product</h3>
            <div class="form-group row">
                <asp:Label ID="LabelAddCategory" AssociatedControlID="DropDownAddCategory" runat="server" CssClass="col-md-2">Category</asp:Label>
                <div class="col-md-10">
                    <asp:DropDownList ID="DropDownAddCategory" runat="server"
                                        ItemType="WingtipToys.Models.Category"
                                        SelectMethod="GetCategories" DataTextField="CategoryName"
                                        DataValueField="CategoryID" CssClass=""
                    >
                    </asp:DropDownList>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label ID="LabelAddName" AssociatedControlID="AddProductName" runat="server" CssClass="col-md-2 control-label">Name</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="AddProductName" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" Text="Product name required."
                                                ControlToValidate="AddProductName" SetFocusOnError="true" Display="Dynamic"
                                                CssClass="text-danger"
                    ></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label ID="LabelAddDescription" AssociatedControlID="AddProductDescription" runat="server" CssClass="col-md-2 control-label">Description</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="AddProductDescription" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" Text="Description required."
                                                ControlToValidate="AddProductDescription" SetFocusOnError="true" Display="Dynamic"
                                                CssClass="text-danger"
                    ></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label ID="LabelAddPrice" AssociatedControlID="AddProductPrice" runat="server" CssClass="col-md-2 control-label">Price</asp:Label>
                <div class="col-md-10">
                    <asp:TextBox ID="AddProductPrice" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" Text="Price required." ControlToValidate="AddProductPrice"
                                                SetFocusOnError="true" Display="Dynamic" CssClass="text-danger"
                    ></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Text="Must be a valid price without $."
                                                    ControlToValidate="AddProductPrice" SetFocusOnError="true" Display="Dynamic"
                                                    ValidationExpression="^[0-9]*(\.)?[0-9]?[0-9]?$" CssClass="text-danger"
                    ></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="form-group row">
                <asp:Label ID="LabelAddImageFile" AssociatedControlID="ProductImage" runat="server" CssClass="col-md-2 control-label">Image File</asp:Label>
                <div class="col-md-10">
                    <asp:FileUpload ID="ProductImage" CssClass="form-control-file" runat="server" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" Text="Image path required"
                                                ControlToValidate="ProductImage" SetFocusOnError="true" Display="Dynamic"
                                                CssClass="text-danger"
                    ></asp:RequiredFieldValidator>
                </div>
            </div>
            <div class="form-group">
                <asp:Button ID="AddProductBtn" runat="server" Text="Add Product" OnClick="AddProductBtn_Click" CausesValidation="true" CssClass="btn btn-primary" />
            </div>
        </div>
        <div class="col-md my-5">
            <h3 class="mb-4">Remove Product</h3>
            <div class="form-group row">
                <asp:Label ID="LabelRemoveProduct" AssociatedControlID="DropDownRemoveProduct" runat="server" CssClass="col-md-2">Product</asp:Label>
                <div class="col-md-10">
                    <asp:DropDownList ID="DropDownRemoveProduct" runat="server" ItemType="WingtipToys.Models.Product"
                                      SelectMethod="GetProducts" AppendDataBoundItems="true" DataTextField="ProductName"
                                      DataValueField="ProductID"
                    ></asp:DropDownList>
                </div>
            </div>
            <div class="form-group">
                <asp:Button ID="RemoveProductBtn" runat="server" Text="Remove Product" OnClick="RemoveProductBtn_Click" CausesValidation="false" CssClass="btn btn-primary"/>
            </div>
        </div>
    </div>    
</asp:Content>
