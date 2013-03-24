using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Readgress.WP8.Models;
using Readgress.WP8.Utils;
using Readgress.WPPostClient;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml.Linq;

namespace Readgress.WP8
{
    public partial class BookListPage : PhoneApplicationPage
    {
        public BookListPage()
        {
            InitializeComponent();

            CreateApplicationBarItems();
            DataContext = App.SearchBooksViewModel;
        }

        private int startIndex;
        private int totalItems;
        private string bookTitleToSearch;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            startIndex = 0;
            totalItems = 0;

            bookTitleToSearch = NavigationContext.QueryString["booktitle"];
            SearchBooksTotalItems(bookTitleToSearch);
        }

        private void SearchBooksTotalItems(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                string booksUrl = ReadgressAPIEndpoints.BooksTotalItemsUrl + title;
                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += (sender, e) =>
                    {
                        if (e.Error == null)
                        {
                            totalItems = JsonConvert.DeserializeObject<int>(e.Result);

                            if (totalItems > 0)
                            {
                                SearchBooksByTitle(bookTitleToSearch);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Service is not available. Try it later.");
                        }
                    };
                webClient.DownloadStringAsync(new Uri(booksUrl), UriKind.Absolute);
                //try
                //{
                //    WebClient webClient = new WebClient();
                //    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SearchBooksTotalItemsCompleted);
                //    webClient.DownloadStringAsync(new Uri(booksUrl));

                //}
                //catch
                //{
                //    MessageBox.Show("Service is not available. Try it later.");
                //}
            }
        }

        //private void SearchBooksTotalItemsCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        return;
        //    }
        //    totalItems = JsonConvert.DeserializeObject<int>(e.Result);

        //    if (totalItems > 0)
        //    {
        //        SearchBooksByTitle(bookTitleToSearch);
        //    }
        //}


        private void SearchBooksByTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                string booksUrl = ReadgressAPIEndpoints.BooksUrl + title + "&startIndex=" + startIndex;
                startIndex += 10;

                WebClient webClient = new WebClient();
                webClient.DownloadStringCompleted += (sender, e) =>
                {
                    if (e.Error == null)
                    {
                        var books = JsonConvert.DeserializeObject<List<Book>>(e.Result).ToList();

                        foreach (var book in books)
                        {
                            App.SearchBooksViewModel.FoundBooks.Add(book);
                        }
                        SearchProgressOverlay.Visibility = Visibility.Collapsed;

                        OnBookListActivated();
                    }
                    else
                    {
                        MessageBox.Show("Service is not available. Try it later.");
                    }
                };
                webClient.DownloadStringAsync(new Uri(booksUrl), UriKind.Absolute);

                //try
                //{
                //    WebClient webClient = new WebClient();
                //    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SearchBooksByTitleCompleted);
                //    webClient.DownloadStringAsync(new Uri(booksUrl));

