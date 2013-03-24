using Facebook;
using Microsoft.Phone.Controls;
using Readgress.WP8.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Readgress.WP8
{
    public partial class FacebookLoginPage : PhoneApplicationPage
    {
        private const string AppId = "115878675265829";

        /// <summary>
        /// Extended permissions is a comma separated list of permissions to ask the user.
        /// </summary>
        /// <remarks>
        /// For extensive list of available extended permissions refer to 
        /// https://developers.facebook.com/docs/reference/api/permissions/
        /// </remarks>
        private const string ExtendedPermissions = "user_about_me,read_stream,publish_stream";

        private readonly FacebookClient _fb = new FacebookClient();

        public FacebookLoginPage()
        {
            InitializeComponent();
        }

        private void webBrowser1_Loaded(object sender, RoutedEventArgs e)
        {
            var loginUrl = GetFacebookLoginUrl(AppId, ExtendedPermissions);
            webBrowser1.Navigate(loginUrl);
        }

        private Uri GetFacebookLoginUrl(string appId, string extendedPermissions)
        {
            var parameters = new Dictionary<string, object>();
            parameters["client_id"] = appId;
            parameters["redirect_uri"] = "https://www.facebook.com/connect/login_success.html";
            parameters["response_type"] = "token";
            parameters["display"] = "touch";

            // add the 'scope' only if we have extendedPermissions.
            if (!string.IsNullOrEmpty(extendedPermissions))
            {
                // A comma-delimited list of permissions
                parameters["scope"] = extendedPermissions;
            }

            return _fb.GetLoginUrl(parameters);
        }

        private void webBrowser1_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            FacebookOAuthResult oauthResult;
            if (!_fb.TryParseOAuthCallbackUrl(e.Uri, out oauthResult))
            {
                return;
            }

            if (oauthResult.IsSuccess)
            {
                LoginSucceded(oauthResult);
            
                //NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));

            }
            else
            {
                // user cancelled
                MessageBox.Show(oauthResult.ErrorDescription);
            }
        }

        private void LoginSucceded(FacebookOAuthResult oauthResult)
        {
            var fb = new FacebookClient(oauthResult.AccessToken);

            fb.GetCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    Dispatcher.BeginInvoke(() => MessageBox.Show(e.Error.Message));
                    return;
                }
                var result = (IDictionary<string, object>)e.GetResultData();

                StorageSettings settings = new StorageSettings();
                settings.FacebookAccessToken = oauthResult.AccessToken;
                settings.FacebookAccessTokenExpires = oauthResult.Expires;
                settings.FacebookUserName = result["username"].ToString();

                SaveNewUserToXmlFile(result["username"].ToString());

                Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
            };

            fb.GetAsync("me");
        }

        private void SaveNewUserToXmlFile(string userName)
        {
            using(IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.FileExists(App.LocalStorageFile))
                {
                    XDocument doc = new XDocument(
                        new XElement("Readgress",
                            new XElement("Readers",
                                new XElement("Reader",
                                    new XElement("UserName", userName))),
                            new XElement("Progresses")));

                    using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Create))
                    {
                        doc.Save(stream);
                    }
                }
                else
                {
                    XDocument doc = XDocument.Load(App.LocalStorageFile);
                    var reader = doc.Element("Readgress").Descendants("Reader").Where(e => e.Element("UserName").Value == userName).FirstOrDefault();
                    if (reader == null)
                    {
                        doc.Element("Readgress").Element("Readers").Add(new XElement("Reader", new XElement("UserName", userName)));
                    }

                    using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.OpenOrCreate))
                    {
                        doc.Save(stream);
                    }
                }
            }
        }
    }
}