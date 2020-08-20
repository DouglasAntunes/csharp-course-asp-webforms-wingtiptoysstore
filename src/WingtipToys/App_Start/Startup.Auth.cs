using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin.Security.Google;
using Owin;
using WingtipToys.Models;

namespace WingtipToys
{
    public partial class Startup {

        // For more information on configuring authentication, please visit https://go.microsoft.com/fwlink/?LinkId=301883
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(ApplicationDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });
            // Use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // OAuth Third Party Providers
            var msApiSettings = ConfigurationManager.AppSettings.AllKeys.Where(s => s.StartsWith("MicrosoftAccountAuth_"));
            if (msApiSettings.All(s => ConfigurationManager.AppSettings.Get(s) != string.Empty))
            {
                app.UseMicrosoftAccountAuthentication(
                    clientId: ConfigurationManager.AppSettings.Get("MicrosoftAccountAuth_clientId"),
                    clientSecret: ConfigurationManager.AppSettings.Get("MicrosoftAccountAuth_clientSecret"));
            }

            var twitterApiSettings = ConfigurationManager.AppSettings.AllKeys.Where(s => s.StartsWith("TwitterAuth_"));
            if (twitterApiSettings.All(s => ConfigurationManager.AppSettings.Get(s) != string.Empty))
            {
                app.UseTwitterAuthentication(
                   consumerKey: ConfigurationManager.AppSettings.Get("TwitterAuth_consumerKey"),
                   consumerSecret: ConfigurationManager.AppSettings.Get("TwitterAuth_consumerSecret"));
            }

            var fbApiSettings = ConfigurationManager.AppSettings.AllKeys.Where(s => s.StartsWith("FacebookAuth_"));
            if (fbApiSettings.All(s => ConfigurationManager.AppSettings.Get(s) != string.Empty))
            {
                app.UseFacebookAuthentication(
                   appId: ConfigurationManager.AppSettings.Get("FacebookAuth_appId"),
                   appSecret: ConfigurationManager.AppSettings.Get("FacebookAuth_appSecret"));
            }

            var gApiSettings = ConfigurationManager.AppSettings.AllKeys.Where(s => s.StartsWith("GoogleAuth_"));
            if (gApiSettings.All(s => ConfigurationManager.AppSettings.Get(s) != string.Empty))
            {
                app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
                {
                    ClientId = ConfigurationManager.AppSettings.Get("GoogleAuth_ClientId"),
                    ClientSecret = ConfigurationManager.AppSettings.Get("GoogleAuth_ClientSecret")
                });
            }
        }
    }
}
