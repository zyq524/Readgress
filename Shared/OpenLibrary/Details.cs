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
    public class Details : IDetails
    {
        private const string baseUrl = "http://www.openlibrary.org/api/";
 
        public List<BookData> FindBooksByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }
            List<BookData> books = new List<BookData>();

            var oLIDs = this.FindOLIDsByTitle(title);

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

            List<BookData> books = new List<BookData>();

            var bibkeys = new StringBuilder();
            foreach (var oLID in oLIDs)
            {
                bibkeys.Append(oLID);
                bibkeys.Append(",");
            }
            if (bibkeys.Length != 0)
            {
                var getUri = baseUrl + "books?bibkeys=OLID:" + bibkeys.ToString().TrimEnd(new char[] { ',' }) + "&format=json&jscmd=data";

                using (var webClient = new WebClient())
                {
                    var response = JsonConvert.DeserializeObject<Dictionary<string, BookData>>(webClient.DownloadString(getUri));
                    if (response.Count() > 0)
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

        public List<string> FindOLIDsByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            // OpenLibrary is friendly with the book title whose first character is captial. 
            title = title[0].ToString().ToUpper() + title.Substring(1);

            var uri = baseUrl + "things?query={\"type\":\"\\/type\\/edition\",\"title~\":\"" + title + "*\"}&prettyprint=true&text=true";

            List<string> oLIDs = new List<string>();
            using (var webClient = new WebClient())
            {
                var response = JsonConvert.DeserializeObject<Thing>(webClient.DownloadString(uri));
                if (response.Status.Equals("ok", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var oLID in response.Result)
                    {
                        string strToRemove = "/books/";
                        if (oLID.StartsWith(strToRemove))
                        {
                            oLIDs.Add(oLID.Replace(strToRemove, ""));
                        }
                    }
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
            List<BookData> books = new List<BookData>();

            var oLIDs = await this.FindOLIDsByTitleAsync(title);

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

            List<BookData> books = new List<BookData>();

            var bibkeys = new StringBuilder();
            foreach (var oLID in oLIDs)
            {
                bibkeys.Append(oLID);
                bibkeys.Append(",");
            }
            if (bibkeys.Length != 0)
            {
                var getUri = baseUrl + "books?bibkeys=OLID:" + bibkeys.ToString().TrimEnd(new char[] { ',' }) + "&format=json&jscmd=data";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(getUri);
                    var bookDetails = JsonConvert.DeserializeObject<Dictionary<string, BookData>>(await response.Content.ReadAsStringAsync());
                    if (bookDetails.Count() > 0)
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

        public async Task<List<string>> FindOLIDsByTitleAsync(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException("title");
            }

            // OpenLibrary is friendly with the book title whose first character is captial. 
            title = title.Replace(title[0], title.ToUpper()[0]);
            var uri = baseUrl + "things?query={\"type\":\"\\/type\\/edition\",\"title~\":\"" + title + "*\"}&prettyprint=true&text=true";
            
            List<string> oLIDs = new List<string>();
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(uri);
                var thing = JsonConvert.DeserializeObject<Thing>(await response.Content.ReadAsStringAsync());
                if (thing.Status.Equals("ok", StringComparison.OrdinalIgnoreCase))
                {
                    foreach (var oLID in thing.Result)
                    {
                        string strToRemove = "/books/";
                        if (oLID.StartsWith(strToRemove))
                        {
                            oLIDs.Add(oLID.Replace(strToRemove, ""));
                        }
                    }
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
