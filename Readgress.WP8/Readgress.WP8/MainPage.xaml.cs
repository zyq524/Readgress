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

            if (App.BookViewModel.IsDataLoading)
            {
                App.BookViewModel.LoadData();
            }

            //double readingBookNum = App.BookViewModel.ReadingBooks.Count();

            //for (int i = 0; i < Math.Floor(readingBookNum / 2); i++)
            //{
            //    StackPanel sp = new StackPanel();
            //    sp.Orientation = System.Windows.Controls.Orientation.Horizontal;
            //    sp.HorizontalAlignment = HorizontalAlignment.Left;
            //    if (i > 0)
            //    {
            //        sp.Margin = new Thickness(0, 12, 0, 0);
            //    }
            //    for (int j = i * 2; j < i * 2 + 2; j++)
            //    {
            //        var book = App.BookViewModel.ReadingBooks[j];
            //        Image cover = new Image();
            //        BitmapImage ImgSource = new BitmapImage(new Uri(
            //            book.Cover_Medium));
            //        cover.Source = ImgSource;
            //        cover.Height = 300;
            //        cover.Width = 200;
            //        cover.VerticalAlignment = VerticalAlignment.Top;
            //        cover.Margin = new Thickness(10, 0, 0, 0);
            //        sp.Children.Add(cover);
            //    }
            //    ReadingBooksStackPanel.Children.Add(sp);
            //}
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