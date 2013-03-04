using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Readgress.WP8.Utils;

namespace Readgress.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;

            ApplicationBar = Resources["readingAppBar"] as ApplicationBar;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageSettings settings = new StorageSettings();
            if (string.IsNullOrEmpty(settings.FacebookAccessToken))
            {
                NavigationService.Navigate(new Uri("/FacebookLoginPage.xaml", UriKind.Relative));
            }

            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void Panorama_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as Panorama).SelectedIndex)
            {
                case 0:
                    ApplicationBar = Resources["readingAppBar"] as ApplicationBar;
                    break;
                case 1:
                    ApplicationBar = Resources["finishedAppBar"] as ApplicationBar;
                    break;
                default:
                    break;
            }
        }

        private void AddNewButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/SearchBookPage.xaml", UriKind.Relative));
        }

        private void SearchBookButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Search book works!");
        }
    }
}