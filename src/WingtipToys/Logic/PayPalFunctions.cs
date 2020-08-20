using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using WingtipToys.Models;

namespace WingtipToys.Logic
{
    public class NVPAPICaller
    {
        //Flag that determines the PayPal environment (live or sandbox)
        private const bool bSandbox = true;
        private const string CVV2 = "CVV2";

        // Live strings.
        private string pEndPointURL = "https://api-3t.paypal.com/nvp";
        private string host = "www.paypal.com";

        // Sandbox strings.
        private string pEndPointURL_SB = "https://api-3t.sandbox.paypal.com/nvp";
        private string host_SB = "www.sandbox.paypal.com";

        private const string SIGNATURE = "SIGNATURE";
        private const string PWD = "PWD";
        private const string ACCT = "ACCT";

        public string APIUsername = string.Empty;
        private string APIPassword = string.Empty;
        private string APISignature = string.Empty;
        private string Subject = string.Empty;
        private string BNCode = "PP-ECWizard";

        //HttpWebRequest Timeout specified in milliseconds 
        private const int Timeout = 15000;
        private static readonly string[] SECURED_NVPS = new string[] { ACCT, CVV2, SIGNATURE, PWD };

        public NVPAPICaller()
        {
            var apiSettings = ConfigurationManager.AppSettings.AllKeys.Where(s => s.StartsWith("PayPal_"));
            if (apiSettings.Count() == 0) throw new Exception("The configuration keys for 'PayPal_' is not defined on 'AppSettingsSecrets.config'.");
            if (apiSettings.All(s => ConfigurationManager.AppSettings.Get(s) == string.Empty)) throw new Exception("The configuration keys for 'PayPal_' on 'AppSettingsSecrets.config' has empty values.");
            SetCredentials(
                ConfigurationManager.AppSettings.Get("PayPal_APIUsername"),
                ConfigurationManager.AppSettings.Get("PayPal_APIPassword"),
                ConfigurationManager.AppSettings.Get("PayPal_APISignature")
            );
        }

        public void SetCredentials(string userId, string pwd, string signature)
        {
            APIUsername = userId;
            APIPassword = pwd;
            APISignature = signature;
        }

        private static readonly string DefaultPaymentCurrencyCode = "USD";
        private static readonly string _getCurrency = new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol;
        private string PaymentCurrencyCode = _getCurrency != "¤" ? _getCurrency : DefaultPaymentCurrencyCode;


        public bool ShortcutExpressCheckout(string amt, ref string token, ref string retMsg)
        {
            if (bSandbox)
            {
                pEndPointURL = pEndPointURL_SB;
                host = host_SB;
            }

            string returnURL = "https://localhost:44302/Checkout/CheckoutReview.aspx";
            string cancelURL = "https://localhost:44302/Checkout/CheckoutCancel.aspx";

            NVPCodec encoder = new NVPCodec
            {
                ["METHOD"] = "SetExpressCheckout",
                ["RETURNURL"] = returnURL,
                ["CANCELURL"] = cancelURL,
                ["BRANDNAME"] = "Wingtip Toys Sample Application",
                ["PAYMENTREQUEST_0_AMT"] = amt,
                ["PAYMENTREQUEST_0_ITEMAMT"] = amt,
                ["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale",
                ["PAYMENTREQUEST_0_CURRENCYCODE"] = PaymentCurrencyCode
            };

            // Get the Shopping Cart Products
            using (ShoppingCartActions myCartOrders = new ShoppingCartActions())
            {
                List<CartItem> myOrderList = myCartOrders.GetCartItems();

                for (int i = 0; i < myOrderList.Count; i++)
                {
                    encoder["L_PAYMENTREQUEST_0_NAME" + i] = myOrderList[i].Product.ProductName.ToString();
                    var unitPrice = myOrderList[i].Product.UnitPrice;
                    encoder["L_PAYMENTREQUEST_0_AMT" + i] = unitPrice.HasValue ? ((double)unitPrice).ToString(CultureInfo.InvariantCulture) : string.Empty;
                    encoder["L_PAYMENTREQUEST_0_QTY" + i] = myOrderList[i].Quantity.ToString();
                }
            }

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            NVPCodec decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                token = decoder["TOKEN"];
                string ECURL = "https://" + host + "/cgi-bin/webscr?cmd=_express-checkout" + "&token=" + token;
                retMsg = ECURL;
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];
                return false;
            }
        }

        public bool GetCheckoutDetails(string token, ref string payerID, ref NVPCodec decoder, ref string retMsg)
        {
            if (bSandbox)
            {
                pEndPointURL = pEndPointURL_SB;
            }

            NVPCodec encoder = new NVPCodec
            {
                ["METHOD"] = "GetExpressCheckoutDetails",
                ["TOKEN"] = token
            };

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                payerID = decoder["PAYERID"];
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }

