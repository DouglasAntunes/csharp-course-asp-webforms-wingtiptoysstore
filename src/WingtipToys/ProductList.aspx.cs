using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.ModelBinding;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Routing;
using WingtipToys.Models;

namespace WingtipToys
{
    public partial class ProductList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public IQueryable<Product> GetProducts([QueryString("id")] int? categoryId, [RouteData] string categoryName)
        {
            var _db = new ProductContext();
            IQueryable<Product> query = _db.Products;

            if(categoryId.HasValue && categoryId > 0)
            {
                query = query.Where(product => product.CategoryID == categoryId);
            }

            if (!string.IsNullOrEmpty(categoryName))
            {
                query = query.Where(product => string.Compare(product.Category.CategoryName, categoryName) == 0);
            }
            return query;
        }
    }
}