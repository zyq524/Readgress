using System;
using System.Collections.Generic;

namespace Readgress.WP8.Models
{
    public class Progress
    {
        public string UserName { get; set; }
        public string Isbn { get; set; }
        public string GoogleBookId { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Authors { get; set; }
        public string CoverMedium { get; set; }
        public int PageCount { get; set; }
        public string PublishedDate { get; set; }
        public string Publisher { get; set; }
        public bool IsFinished { get; set; }

        public List<Bookmark> Bookmarks { get; set; }
    }
}
