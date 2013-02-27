using Microsoft.Phone.Controls;
using Newtonsoft.Json;
using Readgress.WP8.Models;
using System;
using System.Net;
using System.Windows;
using System.Windows.Input;
using System.Collections.Generic;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Windows.Media;
using Coding4Fun.Toolkit.Controls;

namespace Readgress.WP8
{
    public partial class SearchBookPage : PhoneApplicationPage
    {
        public SearchBookPage()
        {
            InitializeComponent();


        }

        private void messagePrompt_Completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
  
        }

        private void SearchTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (!string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    NavigationService.Navigate(new Uri(string.Format("/BookListPage.xaml?booktitle={0}", Uri.EscapeUriString(SearchTextBox.Text)), UriKind.Relative));
                    //SearchBooksByTitle(SearchTextBox.Text);

                    //this.Focus();
                    //SearchTextBox.IsEnabled = false;

                    //NavigationService.Navigate(new Uri("/BookListPage.xaml", UriKind.Relative));
                    //SearchProgressOverlay.Visibility = Visibility.Visible;
                }
            }
        }

    }
}