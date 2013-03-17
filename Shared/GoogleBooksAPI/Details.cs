using Newtonsoft.Json;
using System;
using System.Net;

namespace GoogleBooksAPI
{
    public class Details : IDetails
    {
        private const string baseUrl = @"https://www.googleapis.com/books/v1/volumes";
        private const string searchBaseUrl = baseUrl + @"?q=";
        private const string itemFields = @"&fields=items(id, volumeInfo(title,subtitle,authors,publisher,publishedDate,imageLinks/smallThumbnail,pageCount,infoLink,industryIdentifiers))";
        private const string totalItemsField = @"&fields=totalItems";
        private const string sort = @"&orderBy=relevance";

        public BooksData FindBooksByTitle(string title, int startIndex = 0, int maxResults = 10)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            BooksData books = new BooksData();

            var getUri = searchBaseUrl + Uri.EscapeDataString(title) + itemFields + sort + @"&startIndex=" + startIndex + @"&maxResults=" + maxResults;

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

            var getUri = searchBaseUrl + Uri.EscapeDataString(title) + "+inauthor:" + Uri.EscapeDataString(author) + itemFields + sort + @"&startIndex=" + startIndex + @"&maxResults=" + maxResults;

            using (var webClient = new GZipWebClient())
            {
                books = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri));
            }

            return books;
        }

        public int FindBooksTotalItemsByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            int totalItems = 0;

            var getUri = searchBaseUrl + Uri.EscapeDataString(title) + totalItemsField;

            using (var webClient = new GZipWebClient())
            {
                totalItems = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri)).TotalItems;
            }

            return totalItems;

        }

        public int FindBooksTotalItemsByTitleAndAuthor(string title, string author)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            if (string.IsNullOrEmpty(author))
            {
                throw new ArgumentNullException("title");
            }

            int totalItems = 0;

            var getUri = searchBaseUrl + Uri.EscapeDataString(title) + "+inauthor:" + Uri.EscapeDataString(author) + totalItemsField;

            using (var webClient = new GZipWebClient())
            {
                totalItems = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri)).TotalItems;
            }

            return totalItems;
        }

        public BookData FindBookByIsbn(string isbn)
        {
            if (string.IsNullOrEmpty(isbn))
            {
                throw new ArgumentNullException("isbn");
            }
            BookData book = new BookData();
            var getUri = searchBaseUrl + "isbn:" + isbn;

            using (var webClient = new GZipWebClient())
            {
                var books = JsonConvert.DeserializeObject<BooksData>(webClient.DownloadString(getUri));
                if (books.TotalItems == 1)
                {
                    book = books.Items[0];
                }
            }

            return book;
        }

        public BookData FindBookById(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException("id");
            }
            BookData book = new BookData();
            var getUri = baseUrl + @"/" + id;

            using (var webClient = new GZipWebClient())
            {
                book = JsonConvert.DeserializeObject<BookData>(webClient.DownloadString(getUri));
            }

            return book;
        }

    }
}
