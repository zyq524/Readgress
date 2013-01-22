using OpenLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Readgress.PresentationModel.Controllers
{
    public class BookController : ApiController
    {
        private IDetails details;

        public BookController(IDetails details)
        {
            if (details == null)
            {
                throw new ArgumentNullException("details");
            }
            this.details = details;
        }

        // GET api/book/?OLId=OL3315089M
        [ActionName("getbyOLId")]
        public BookData GetByOLId(string oLId)
        {
            if (string.IsNullOrEmpty(oLId))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest); 
            }
            var book = this.details.FindBooksByOLIDs(new List<string>() { oLId }).FirstOrDefault();
            if (book == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return book;
        }

        // GET api/book/?Title="working effectively with legacy code"
        [ActionName("getbyTitle")]
        public List<BookData> GetByTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var books = this.details.FindBooksByTitle(title);

            if (books.Count == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return books;
        }
    }
}
