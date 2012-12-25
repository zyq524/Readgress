using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OpenLibrary
{
    public class Details
    {
        private const string baseUrl = "http://www.openlibrary.org/api/";
 
        public List<BookData> FindBooksByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            List<BookData> books = null;

            // OpenLibrary is friendly with the book title whose first character is captial. 
            title = title.Replace(title[0], title.ToUpper()[0]);

            var uri = baseUrl + "things?query={\"type\":\"\\/type\\/edition\",\"title~\":\"" + title + "*\"}&prettyprint=true&text=true";
            var oLIDs = this.GetOLIDs(uri);

            if (oLIDs != null)
            {
                books = this.FindBooksByOLIDs(oLIDs);
            }
            return books;
        }

        public List<BookData> FindBooksByOLIDs(List<string> oLIDs)
        {
            if (oLIDs == null)
            {
                throw new ArgumentNullException("oLIDs");
            }

            List<BookData> books = null;

            var bibkeys = new StringBuilder();
            foreach (var oLId in oLIDs)
            {
                string strToRemove = "/books/";
                if (oLId.StartsWith(strToRemove))
                {
                    bibkeys.Append(oLId.Replace(strToRemove, ""));
                    bibkeys.Append(",");
                }
            }
            if (bibkeys.Length != 0)
            {
                var getUri = baseUrl + "books?bibkeys=OLID:" + bibkeys.ToString().TrimEnd(new char[] { ',' }) + "&format=json&jscmd=data";

                using (var webClient = new WebClient())
                {
                    var response = JsonConvert.DeserializeObject<Dictionary<string, BookData>>(webClient.DownloadString(getUri));
                    if (response != null && response.Values.Count() > 0)
                    {
                        books = new List<BookData>();
                        foreach (var value in response.Values)
                        {
                            books.Add(value);
                        }
                    }
                }
            }
            return books;
        }

        public List<string> GetOLIDs(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(requestUri);
            }

            List<string> oLIDs = null;
            using (var webClient = new WebClient())
            {
                var response = JsonConvert.DeserializeObject<Thing>(webClient.DownloadString(requestUri));
                if (response.Status.Equals("ok", StringComparison.OrdinalIgnoreCase))
                {
                    oLIDs = response.Result;
                }
            }
            return oLIDs;
        }

        public async Task<List<BookData>> FindBooksByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            List<BookData> books = null;

            // OpenLibrary is friendly with the book title whose first character is captial. 
            title = title.Replace(title[0], title.ToUpper()[0]);

            var uri = baseUrl + "things?query={\"type\":\"\\/type\\/edition\",\"title~\":\"" + title + "*\"}&prettyprint=true&text=true";
            var oLIDs = await this.GetOLIDsAsync(uri);

            if (oLIDs != null)
            {
                books = await this.FindBooksByOLIDsAsync(oLIDs);
            }
            return books;
        }

        public async Task<List<BookData>> FindBooksByOLIDsAsync(List<string> oLIDs)
        {
            if (oLIDs == null)
            {
                throw new ArgumentNullException("oLIDs");
            }

            List<BookData> books = null;

            var bibkeys = new StringBuilder();
            foreach (var oLId in oLIDs)
            {
                string strToRemove = "/books/";
                if (oLId.StartsWith(strToRemove))
                {
                    bibkeys.Append(oLId.Replace(strToRemove, ""));
                    bibkeys.Append(",");
                }
            }
            if (bibkeys.Length != 0)
            {
                var getUri = baseUrl + "books?bibkeys=OLID:" + bibkeys.ToString().TrimEnd(new char[] { ',' }) + "&format=json&jscmd=data";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(getUri);
                    var bookDetails = await response.Content.ReadAsAsync<Dictionary<string, BookData>>();
                    if (bookDetails != null && bookDetails.Values.Count() > 0)
                    {
                        books = new List<BookData>();
                        foreach (var value in bookDetails.Values)
                        {
                            books.Add(value);
                        }
                    }
                }
            }
            return books;
        }

        public async Task<List<string>> GetOLIDsAsync(string requestUri)
        {
            if (string.IsNullOrEmpty(requestUri))
            {
                throw new ArgumentNullException(requestUri);
            }
            List<string> oLIDs = null;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(requestUri);
                var thing = await response.Content.ReadAsAsync<Thing>();
                if (thing.Status.Equals("ok", StringComparison.OrdinalIgnoreCase))
                {
                    oLIDs = thing.Result;
                }
            }
            return oLIDs;
        }
    }

    class Thing
    {
        public string Status { get; set; }
        public List<string> Result { get; set; }
    }
}
