using System.Collections.Generic;

namespace GoogleBooksAPI
{
    public class BooksData
    {
        public int TotalItems;
        public List<BookData> Items;
    }

    public class BookData
    {
        public string SelfLink;
        public VolumeInfo VolumeInfo;
    }

    public class VolumeInfo
    {
        public string Title;
        public string SubTitle;
        public List<string> Authors;
        public string Publisher;
        public string PublishedDate;
        public ImageLinks imageLinks;
        public int PageCount;
        public string InfoLink;
    }

    public class ImageLinks
    {
        public string SmallThumbnail;
    }
}

