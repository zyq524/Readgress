using Readgress.WP8.Models;
using Readgress.WP8.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Xml.Linq;

namespace Readgress.WP8.ViewModels
{
    public class BookViewModel : INotifyPropertyChanged
    {

        public ObservableCollection<Book> FinishedBooks { get; private set; }
        public ObservableCollection<Book> FirstThreeReadingBooks { get; private set; }
        public ObservableCollection<Book> SecondThreeReadingBooks { get; private set; }

        public ObservableCollection<Book> ReadingBooks { get; private set; }

        private bool isDataLoading = true;
        private bool hasNoReadingBook = false;
        private bool hasNoFinishedBook = false;
        private bool hasTooManyReadingBooks = false;
        
        public BookViewModel()
        {
            this.FinishedBooks = new ObservableCollection<Book>();
            this.FinishedBooks.CollectionChanged += FinishedBooks_CollectionChanged;

            this.FirstThreeReadingBooks = new ObservableCollection<Book>();
            this.FirstThreeReadingBooks.CollectionChanged += FirstThreeReadingBooks_CollectionChanged;

            this.SecondThreeReadingBooks = new ObservableCollection<Book>();
            this.SecondThreeReadingBooks.CollectionChanged += SecondThreeReadingBooks_CollectionChanged;

            this.ReadingBooks = new ObservableCollection<Book>();
            this.ReadingBooks.CollectionChanged += ReadingBooks_CollectionChanged;
        }

        private void FinishedBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FinishedBooks");
        }

