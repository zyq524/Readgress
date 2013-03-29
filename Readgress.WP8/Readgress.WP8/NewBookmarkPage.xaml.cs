using Microsoft.Phone.Controls;
using Readgress.WP8.Models;
using Readgress.WP8.Utils;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Readgress.WP8
{
    public partial class NewBookmarkPage : PhoneApplicationPage
    {
        private string hintText = "what page have you got up to?";
        private string isbn = string.Empty;
        private string lastPage = string.Empty;

        public NewBookmarkPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString["Isbn"].Length > 0)
            {
                this.isbn = NavigationContext.QueryString["Isbn"];
            }
            if (NavigationContext.QueryString.TryGetValue("LastPage", out lastPage))
            {
                PageNumberTB.Text = this.lastPage;
                PageNumberTB.IsEnabled = false;
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (PageNumberTB.Text == hintText || string.IsNullOrEmpty(PageNumberTB.Text) || PageNumberTB.Text.Contains("."))
            {
                MessageBox.Show("page number must be an integer");
            }
            else
            {
                int pageNumber = Convert.ToInt32(this.PageNumberTB.Text);
                DateTime createdOn = Convert.ToDateTime(this.When.Value);

                bool xmlChanged = false;

                StorageSettings settings = new StorageSettings();

                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    XDocument doc = null;
                    using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Open, FileAccess.Read))
                    {
                        doc = XDocument.Load(stream);
                        var existingBook = doc.Descendants("Progress").Where(
                             elm => elm.Attribute("UserName").Value == settings.FacebookUserName && elm.Attribute("Isbn").Value == isbn).FirstOrDefault();

                        if (existingBook != null)
                        {
                            if (!string.IsNullOrEmpty(lastPage))
                            {
                                existingBook.Element("IsFinished").SetValue(true);
                                var book = App.BookViewModel.ReadingBooks.Where(b => b.VolumeInfo.Isbn == isbn).First();
                                App.BookViewModel.ReadingBooks.Remove(book);
                                App.BookViewModel.FinishedBooks.Add(book);
                                App.BookViewModel.HasNoFinishedBook = false;
                                App.BookViewModel.HasNoReadingBook = App.BookViewModel.ReadingBooks.Count == 0;
                                App.ProgressViewModel.Progress.IsFinished = true;
                                NavigationService.Navigate(new Uri("/MainPage.xaml?goto=1", UriKind.Relative));
                            }
                            XElement bookmarkElm = new XElement("Bookmark",
                                new XElement("PageNumber", pageNumber),
                                new XElement("CreatedOn", createdOn));

                            XElement bookmarks = existingBook.Element("Bookmarks");
                            bookmarks.Add(bookmarkElm);
                            xmlChanged = true;
                            App.ProgressViewModel.Bookmarks.Insert(0, new Bookmark { CreatedOn = createdOn, PageNumber = pageNumber });
                            App.ProgressViewModel.HasNoBookmark = false;
                        }

                    }

                    if (doc != null && xmlChanged)
                    {
                        using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Open, FileAccess.Write))
                        {
                            doc.Save(stream);
                        }
                    }

                    NavigationService.GoBack();
                }

            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            NavigationService.GoBack();
        }

        //The foreground color of the text in PageNumberTB is set to Magenta when PageNumberTB
        //gets focus.
        private void PageNumberTB_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PageNumberTB.Text == hintText)
            {
                PageNumberTB.Text = "";
            }
        }
        //The foreground color of the text in PageNumberTB is set to Blue when PageNumberTB
        //loses focus. Also, if SearchTB loses focus and no text is entered, the
        //text "Search" is displayed.
        private void PageNumberTB_LostFocus(object sender, RoutedEventArgs e)
        {
            if (PageNumberTB.Text == String.Empty)
            {
                PageNumberTB.Text = hintText;
                SolidColorBrush Brush2 = new SolidColorBrush();
                Brush2.Color = Colors.Gray;
                PageNumberTB.Foreground = Brush2;
            }
        }

        private void DatePicker_ValueChanged(object sender, DateTimeValueChangedEventArgs e)
        {

        }
    }
}