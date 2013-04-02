using Microsoft.Phone.Controls;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Navigation;

namespace Readgress.WP8
{
    public partial class WelcomePage : PhoneApplicationPage
    {
        public WelcomePage()
        {
            InitializeComponent();

            BackKeyPress += OnBackKeyPressed;
        }

        private void OnBackKeyPressed(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Do you want to exit?", "Attention!",
                                          MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {
                while (NavigationService.CanGoBack)
                {
                    NavigationService.RemoveBackEntry();
                }
                return;
            }
            e.Cancel = true;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
        }
    }
}