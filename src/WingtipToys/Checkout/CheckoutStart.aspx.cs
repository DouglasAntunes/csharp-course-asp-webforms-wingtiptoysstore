using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WingtipToys.Logic;

namespace WingtipToys.Checkout
{
    public partial class CheckoutStart : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            NVPAPICaller paypalCaller = new NVPAPICaller();
            string retMsg = string.Empty;
            string token = string.Empty;

            if (Session["payment_amt"] != null)
            {
                string amt = Session["payment_amt"].ToString();
                bool ret = paypalCaller.ShortcutExpressCheckout(amt, ref token, ref retMsg);
                if (ret)
                {
                    Session["token"] = token;
                    Response.Redirect(retMsg);
                }
                else Response.Redirect($"CheckoutError.aspx?{retMsg}");
            }
            else Response.Redirect("CheckoutError.aspx?ErrorCode=AmtMissing");

        }
    }
}