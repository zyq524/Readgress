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
        public string Id;
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
        public List<IndustryIdentifiers> IndustryIdentifiers;
    }

    public class ImageLinks
    {
        public string SmallThumbnail;
    }

    public class IndustryIdentifiers
    {
        public string Type;
        public string identifier;
    }
}

