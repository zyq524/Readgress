using Readgress.WP8.Models;
using Readgress.WP8.Utils;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;

namespace Readgress.WP8.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Book> FinsihedBooks { get; private set; }
        public ObservableCollection<Book> FirstThreeReadingBooks { get; private set; }
        public ObservableCollection<Book> SecondThreeReadingBooks { get; private set; }

        public ObservableCollection<Book> ReadingBooks { get; private set; }

        private bool isDataLoading = true;
        private bool hasNoReadingBook = false;
        private bool hasNoFinishedBook = false;

        public BookViewModel()
        {
            this.FinsihedBooks = new ObservableCollection<Book>();
            this.FinsihedBooks.CollectionChanged += FinishedBooks_CollectionChanged;

            this.FirstThreeReadingBooks = new ObservableCollection<Book>();
            this.FirstThreeReadingBooks.CollectionChanged += FirstThreeReadingBooks_CollectionChanged;

            this.SecondThreeReadingBooks = new ObservableCollection<Book>();
            this.SecondThreeReadingBooks.CollectionChanged += SecondThreeReadingBooks_CollectionChanged;

            this.ReadingBooks = new ObservableCollection<Book>();
        }

        private void FinishedBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FinsihedBooks");
        }

        private void FirstThreeReadingBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FirstThreeReadingBooks");
        }

        private void SecondThreeReadingBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("SecondThreeReadingBooks");
        }

        public bool IsDataLoading
        {
            get 
            { 
                return this.isDataLoading; 
            }
            private set
            {
                this.isDataLoading = value;
                NotifyPropertyChanged("IsDataLoading");
            }
        }

        public bool HasNoReadingBook
        {
            get
            {
                return this.hasNoReadingBook;
            }
            private set
            {
                this.hasNoReadingBook = value;
                NotifyPropertyChanged("HasNoReadingBook");
            }
        }

        public bool HasNoFinishedBook
        {
            get
            {
                return this.hasNoFinishedBook;
            }
            private set
            {
                this.hasNoFinishedBook = value;
                NotifyPropertyChanged("HasNoFinishedBook");
            }
        }

        public void LoadData()
        {
        //    ReadingBooks.Add(new Book() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461563-M.jpg" }, IsFinished = false });
        //    ReadingBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = false });

            //ReadingBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = false });
            //ReadingBooks.Add(new Book() { Title = "Microsoft ASP.NET 2.0 Step By Step", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461594-M.jpg" }, IsFinished = false });

            //IsDataLoaded = true;
            try
            {
                StorageSettings settings = new StorageSettings();
                WebClient webClient = new WebClient();
                webClient.Headers["FacebookAccessToken"] = settings.FacebookAccessToken;

                webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_DownloadStringCompleted);
                webClient.DownloadStringAsync(new Uri(ReadgressAPIEndpoints.ReaderUrl));

            }
            catch
            {
                throw new Exception("Service is not available. Try it later.");
            }


        }

        private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            //if (e.Error != null)
            //{
            //    return;
            //}
            IsDataLoading = false;
            FinsihedBooks.Add(new Book() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461563-M.jpg" }, IsFinished = true });
            FinsihedBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = true });
            FinsihedBooks.Add(new Book() { Title = "Microsoft ASP.NET 2.0 Step By Step", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461594-M.jpg" }, IsFinished = true });
            FinsihedBooks.Add(new Book() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461563-M.jpg" }, IsFinished = true });
            FinsihedBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = true });
            FinsihedBooks.Add(new Book() { Title = "Microsoft ASP.NET 2.0 Step By Step", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461594-M.jpg" }, IsFinished = true });

            //FirstThreeReadingBooks.Add(new Book() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461563-M.jpg" }, IsFinished = false });
            //FirstThreeReadingBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = false });
            
            //SecondThreeReadingBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = false });
            //SecondThreeReadingBooks.Add(new Book() { Title = "Microsoft ASP.NET 2.0 Step By Step", Cover = new Cover() { Medium = @"http://covers.openlibrary.org/b/id/461594-M.jpg" }, IsFinished = false });

            HasNoFinishedBook = FinsihedBooks.Count == 0;
            HasNoReadingBook = ReadingBooks.Count == 0;
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
