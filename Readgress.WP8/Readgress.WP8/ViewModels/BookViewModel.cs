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
        private bool hasNoFinishedBook = true;
        private bool hasTooManyReadingBooks = false;
        private string backgroundSource = string.Empty;
        
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
            int index = 0;
            FirstThreeReadingBooks = new ObservableCollection<Book>();
            SecondThreeReadingBooks = new ObservableCollection<Book>();
            foreach (var book in ReadingBooks)
            {
                if (index < 3)
                {
                    FirstThreeReadingBooks.Add(book);
                }
                else if (index < 6)
                {
                    SecondThreeReadingBooks.Add(book);
                }
                index++;
            }
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
            set
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
            set
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
            set
            {
                this.hasTooManyReadingBooks = value;
                NotifyPropertyChanged("HasTooManyReadingBooks");
            }
        }

        public string BackgroundSource
        {
            get
            {
                return this.backgroundSource;
            }
            private set
            {
                this.backgroundSource = value;
                NotifyPropertyChanged("BackgroundSource");
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
                                //if (index < 3)
                                //{
                                //    FirstThreeReadingBooks.Add(book);
                                //}
                                //else if (index < 6)
                                //{
                                //    SecondThreeReadingBooks.Add(book);
                                //}
                                ReadingBooks.Add(book);
                                index++;
                            }
                        }
                    }
                    this.hasNoFinishedBook = FinishedBooks.Count == 0;
                    this.hasNoReadingBook = ReadingBooks.Count == 0;
                    this.hasTooManyReadingBooks = ReadingBooks.Count > 6;
                }

                this.isDataLoading = false;
            }
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
