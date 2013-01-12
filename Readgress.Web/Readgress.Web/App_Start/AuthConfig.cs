using Microsoft.Web.WebPages.OAuth;
using System.Collections.Generic;

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
            FacebooksocialData.Add("Icon", "Images/facebook.png");

            OAuthWebSecurity.RegisterFacebookClient(
                appId: "327334420706146",
                appSecret: "c4b24ab6f876cce03ab2026474a0115b",
                displayName: "Facebook",
                extraData: FacebooksocialData);

            //OAuthWebSecurity.RegisterGoogleClient();
        }
    }
}
