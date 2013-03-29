using Coding4Fun.Toolkit.Controls;
using Microsoft.Phone.Controls;
using System;
using System.Windows.Input;

namespace Readgress.WP8
{
    public partial class SearchBookPage : PhoneApplicationPage
    {
        string mode = string.Empty;

        public SearchBookPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationContext.QueryString.TryGetValue("Mode", out mode);

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
                    App.SearchBooksViewModel.Reset();

                    NavigationService.Navigate(new Uri(string.Format("/BookListPage.xaml?booktitle={0}&mode={1}", Uri.EscapeUriString(SearchTextBox.Text), mode), UriKind.Relative));
                }
            }
        }

    }
}