                //}
                //catch
                //{
                //    MessageBox.Show("Service is not available. Try it later.");
                //}
            }
        }

        //private void SearchBooksByTitleCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        return;
        //    }
        //    var books = JsonConvert.DeserializeObject<List<Book>>(e.Result).ToList();

        //    foreach (var book in books)
        //    {
        //        App.SearchBooksViewModel.FoundBooks.Add(book.VolumeInfo);
        //    }
        //    SearchProgressOverlay.Visibility = Visibility.Collapsed;

        //    OnBookListActivated();
        //}

        #region MultiselectListbox item
        ApplicationBarIconButton select;
        ApplicationBarIconButton add;
        ApplicationBarIconButton refresh;

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            base.OnBackKeyPress(e);
            OnBackKeyPressed(e);
        }

        /// <summary>
        /// Creates ApplicationBar items for book list
        /// </summary>
        private void CreateApplicationBarItems()
        {
            select = new ApplicationBarIconButton();
            select.IconUri = new Uri("/Assets/AppBar/select.png", UriKind.RelativeOrAbsolute);
            select.Text = "select";
            select.Click += OnSelectClick;

            add = new ApplicationBarIconButton();
            add.IconUri = new Uri("/Assets/AppBar/add.png", UriKind.RelativeOrAbsolute);
            add.Text = "add";
            add.Click += OnAddClick;

            refresh = new ApplicationBarIconButton();
            refresh.IconUri = new Uri("/Assets/AppBar/refresh.png", UriKind.RelativeOrAbsolute);
            refresh.Text = "more";
            refresh.Click += OnRefreshClick;
        }

        /// <summary>
        /// Called when Book list is activated : makes sure that selection is disabled and updates the application bar
        /// </summary>
        void OnBookListActivated()
        {
            if (BookList.IsSelectionEnabled)
            {
                BookList.IsSelectionEnabled = false; // Will update the application bar too
            }
            else
            {
                SetupApplicationBar();
            }
        }

        /// <summary>
        /// Configure ApplicationBar items for book list
        /// </summary>
        private void SetupApplicationBar()
        {
            ClearApplicationBar();

            if (BookList.IsSelectionEnabled)
            {
                ApplicationBar.Buttons.Add(add);
                UpdateApplicationBar();
            }
            else
            {
                if (totalItems > startIndex)
                {
                    ApplicationBar.Buttons.Add(refresh);
                }

                ApplicationBar.Buttons.Add(select);
            }
            ApplicationBar.IsVisible = true;
        }

        /// <summary>
        /// Resets the application bar
        /// </summary>
        void ClearApplicationBar()
        {
            while (ApplicationBar.Buttons.Count > 0)
            {
                ApplicationBar.Buttons.RemoveAt(0);
            }

            while (ApplicationBar.MenuItems.Count > 0)
            {
                ApplicationBar.MenuItems.RemoveAt(0);
            }
        }

        /// <summary>
        /// Updates the book list Application bar items depending on selection
        /// </summary>
        private void UpdateApplicationBar()
        {
            if (BookList.IsSelectionEnabled)
            {
                bool hasSelection = ((BookList.SelectedItems != null) && (BookList.SelectedItems.Count > 0));
                add.IsEnabled = hasSelection;
            }
        }

        /// <summary>
        /// Back Key Pressed = leaves the selection mode
        /// </summary>
        /// <param name="e"></param>
        protected void OnBackKeyPressed(CancelEventArgs e)
        {
            if (BookList.IsSelectionEnabled)
            {
                BookList.IsSelectionEnabled = false;
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Passes the book list in selection mode
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSelectClick(object sender, EventArgs e)
        {
            BookList.IsSelectionEnabled = true;
        }

        /// <summary>
        /// Adds selected items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnAddClick(object sender, EventArgs e)
        {
            IList source = BookList.ItemsSource as IList;
            int selectedItemsCount = BookList.SelectedItems.Count;
            for (int i = 0; i < selectedItemsCount; i++)
            {
                AddNewProgress((Book)BookList.SelectedItems[i]);
            }
        }

        void AddNewProgress(Book book)
        {
            StorageSettings settings = new StorageSettings();

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                XDocument doc = null;
                using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Open, FileAccess.Read))
                {
                    doc = XDocument.Load(stream);
                    var existingBook = doc.Descendants("Progress").Where(
                         e => e.Attribute("UserName").Value == settings.FacebookUserName && e.Attribute("Isbn").Value == book.VolumeInfo.Isbn).FirstOrDefault();
                    if (existingBook == null)
                    {
                        XElement progressElm = new XElement("Progress",
                            new XAttribute("UserName", settings.FacebookUserName),
                            new XAttribute("Isbn", book.VolumeInfo.Isbn),
                            new XElement("GoogleBookId", book.Id),
                            new XElement("Title", book.VolumeInfo.Title),
                            new XElement("SubTitle", book.VolumeInfo.SubTitle),
                            new XElement("Authors", book.VolumeInfo.AuthorsStr),
                            new XElement("CoverMedium", book.VolumeInfo.Cover_Medium),
                            new XElement("IsFinished", false),
                            new XElement("Bookmarks"));
                        XElement progresses = doc.Descendants("Progresses").First();
                        progresses.Add(progressElm);
                    }
                    else
                    {
                        doc = null;
                        MessageBox.Show("You have added this book.");
                    }

                }

                if (doc != null)
                {
                    using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Open, FileAccess.Write))
                    {
                        doc.Save(stream);
                    }
                }
                BookList.IsSelectionEnabled = false;
            }
        }

        void PostNewProgress(Book book)
        {
            StorageSettings settings = new StorageSettings();
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("UserName", settings.FacebookUserName);
            parameters.Add("Isbn", book.VolumeInfo.Isbn);
            parameters.Add("GoogleBookId", book.Id);
            parameters.Add("IsFinished", false);

            PostClient client = new PostClient(parameters);
            client.DownloadStringCompleted += (sender, e) =>
            {
                if (e.Error == null)
                {
                    //Process the result...
                    string data = e.Result;
                }
                else
                {
                    MessageBox.Show("Service is not available. Try it later.");
                }
            };
            client.DownloadStringAsync(new Uri(ReadgressAPIEndpoints.ProgressUrl, UriKind.Absolute), settings.FacebookAccessToken);
        }

        /// <summary>
        /// Adds more items
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnRefreshClick(object sender, EventArgs e)
        {
            SearchBooksByTitle(bookTitleToSearch);
        }

        /// <summary>
        /// Adjusts the user interface according to the number of selected books
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBookListSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateApplicationBar();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBookListIsSelectionEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            SetupApplicationBar();
        }

        /// <summary>
        /// Tap on an item : depending on the selection state, either unselect it or consider it as read
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnItemContentTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            //EmailObject item = ((FrameworkElement)sender).DataContext as EmailObject;
            //if (item != null)
            //{
            //    item.Unread = false;
            //}
        }

        #endregion
    }
}