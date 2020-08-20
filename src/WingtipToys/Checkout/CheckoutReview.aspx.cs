using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Logic;
using WingtipToys.Models;

namespace WingtipToys.Checkout
{
    public partial class CheckoutReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                NVPAPICaller paypalCaller = new NVPAPICaller();

                string retMsg = string.Empty;
                string token = Session["token"].ToString();
                string payerId = string.Empty;
                NVPCodec decoder = new NVPCodec();
                
                bool ret = paypalCaller.GetCheckoutDetails(token, ref payerId, ref decoder, ref retMsg);
                if (ret)
                {
                    Session["payerId"] = payerId;

                    var myOrder = new Order()
                    {
                        OrderDate = Convert.ToDateTime(decoder["TIMESTAMP"].ToString()),
                        Username = User.Identity.Name,
                        FirstName = decoder["FIRSTNAME"].ToString(),
                        LastName = decoder["LASTNAME"].ToString(),
                        Address = decoder["SHIPTOSTREET"].ToString(),
                        City = decoder["SHIPTOCITY"].ToString(),
                        State = decoder["SHIPTOSTATE"].ToString(),
                        PostalCode = decoder["SHIPTOZIP"].ToString(),
                        Country = decoder["SHIPTOCOUNTRYCODE"].ToString(),
                        Email = decoder["EMAIL"].ToString(),
                        Total = Convert.ToDecimal(decoder["AMT"].ToString(), CultureInfo.InvariantCulture),
                    };

                    // Verify total payment amount as set on CheckoutStart.aspx.
                    try
                    {
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString(), CultureInfo.InvariantCulture);
                        decimal paymentAmoutFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString(), CultureInfo.InvariantCulture);
                        if (paymentAmountOnCheckout != paymentAmoutFromPayPal)
                        {
                            Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                        }
                    }
                    catch (Exception)
                    {
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                    }

                    // Get DB context.
                    ProductContext _db = new ProductContext();

                    // Add order to DB.
                    _db.Orders.Add(myOrder);
                    _db.SaveChanges();

                    // Get the shopping cart items and process them.
                    using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
                    {
                        List<CartItem> myOrderList = usersShoppingCart.GetCartItems();

                        // Add OrderDetail information to the DB for each product purchased.
                        for (int i = 0; i < myOrderList.Count; i++)
                        {
                            // Create a new OrderDetail object.
                            var myOrderDetail = new OrderDetail
                            {
                                OrderId = myOrder.OrderId,
                                Username = User.Identity.Name,
                                ProductId = myOrderList[i].ProductId,
                                Quantity = myOrderList[i].Quantity,
                                UnitPrice = myOrderList[i].Product.UnitPrice
                            };

                            // Add OrderDetail to DB.
                            _db.OrderDetails.Add(myOrderDetail);
                            _db.SaveChanges();
                        }

                        // Set OrderId.
                        Session["currentOrderId"] = myOrder.OrderId;

                        // Display Order information.
                        List<Order> orderList = new List<Order>
                        {
                            myOrder
                        };
                        ShipInfo.DataSource = orderList;
                        ShipInfo.DataBind();

                        // Display OrderDetails.
                        OrderItemList.DataSource = myOrderList;
                        OrderItemList.DataBind();
                    }
                }
                else Response.Redirect($"CheckoutError.aspx?{retMsg}");
            }
        }

        protected void CheckoutConfirm_Click(object sender, EventArgs e)
        {
            Session["userCheckoutCompleted"] = "true";
            Response.Redirect("~/Checkout/CheckoutComplete.aspx");
        }
    }
}