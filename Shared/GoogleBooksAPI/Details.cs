using Newtonsoft.Json;
using System;
using System.Net;

namespace GoogleBooksAPI
{
    public class Details : IDetails
    {
        private const string baseUrl = @"https://www.googleapis.com/books/v1/volumes?q=";
        private const string fields = @"&fields=totalItems,items(selfLink, volumeInfo(title,subtitle,authors,publisher,publishedDate,imageLinks/smallThumbnail,pageCount))";
        private const string sort = @"&orderBy=relevance";

        public BooksData FindBooksByTitle(string title, int startIndex = 0, int maxResults = 10)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            BooksData books = new BooksData();

            var getUri = baseUrl + Uri.EscapeDataString(title) + fields + sort + @"&startIndex=" + startIndex + @"&maxResults=" + maxResults;

            using (var webClient = new GZipWebClient())
            {
                books = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri));
            }

            return books;
        }

        public BooksData FindBooksByTitleAndAuthor(string title, string author, int startIndex = 0, int maxResults = 10)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentNullException("title");
            }

            BooksData books = new BooksData();

            var getUri = baseUrl + Uri.EscapeDataString(title) + "+inauthor:" + Uri.EscapeDataString(author) + fields + sort + @"&startIndex=" + startIndex + @"&maxResults=" + maxResults;

            using (var webClient = new GZipWebClient())
            {
                books = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri));
            }

            return books;
        }
    }
}
