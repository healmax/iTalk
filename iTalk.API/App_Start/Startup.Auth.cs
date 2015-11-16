using iTalk.DAO;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using System;

namespace iTalk.API {
    /// <summary>
    /// 啟動程序
    /// </summary>
    public partial class Startup {
        /// <summary>
        /// OAuth 驗證選項
        /// </summary>
        public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }

        /// <summary>
        /// 不明
        /// </summary>
        public static string PublicClientId { get; private set; }

        
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        /// <summary>
        /// 設置同時使用 Cookie 和 Bearer Toekn 驗證
        /// </summary>
        /// <param name="app">IAppBuilder</param>
        public void ConfigureAuth(IAppBuilder app) {
            // Configure the db context and user manager to use a single instance per request
            app.CreatePerOwinContext(iTalkDbContext.Create);
            app.CreatePerOwinContext<UserManager>(UserManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            app.UseCookieAuthentication(new CookieAuthenticationOptions() {
                LoginPath = new PathString("/Admin/Account/Login")
            });
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Configure the application for OAuth based flow
            PublicClientId = "self";
            OAuthOptions = new OAuthAuthorizationServerOptions {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/api/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(14),
                AllowInsecureHttp = true
            };

            // Enable the application to use bearer tokens to authenticate users
            app.UseOAuthBearerTokens(OAuthOptions);

            // Uncomment the following lines to enable logging in with third party login providers
            //app.UseMicrosoftAccountAuthentication(
            //    clientId: "",
            //    clientSecret: "");

            //app.UseTwitterAuthentication(
            //    consumerKey: "",
            //    consumerSecret: "");

            //app.UseFacebookAuthentication(
            //    appId: "",
            //    appSecret: "");

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            //{
            //    ClientId = "",
            //    ClientSecret = ""
            //});
        }
    }
}
