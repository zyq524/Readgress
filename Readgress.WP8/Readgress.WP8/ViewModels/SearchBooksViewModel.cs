using Readgress.WP8.Models;
using Readgress.WP8.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Readgress.WP8.ViewModels
{
    public class SearchBooksViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> FoundBooks { get; set; }


        private int startIndex = 0;
        private int totalItems = 0;
        private bool hasMore = false;
        private string bookTitleToSearch = string.Empty;

        public SearchBooksViewModel()
        {
            this.FoundBooks = new ObservableCollection<Book>();
            this.FoundBooks.CollectionChanged += FoundBooks_CollectionChanged;
        }

        private void FoundBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FoundBooks");
        }

        public bool HasMore
        {
            get
            {
                return this.hasMore;
            }
            private set
            {
                this.hasMore = value;
                NotifyPropertyChanged("HasMore");
            }
        }

        //public void SearchBooksTotalItems(string title)
        //{
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        string booksUrl = ReadgressAPIEndpoints.BooksTotalItemsUrl + title;

        //        try
        //        {
        //            WebClient webClient = new WebClient();
        //            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SearchBooksTotalItemsCompleted);
        //            webClient.DownloadStringAsync(new Uri(booksUrl));

        //        }
        //        catch
        //        {
        //            MessageBox.Show("Service is not available. Try it later.");
        //        }
        //    }
        //}

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


        //private void SearchBooksByTitle(string title)
        //{
        //    if (!string.IsNullOrEmpty(title))
        //    {
        //        string booksUrl = ReadgressAPIEndpoints.BooksUrl + title + "&startIndex=" + startIndex;
        //        startIndex++;

        //        try
        //        {
        //            WebClient webClient = new WebClient();
        //            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(SearchBooksByTitleCompleted);
        //            webClient.DownloadStringAsync(new Uri(booksUrl));

        //        }
        //        catch
        //        {
        //            MessageBox.Show("Service is not available. Try it later.");
        //        }
        //    }
        //}

        //private void SearchBooksByTitleCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (e.Error != null)
        //    {
        //        return;
        //    }
        //    Books.AddRange(JsonConvert.DeserializeObject<List<Book>>(e.Result).Select(b => b.VolumeInfo).ToList());

        //    SearchProgressOverlay.Visibility = Visibility.Collapsed;

        //    OnBookListActivated();
        //    BookList.ItemsSource = Books;
        //}
        public void Reset()
        {
            startIndex = 0;
            totalItems = 0;
            bookTitleToSearch = string.Empty;
            FoundBooks = new ObservableCollection<Book>();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
