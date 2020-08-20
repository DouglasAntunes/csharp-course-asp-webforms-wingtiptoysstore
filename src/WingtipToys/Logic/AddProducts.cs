using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using WingtipToys.Models;

namespace WingtipToys.Logic
{
    public class AddProducts
    {
        public bool AddProduct(string productName, string productDesc, string productPrice,
                                string productCategory, string productImagePath)
        {
            var myProduct = new Product()
            {
                ProductName = productName, Description = productDesc,
                UnitPrice = Convert.ToDouble(productPrice, CultureInfo.InvariantCulture), ImagePath = productImagePath,
                CategoryID = Convert.ToInt32(productCategory),
            };
            using (var _db = new ProductContext())
            {
                // Add product to DB.
                _db.Products.Add(myProduct);
                _db.SaveChanges();
            }

            return true;
        }
    }
}