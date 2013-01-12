using OpenLibrary;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Readgress.PresentaionModel.Controllers
{
    public class BooksController : ApiController
    {
        private Details details = new Details();

        // GET api/books/?OLId=OL3315089M
        [ActionName("getbyOLId")]
        public BookData GetByOLId(string oLId)
        {
            return this.details.FindBooksByOLIDs(new List<string>() { oLId }).FirstOrDefault();
        }

        // GET api/books/?Title="working effectively with legacy code"
        [ActionName("getbyTitle")]
        public List<BookData> GetByTitle(string title)
        {
            return this.details.FindBooksByTitle(title);
        }
    }
}
