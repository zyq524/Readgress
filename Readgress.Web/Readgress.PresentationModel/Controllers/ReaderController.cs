using Readgress.Data.Contracts;
using Readgress.Models;
using Readgress.PresentationModel.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Readgress.PresentationModel.Controllers
{
    [Authorize]
    public class ReaderController : ApiBaseController
    {
        public ReaderController(IReadgressUow uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            Uow = uow;
        }

        // GET api/reader
        public ReaderDto Get()
        {
            Reader reader = Uow.Readers.GetAll().Where(r => r.UserName == User.Identity.Name).FirstOrDefault();
            if (reader == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
            return new ReaderDto(reader);
        }
    }
}
