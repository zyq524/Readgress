using GoogleBooksAPI;
using System;
using System.Collections.Generic;
using System.Net;
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

        // GET api/book/?Title="working effectively with legacy code"&startIndex=0
        [ActionName("getbyTitle")]
        public List<BookData> GetByTitle(string title, int startIndex = 0)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var books = this.details.FindBooksByTitle(title, startIndex);

            if (books.TotalItems == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return books.Items;
        }

        //// GET api/book/?Title="book title"&author="author name"
        //[ActionName("getbyTitle")]
        //public List<BookData> GetByTitleAndAuthor(string title, string author)
        //{
        //    if (string.IsNullOrEmpty(title))
        //    {
        //        throw new HttpResponseException(HttpStatusCode.BadRequest);
        //    }

        //    var books = this.details.FindBooksByTitle(title);

        //    if (books.TotalItems == 0)
        //    {
        //        throw new HttpResponseException(HttpStatusCode.NotFound);
        //    }

        //    return books.Items;
        //}
    }
}
