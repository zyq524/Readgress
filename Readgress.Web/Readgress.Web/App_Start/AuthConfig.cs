using Microsoft.Web.WebPages.OAuth;
using System.Collections.Generic;
using System.Configuration;

namespace Readgress.Web
{
    public static class AuthConfig
    {
        public static void RegisterAuth()
        {
            // To let users of this site log in using their accounts from other sites such as Microsoft, Facebook, and Twitter,
            // you must update this site. For more information visit http://go.microsoft.com/fwlink/?LinkID=252166

            //OAuthWebSecurity.RegisterMicrosoftClient(
            //    clientId: "",
            //    clientSecret: "");

            //OAuthWebSecurity.RegisterTwitterClient(
            //    consumerKey: "",
            //    consumerSecret: "");

            Dictionary<string, object> FacebooksocialData = new Dictionary<string, object>();
            FacebooksocialData.Add("Icon", "Images/fb_login.png");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: ConfigurationManager.AppSettings["appId"],
                appSecret: ConfigurationManager.AppSettings["appSecret"],
                displayName: "Facebook",
                extraData: FacebooksocialData);

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
