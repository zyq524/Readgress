using Readgress.Data.Contracts;
using Readgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Readgress.PresentaionModel.Controllers
{
    public class ProgressesController : ApiBaseController
    {
        public ProgressesController(IReadgressUow uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            Uow = uow;
        }

        // GET api/Progresses
        public IEnumerable<Progress> Get()
        {
            return Uow.Progresses.GetAll()
                .OrderBy(p=>p.Reader.FirstName);
        }

        // GET api/Progresses/5
        public Progress Get(int id)
        {
            var progress = Uow.Progresses.GetById(id);
            if (progress == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return progress; 
        }

        //Update an existing progress
        // PUT api/Progresses/5
        public HttpResponseMessage Put(Progress progress)
        {
            Uow.Progresses.Update(progress);
            Uow.Commit();
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}
