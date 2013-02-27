using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Newtonsoft.Json;
using Readgress.WP8.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Collections;

namespace Readgress.WP8
{
    public partial class BookListPage : PhoneApplicationPage
    {
        public BookListPage()
        {
            InitializeComponent();

            CreateApplicationBarItems();
        }

        public List<Book> Books { get; private set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string bookTitleToSearch = NavigationContext.QueryString["booktitle"];
            SearchBooksByTitle(bookTitleToSearch);
        }

        private void SearchBooksByTitle(string title)
        {
            if (!string.IsNullOrEmpty(title))
            {
                string openLibraryBooksUrl = @"http://readgress.azurewebsites.net/api/book/?Title=" + title;

                try
                {
                    WebClient webClient = new WebClient();
                    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
                    webClient.DownloadStringAsync(new Uri(openLibraryBooksUrl));

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Service is not available. Try it later.");
                }
            }
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }
            Books = JsonConvert.DeserializeObject<List<Book>>(e.Result);

            SearchProgressOverlay.Visibility = Visibility.Collapsed;

            OnBookListActivated();
            BookList.ItemsSource = Books;
        }

        #region MultiselectListbox item
        ApplicationBarIconButton select;
        ApplicationBarIconButton add;

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
            //while (BookList.SelectedItems.Count > 0)
            //{
            //    source.Remove((EmailObject)EmailList.SelectedItems[0]);
            //}
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