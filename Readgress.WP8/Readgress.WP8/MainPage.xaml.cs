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
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Readgress.WP8
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.BookViewModel;

            ApplicationBar = Resources["readingAppBar"] as ApplicationBar;
        }

        // Load data for the ViewModel Items
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            StorageSettings settings = new StorageSettings();

            if (string.IsNullOrEmpty(settings.FacebookAccessToken))
            {
                NavigationService.Navigate(new Uri("/WelcomePage.xaml", UriKind.Relative));
            }

            else
            {
                if (App.BookViewModel.IsDataLoading)
                {
                    App.BookViewModel.LoadData();
                }

                string itemIndex;
                if (NavigationContext.QueryString.TryGetValue("goto", out itemIndex))
                {
                    ReadgressPanorama.DefaultItem = ReadgressPanorama.Items[Convert.ToInt32(itemIndex)];
                }
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
            if (App.BookViewModel.ReadingBooks.Count == 6)
            {
                MessageBox.Show("only 6 reading books are allowed");
            }
            else
            {
                NavigationService.Navigate(new Uri("/SearchBookPage.xaml?Mode=Internet", UriKind.Relative));
            }
        }

        private void SearchBookButton_Click(object sender, EventArgs e)
        {
            if (App.BookViewModel.FinishedBooks.Count > 0)
            {
                NavigationService.Navigate(new Uri("/SearchBookPage.xaml?Mode=Local", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("you have no completed book");
            }
        }

        private void BookProgressButton_Click(object sender, RoutedEventArgs e)
        {
            var element = sender as Button;
            NavigationService.Navigate(new Uri(string.Format("/BookProgressPage.xaml?Isbn={0}", element.Tag.ToString()), UriKind.Relative));
        }

    }
}