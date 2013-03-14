using Newtonsoft.Json;
using System;
using System.Net;

namespace GoogleBooksAPI
{
    public class Details : IDetails
    {
        private const string baseUrl = "https://www.googleapis.com/books/v1/volumes?q=";

        public BooksData FindBooksByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            BooksData books = new BooksData();

            var getUri = baseUrl + Uri.EscapeDataString(title);

            using (var webClient = new WebClient())
            {
                books = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri));
            }

            return books;
        }

        public BooksData FindBooksByTitleAndAuthor(string title, string author)
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

            var getUri = baseUrl + Uri.EscapeDataString(title) + "+inauthor:" + Uri.EscapeDataString(author);

            using (var webClient = new WebClient())
            {
                books = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri));
            }

            return books;
        }
    }
}
