using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Readgress.WP8
{
    public partial class BookProgressPage : PhoneApplicationPage
    {
        private string isbn;

        public BookProgressPage()
        {
            InitializeComponent();

            DataContext = App.ProgressViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.TryGetValue("Isbn", out isbn))
            {
                App.ProgressViewModel.LoadData(isbn);
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch ((sender as Pivot).SelectedIndex)
            {
                case 0:
                    ApplicationBar.IsVisible = false;
                    break;
                case 1:
                    if (!App.ProgressViewModel.Progress.IsFinished)
                    {
                        ApplicationBar.IsVisible = true;
                    }
                    else
                    {
                        foreach (var button in ApplicationBar.Buttons)
                        {
                            ((ApplicationBarIconButton)button).IsEnabled = false; 
                        }

                        ApplicationBar.IsMenuEnabled = false;
                    }
                    break;
                default:
                    break;
            }
        }

        private void AddNewButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/NewBookmarkPage.xaml?Isbn={0}", isbn), UriKind.Relative));
        }

        private void CompleteButton_Click(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri(string.Format("/NewBookmarkPage.xaml?Isbn={0}&LastPage={1}", isbn, App.ProgressViewModel.Progress.PageCount), UriKind.Relative));
        }
    }
}