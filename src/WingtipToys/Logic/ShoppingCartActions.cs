using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WingtipToys.Models;

namespace WingtipToys.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartId { get; set; }
        private ProductContext _db = new ProductContext();
        public const string CartSessionKey = "CartId";

        public void AddToCart(int id)
        {
            // Retrieve the product from the database.
            ShoppingCartId = GetCartId();

            var cartItem = _db.ShoppingCartItems
                                .SingleOrDefault(cart => cart.CartId == ShoppingCartId &&
                                                    cart.ProductId == id);

            if(cartItem == null)
            {
                // Create a new cart item if no cart item exists.
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = id,
                    CartId = ShoppingCartId,
                    Product = _db.Products.SingleOrDefault(prod => prod.ProductID == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now,
                };
                _db.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                // If the item does exists in the cart, then add one to quantity.
                cartItem.Quantity++;
            }
            _db.SaveChanges();
        }

        public string GetCartId()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                // User Logged
                if(!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                // Guest or without a cart
                else
                {
                    // Generate a new random GUID using System.Guild class.
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public void MigrateCart(string cartId, string userName)
        {
            var shoppingCart = _db.ShoppingCartItems.Where(cart => cart.CartId == cartId);
            foreach(CartItem item in shoppingCart)
            {
                item.CartId = userName;
            }
            HttpContext.Current.Session[CartSessionKey] = userName;
            _db.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartId();
            return _db.ShoppingCartItems.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }

        public decimal GetTotal()
        {
            ShoppingCartId = GetCartId();
            /* Multiply product price by quantity of that product to get the current price 
             * for each of those products in the cart. Sum all product prices total 
             * to get the cart total
             */
            decimal? total = decimal.Zero;
            total = (decimal?)(from cartItems in _db.ShoppingCartItems
                               where cartItems.CartId == ShoppingCartId
                               select cartItems.Quantity * cartItems.Product.UnitPrice)
                               .Sum();
            return total ?? decimal.Zero;
        }

        public void Dispose()
        {
            if(_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public ShoppingCartActions GetCart(HttpContext context)
        {
            using (var cart = new ShoppingCartActions())
            {
                cart.ShoppingCartId = cart.GetCartId();
                return cart;
            }
        }

        public void UpdateShoppingCartDatabase(string cartId, ShoppingCartUpdates[] cartItemUpdates)
        {
            using (var _db = new ProductContext())
            {
                int cartItemCount = cartItemUpdates.Count();
                List<CartItem> myCart = GetCartItems();
                foreach (var cartItem in myCart)
                {
                    // Iterate through all rows within shopping cart list
                    for (int i = 0; i < cartItemCount; i++)
                    {
                        if(cartItem.Product.ProductID == cartItemUpdates[i].ProductId)
                        {
                            if(cartItemUpdates[i].PurchaseQuantity < 1 || cartItemUpdates[i].RemoveItem == true)
                            {
                                RemoveItem(cartId, cartItem.ProductId);
                            }
                            else
                            {
                                UpdateItem(cartId, cartItem.ProductId, cartItemUpdates[i].PurchaseQuantity);
                            }
                        }
                    }
                }
            }
        }

        public void RemoveItem(string removeCartID, int removeProductID)
        {
            using (var _db = new ProductContext())
            {
                try
                {
                    var myItem = (from c in _db.ShoppingCartItems
                                  where c.CartId == removeCartID &&
                                  c.Product.ProductID == removeProductID
                                  select c).FirstOrDefault();

                    if (myItem != null)
                    {
                        // Remove Item.
                        _db.ShoppingCartItems.Remove(myItem);
                        _db.SaveChanges();
                    }
                }
                catch(Exception e)
                {
                    throw new Exception("ERROR: Unable to Remove Cart Item - " + e.Message.ToString(), e);
                }
            }
        }

        public void UpdateItem(string updateCartID, int updateProductID, int quantity)
        {
            using (var _db = new ProductContext())
            {
                try
                {
                    var myItem = (from c in _db.ShoppingCartItems
                                  where c.CartId == updateCartID &&
                                  c.Product.ProductID == updateProductID
                                  select c).FirstOrDefault();
                    
                    if(myItem != null)
                    {
                        myItem.Quantity = quantity;
                        _db.SaveChanges();
                    }

                }
                catch(Exception e)
                {
                    throw new Exception("ERROR: Unable to Update Cart Item - " + e.Message.ToString(), e);
                }
            }
        }

        public void EmptyCart()
        {
            ShoppingCartId = GetCartId();
            var cartItems = _db.ShoppingCartItems.Where(cart => cart.CartId == ShoppingCartId);
            foreach (var cartItem in cartItems)
            {
                _db.ShoppingCartItems.Remove(cartItem);
            }
            _db.SaveChanges();
        }

        public int GetCount()
        {
            ShoppingCartId = GetCartId();

            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in _db.ShoppingCartItems
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();
            // return 0 if all entries are null
            return count ?? 0;
        }

        public struct ShoppingCartUpdates
        {
            public int ProductId;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }
    }
}
