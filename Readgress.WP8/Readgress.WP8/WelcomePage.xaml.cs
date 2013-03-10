using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Readgress.WP8
{
    public partial class WelcomePage : PhoneApplicationPage
    {
        public WelcomePage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
        }
    }
}