using Readgress.Data.Contracts;
using Readgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Readgress.Web.Controllers
{
    public class ReadersController : ApiBaseController
    {
        public ReadersController(IReadgressUow uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            Uow = uow;
        }

        // GET api/readers
        public IEnumerable<Reader> Get()
        {
            return Uow.Readers.GetAll()
                .OrderBy(r => r.FirstName);
        }

        // GET api/readers/5
        public Reader Get(int id)
        {
            var reader = Uow.Readers.GetById(id);
            if (reader == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return reader; 
        }

        //Update an existing reader
        // PUT api/readers/5
        public HttpResponseMessage Put(Reader reader)
        {
            Uow.Readers.Update(reader);
            Uow.Commit();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
