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
    public class ProgressViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Bookmark> Bookmarks { get; private set; }

        private Progress progress;

        private bool hasNoBookmark = true;
        private bool isDataLoading = true;

        public ProgressViewModel()
        {
            this.Bookmarks = new ObservableCollection<Bookmark>();
            this.Bookmarks.CollectionChanged += Bookmarks_CollectionChanged;

            this.progress = new Progress();
        }

        private void Bookmarks_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyPropertyChanged("Bookmarks");
        }

        public Progress Progress
        {
            get
            {
                return this.progress;
            }
            private set
            {
                this.progress = value;
                NotifyPropertyChanged("Progress");
            }
        }

        public bool HasNoBookmark
        {
            get
            {
                return this.hasNoBookmark;
            }
            set
            {
                this.hasNoBookmark = value;
                NotifyPropertyChanged("HasNoBookmark");
            }
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

        public void LoadData(string isbn)
        {
            this.progress = new Progress();
            this.Bookmarks.Clear();

            StorageSettings settings = new StorageSettings();

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream stream = storage.OpenFile(App.LocalStorageFile, FileMode.Open, FileAccess.Read))
                {
                    XDocument doc = XDocument.Load(stream);
                    var progressElm = doc.Descendants("Progress").Where(
                         e => e.Attribute("UserName").Value == settings.FacebookUserName && e.Attribute("Isbn").Value == isbn).FirstOrDefault();
                    if (progressElm != null)
                    {
                        this.progress.UserName = settings.FacebookUserName;
                        this.progress.Isbn = isbn;
                        this.progress.Title = progressElm.Element("Title").Value;
                        this.progress.SubTitle = progressElm.Element("SubTitle").Value;
                        this.progress.Authors = progressElm.Element("Authors").Value;
                        this.progress.GoogleBookId = progressElm.Element("GoogleBookId").Value;
                        this.progress.CoverMedium = progressElm.Element("CoverMedium").Value;
                        this.progress.PublishedDate = Convert.ToDateTime(progressElm.Element("PublishedDate").Value).ToShortDateString();
                        this.progress.Publisher = progressElm.Element("Publisher").Value;
                        this.progress.PageCount = Convert.ToInt32(progressElm.Element("PageCount").Value);
                        this.progress.IsFinished = Convert.ToBoolean(progressElm.Element("IsFinished").Value);

                        this.progress.Bookmarks = new List<Bookmark>();

                        var bookmarkElms = progressElm.Descendants("Bookmark").OrderByDescending(b => (DateTime)b.Element("CreatedOn"));
                        foreach (var bookmarkElm in bookmarkElms)
                        {
                            Bookmark bookmark = new Bookmark();

                            bookmark.CreatedOn = Convert.ToDateTime(bookmarkElm.Element("CreatedOn").Value);
                            bookmark.PageNumber = Convert.ToInt32(bookmarkElm.Element("PageNumber").Value);

                            this.Bookmarks.Add(bookmark);
                        }
                        this.hasNoBookmark = this.Bookmarks.Count == 0;
                    }
                }
            }
            IsDataLoading = false;
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
