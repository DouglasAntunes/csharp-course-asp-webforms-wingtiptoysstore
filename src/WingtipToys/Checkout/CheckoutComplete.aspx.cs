using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Logic;
using WingtipToys.Models;

namespace WingtipToys.Checkout
{
    public partial class CheckoutComplete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Verify user has completed the checkout process.
                if ((string)Session["userCheckoutCompleted"] != "true")
                {
                    Session["userCheckoutCompleted"] = string.Empty;
                    Response.Redirect("CheckoutError.aspx?Desc=Unvalidated%20Checkout.");
                }

                NVPAPICaller payPalCaller = new NVPAPICaller();

                string retMsg = string.Empty;
                string token = Session["token"].ToString();
                string finalPaymentAmount = Session["payment_amt"].ToString();
                string PayerID = Session["payerId"].ToString();
                NVPCodec decoder = new NVPCodec();

                bool ret = payPalCaller.DoCheckoutPayment(finalPaymentAmount, token, PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    // Retrieve PayPal confirmation value.
                    string PaymentConfirmation = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
                    TransactionId.Text = PaymentConfirmation;

                    ProductContext _db = new ProductContext();
                    // Get the current order id.
                    int currentOrderId = -1;
#pragma warning disable CS0184 // 'is' expression's given expression is never of the provided type
                    currentOrderId = Session["currentOrderId"].GetType() is Int32
#pragma warning restore CS0184 // 'is' expression's given expression is never of the provided type
                        ? (Int32)Session["currentOrderId"]
                        : Convert.ToInt32(Session["currentOrderID"]);
                    Order myCurrentOrder;
                    if (currentOrderId >= 0)
                    {
                        // Get the order based on order id.
                        myCurrentOrder = _db.Orders.Single(o => o.OrderId == currentOrderId);
                        // Update the order to reflect payment has been completed.
                        myCurrentOrder.PaymentTransactionId = PaymentConfirmation;
                        // Save to DB.
                        _db.SaveChanges();
                    }

                    // Clear shopping cart.
                    using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
                    {
                        usersShoppingCart.EmptyCart();
                    }

                    // Clear order id.
                    Session["currentOrderId"] = string.Empty;
                }
                else Response.Redirect("CheckoutError.aspx?" + retMsg);
            }
        }

        protected void Continue_Click(object sender, EventArgs e) => Response.Redirect("~/Default.aspx");
    }
}