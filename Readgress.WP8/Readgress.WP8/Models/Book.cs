using System;
using System.Collections.Generic;

namespace Readgress.WP8.Models
{
    public class Books
    {
        public int TotalItems { get; set; }
        public List<Book> Items { get; set; }
    }

    public class Book
    {
        public string Id { get; set; }
        public VolumeInfo VolumeInfo { get; set; }
    }

    public class VolumeInfo
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Publisher { get; set; }
        public string PublishedDate { get; set; }
        public int PageCount { get; set; }
        public string InfoLink { get; set; }

        public ImageLinks ImageLinks;
        public List<IndustryIdentifiers> IndustryIdentifiers;
        public List<string> Authors;

        public bool IsFinished { get; set; }

        public string TrimmedTitle
        {
            get
            {
                return !string.IsNullOrEmpty(Title) && Title.Length > 17 ? Title.Substring(0, 17) + "..." : Title;
            }
        }

        public string AuthorsStr
        {
            get
            {
                string a = Authors != null ? string.Join(",", Authors) : string.Empty;
                return a.Length > 22 ? a.Substring(0, 20) + "..." : a;
            }
        }

        public string Cover_Small
        {
            get
            {
                return ImageLinks == null || string.IsNullOrEmpty(ImageLinks.SmallThumbnail) ? "/Assets/cover_s.png" : ImageLinks.SmallThumbnail;
            }
        }

        public string Cover_Medium
        {
            get
            {
                return ImageLinks == null || string.IsNullOrEmpty(ImageLinks.SmallThumbnail) ? "/Assets/cover_m.png" : ImageLinks.SmallThumbnail.Replace("&zoom=5", "&zoom=1");
            }
        }

        public string Isbn
        {
            get
            {
                return IndustryIdentifiers.Find(i => i.Type == "ISBN_10").identifier;
            }
        }
    }

    public class ImageLinks
    {
        public string SmallThumbnail { get; set; }
    }

    public class IndustryIdentifiers
    {
        public string Type;
        public string identifier;
    }
}