        public bool DoCheckoutPayment(string finalPaymentAmount, string token, string payerID, ref NVPCodec decoder, ref string retMsg)
        {
            if (bSandbox)
            {
                pEndPointURL = pEndPointURL_SB;
            }

            NVPCodec encoder = new NVPCodec
            {
                ["METHOD"] = "DoExpressCheckoutPayment",
                ["TOKEN"] = token,
                ["PAYERID"] = payerID,
                ["PAYMENTREQUEST_0_AMT"] = finalPaymentAmount,
                ["PAYMENTREQUEST_0_CURRENCYCODE"] = PaymentCurrencyCode,
                ["PAYMENTREQUEST_0_PAYMENTACTION"] = "Sale"
            };

            string pStrrequestforNvp = encoder.Encode();
            string pStresponsenvp = HttpCall(pStrrequestforNvp);

            decoder = new NVPCodec();
            decoder.Decode(pStresponsenvp);

            string strAck = decoder["ACK"].ToLower();
            if (strAck != null && (strAck == "success" || strAck == "successwithwarning"))
            {
                return true;
            }
            else
            {
                retMsg = "ErrorCode=" + decoder["L_ERRORCODE0"] + "&" +
                    "Desc=" + decoder["L_SHORTMESSAGE0"] + "&" +
                    "Desc2=" + decoder["L_LONGMESSAGE0"];

                return false;
            }
        }

        public string HttpCall(string nvpRequest)
        {
            string url = pEndPointURL;

            string strPost = nvpRequest + "&" + BuildCredentialsNVPString();
            strPost = strPost + "&BUTTONSOURCE=" + HttpUtility.UrlEncode(BNCode);

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Timeout = Timeout;
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;

            try
            {
                using (StreamWriter myWriter = new StreamWriter(objRequest.GetRequestStream()))
                {
                    myWriter.Write(strPost);
                }
            }
            catch (Exception e)
            {
                ExceptionUtility.LogException(e, "HttpCall in PayPalFunction.cs");
            }

            //Retrieve the Response returned from the NVP API call to PayPal.
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            string result;
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }

        private string BuildCredentialsNVPString()
        {
            NVPCodec codec = new NVPCodec();

            if (!IsEmpty(APIUsername))
                codec["USER"] = APIUsername;

            if (!IsEmpty(APIPassword))
                codec[PWD] = APIPassword;

            if (!IsEmpty(APISignature))
                codec[SIGNATURE] = APISignature;

            if (!IsEmpty(Subject))
                codec["SUBJECT"] = Subject;

            codec["VERSION"] = "88.0";

            return codec.Encode();
        }

        public static bool IsEmpty(string s) => s == null || s.Trim() == string.Empty;
    }

    public sealed class NVPCodec : NameValueCollection
    {
        private const string AMPERSAND = "&";
        private const string EQUALS = "=";
        private static readonly char[] AMPERSAND_CHAR_ARRAY = AMPERSAND.ToCharArray();
        private static readonly char[] EQUALS_CHAR_ARRAY = EQUALS.ToCharArray();

        public string Encode()
        {
            StringBuilder sb = new StringBuilder();
            bool firstPair = true;
            foreach (string kv in AllKeys)
            {
                string name = HttpUtility.UrlEncode(kv);
                string value = HttpUtility.UrlEncode(this[kv]);
                if (!firstPair)
                {
                    sb.Append(AMPERSAND);
                }
                sb.Append(name).Append(EQUALS).Append(value);
                firstPair = false;
            }
            return sb.ToString();
        }

        public void Decode(string nvpString)
        {
            Clear();
            foreach (string nvp in nvpString.Split(AMPERSAND_CHAR_ARRAY))
            {
                string[] tokens = nvp.Split(EQUALS_CHAR_ARRAY);
                if (tokens.Length >= 2)
                {
                    string name = HttpUtility.UrlDecode(tokens[0]);
                    string value = HttpUtility.UrlDecode(tokens[1]);
                    Add(name, value);
                }
            }
        }

        public void Add(string name, string value, int index) => Add(GetArrayName(index, name), value);

        public void Remove(string arrayName, int index) => Remove(GetArrayName(index, arrayName));

        public string this[string name, int index]
        {
            get
            {
                return this[GetArrayName(index, name)];
            }
            set
            {
                this[GetArrayName(index, name)] = value;
            }
        }

        private static string GetArrayName(int index, string name)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index", "index cannot be negative : " + index);
            }
            return name + index;
        }
    }
}