        private void FirstThreeReadingBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("FirstThreeReadingBooks");
        }

        private void SecondThreeReadingBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("SecondThreeReadingBooks");
        }

        private void ReadingBooks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("ReadingBooks");
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

        public bool HasTooManyReadingBooks
        {
            get
            {
                return this.hasTooManyReadingBooks;
            }
            private set
            {
                this.hasTooManyReadingBooks = value;
                NotifyPropertyChanged("HasTooManyReadingBooks");
            }
        }

        public void LoadData()
        {
            StorageSettings settings = new StorageSettings();

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Open, FileAccess.Read))
                {
                    XDocument doc = XDocument.Load(stream);
                    var progresses = doc.Descendants("Progress").Where(
                         e => e.Attribute("UserName").Value == settings.FacebookUserName);
                    if (progresses != null)
                    {
                        int index = 0;
                        foreach (var progress in progresses)
                        {
                            bool isFinished = Convert.ToBoolean(progress.Element("IsFinished").Value);
                            var book = new Book
                            {
                                Id = progress.Element("GoogleBookId").Value,
                                VolumeInfo = new VolumeInfo
                                {
                                    Title = progress.Element("Title").Value,
                                    SubTitle = progress.Element("SubTitle").Value,
                                    Authors = progress.Element("Authors").Value.Split(new char[] { ',' }).ToList(),
                                    IsFinished = isFinished,
                                    ImageLinks = new ImageLinks { SmallThumbnail = progress.Element("CoverMedium").Value.Replace("&zoom=1", "&zoom=5") },
                                    IndustryIdentifiers = new List<IndustryIdentifiers>() { new IndustryIdentifiers { Type = "ISBN_10", identifier = progress.Attribute("Isbn").Value } }
                                }
                            };
                            if (isFinished)
                            {
                                FinishedBooks.Add(book);
                            }
                            else
                            {
                                if (index < 3)
                                {
                                    FirstThreeReadingBooks.Add(book);
                                }
                                else if (index < 6)
                                {
                                    SecondThreeReadingBooks.Add(book);
                                }
                                ReadingBooks.Add(book);
                                index++;
                            }
                        }
                    }
                    this.hasNoFinishedBook = FinishedBooks.Count == 0 ? true : false;
                    this.hasNoReadingBook = ReadingBooks.Count == 0 ? true : false;
                    this.hasTooManyReadingBooks = ReadingBooks.Count > 4;
                }
            }

            //ReadingBooks.Add(new Book() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://ImageLinkss.openlibrary.org/b/id/461563-M.jpg" }, IsFinished = false });
            //ReadingBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://ImageLinkss.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = false });

            //ReadingBooks.Add(new Book() { Title = "The VaultReports.com Employer Profile for Job Seekers", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://ImageLinkss.openlibrary.org/b/id/2934627-M.jpg" }, IsFinished = false });
            //ReadingBooks.Add(new Book() { Title = "Microsoft ASP.NET 2.0 Step By Step", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://ImageLinkss.openlibrary.org/b/id/461594-M.jpg" }, IsFinished = false });

            //IsDataLoaded = true;
            //try
            //{
            //    StorageSettings settings = new StorageSettings();
            //    WebClient webClient = new WebClient();
            //    webClient.Headers["FacebookAccessToken"] = settings.FacebookAccessToken;

            //    webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(WebClient_DownloadStringCompleted);
            //    webClient.DownloadStringAsync(new Uri(ReadgressAPIEndpoints.ReaderUrl));

            //}
            //catch
            //{
            //    throw new Exception("Service is not available. Try it later.");
            //}


        }

        //private void WebClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    //if (e.Error != null)
        //    //{
        //    //    return;
        //    //}
        //    IsDataLoading = false;
        //    FinishedBooks.Add(new VolumeInfo() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks4.books.google.com/books?id=zmFJq_-3vZAC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api" }, IsFinished = true });
        //    FinsihedBooks.Add(new VolumeInfo() { Title = "The VaultReports.com Employer Profile for Job Seekers", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks7.books.google.com/books?id=nZSaSwggmw8C&printsec=frontcover&img=1&zoom=5&source=gbs_api" }, IsFinished = true });
        //    FinsihedBooks.Add(new VolumeInfo() { Title = "Microsoft ASP.NET 2.0 Step By Step", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks2.books.google.com/books?id=PFG22kTSiPwC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api" }, IsFinished = true });
        //    FinsihedBooks.Add(new VolumeInfo() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks4.books.google.com/books?id=zmFJq_-3vZAC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api" }, IsFinished = true });
        //    FinsihedBooks.Add(new VolumeInfo() { Title = "The VaultReports.com Employer Profile for Job Seekers", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks7.books.google.com/books?id=nZSaSwggmw8C&printsec=frontcover&img=1&zoom=5&source=gbs_api" }, IsFinished = true });
        //    FinsihedBooks.Add(new VolumeInfo() { Title = "Microsoft ASP.NET 2.0 Step By Step", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks2.books.google.com/books?id=PFG22kTSiPwC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api" }, IsFinished = true });

        //    FirstThreeReadingBooks.Add(new VolumeInfo() { Title = "The VaultReports.com Employer Profile for Job Seekers", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks7.books.google.com/books?id=nZSaSwggmw8C&printsec=frontcover&img=1&zoom=5&source=gbs_api" }, IsFinished = true });
        //    FirstThreeReadingBooks.Add(new VolumeInfo() { Title = "Microsoft ASP.NET 2.0 Step By Step", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks2.books.google.com/books?id=PFG22kTSiPwC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api" }, IsFinished = true });

        //    SecondThreeReadingBooks.Add(new VolumeInfo() { Title = "Microsoft ASP.NET 2.0 Step By Step", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://bks2.books.google.com/books?id=PFG22kTSiPwC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api" }, IsFinished = true });
        //    SecondThreeReadingBooks.Add(new VolumeInfo() { Title = "Microsoft .NET Development for Microsoft Office (Office/Progmng/Net)", ImageLinks = new ImageLinks() { SmallThumbnail = @"http://ImageLinkss.openlibrary.org/b/id/461563-M.jpg" }, IsFinished = true });
           
        //    HasNoFinishedBook = FinsihedBooks.Count == 0;
        //    HasNoReadingBook = ReadingBooks.Count == 0;

        //    HasTooManyReadingBooks = ReadingBooks.Count > 4;
        //}

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
