using System;
using System.IO;
using System.Linq;
using WingtipToys.Logic;
using WingtipToys.Models;

namespace WingtipToys.Admin
{
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["ProductAction"];
            switch (productAction)
            {
                case "add":
                    AlertBox.Visible = true;
                    LabelAlertStatus.Text = "Product added!";
                    break;

                case "remove":
                    AlertBox.Visible = true;
                    LabelAlertStatus.Text = "Product removed!";
                    break;
            }
        }

        protected void AddProductBtn_Click(object sender, EventArgs e)
        {
            bool fileOk = false;
            string path = Server.MapPath("~/Catalog/Images/");
            if (ProductImage.HasFile)
            {
                string uploadedFileExt = Path.GetExtension(ProductImage.FileName).ToLower();
                string[] allowedExtensions = { ".gif", ".png", ".jpeg", ".jpg" };

                fileOk = allowedExtensions.Any(ext => ext == uploadedFileExt);
            }

            if (!fileOk)
            {
                AlertBox.Attributes.Add("class", AlertBox.Attributes["class"].Replace("alert-success", "alert-danger"));
                AlertBox.Visible = true;
                LabelAlertStatus.Text = "Unable to accept file type.";
                return;
            }
            else
            {
                try
                {
                    // Save to Images folder.
                    ProductImage.PostedFile.SaveAs(path + ProductImage.FileName);
                    // Save to Images/Thumb folder.
                    ProductImage.PostedFile.SaveAs(path + "Thumbs/" + ProductImage.FileName);
                }
                catch(Exception ex)
                {
                    AlertBox.Attributes.Add("class", AlertBox.Attributes["class"].Replace("alert-success", "alert-danger"));
                    AlertBox.Visible = true;
                    LabelAlertStatus.Text = ex.Message;
                }

                // Add product data to DB.
                AddProducts products = new AddProducts();
                bool addSuccess = products.AddProduct(
                    AddProductName.Text, AddProductDescription.Text, AddProductPrice.Text,
                    DropDownAddCategory.SelectedValue, ProductImage.FileName
                );
                if(!addSuccess)
                {
                    AlertBox.Attributes.Add("class", AlertBox.Attributes["class"].Replace("alert-success", "alert-danger"));
                    AlertBox.Visible = true;
                    LabelAlertStatus.Text = "Unable to add new product to database.";
                    return;
                }
                else
                {
                    Response.Redirect($"{GetPageUrl()}?ProductAction=add");
                }
            }
        }

        public IQueryable GetCategories()
        {
            var _db = new ProductContext();
            IQueryable query = _db.Categories;
            return query;
        }

        public IQueryable GetProducts()
        {
            var _db = new ProductContext();
            IQueryable query = _db.Products;
            return query;
        }

        private string GetPageUrl()
        {
            return Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
        }

        protected void RemoveProductBtn_Click(object sender, EventArgs e)
        {
            using (var _db = new ProductContext())
            {
                int productId = Convert.ToInt16(DropDownRemoveProduct.SelectedValue);
                var myItem = _db.Products.Where(c => c.ProductID == productId).FirstOrDefault();
                if (myItem == null)
                {
                    AlertBox.Attributes.Add("class", AlertBox.Attributes["class"].Replace("alert-success", "alert-danger"));
                    AlertBox.Visible = true;
                    LabelAlertStatus.Text = "Unable to locate product.";
                    return;
                }
                else
                {
                    _db.Products.Remove(myItem);
                    _db.SaveChanges();

                    // Reload the page
                    Response.Redirect($"{GetPageUrl()}?ProductAction=remove");
                }
            }
        }
    }
}