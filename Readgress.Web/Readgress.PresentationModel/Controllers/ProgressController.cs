using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Readgress.Data.Contracts;
using Readgress.Models;
using Readgress.PresentationModel.Filters;
using Readgress.PresentationModel.Models;

namespace Readgress.PresentationModel.Controllers
{
    [FBAuthorize]
    public class ProgressController : ApiBaseController
    {
        public ProgressController(IReadgressUow uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            Uow = uow;
        }
        
        // GET api/Progress
        public IEnumerable<ProgressDto> Get()
        {
            return Uow.Progresses.GetAll().Where(p => p.Reader.UserName == User.Identity.Name)
                .OrderByDescending(p => p.Id).AsEnumerable()
                .Select(p => new ProgressDto(p));
        }

        // GET api/Progress/5
        public ProgressDto Get(int id)
        {
            var progress = Uow.Progresses.GetById(id);
            if (progress == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            if (progress.Reader.UserName != User.Identity.Name)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            return new ProgressDto(progress); 
        }

        //POST api/progress
        public HttpResponseMessage Post([FromBody]ProgressDto progressDto)
        {
            if (progressDto != null && ModelState.IsValid)
            {
                if (progressDto.UserName != User.Identity.Name)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
                else
                {
                    Progress progress = progressDto.ToEntity();
                    progress.ReaderId = Uow.Readers.GetAll().Where(r => r.UserName == User.Identity.Name).First().Id;
                    Uow.Progresses.Add(progress);
                    Uow.Commit();

                    progressDto.Id = progress.Id;
                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, progressDto);

                    string uri = Url.Link("DefaultApi", new { id = progress.Id });
                    response.Headers.Location = new Uri(uri);
                    return response;
                }
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }

        //Update an existing progress
        // PUT api/Progress/5
        public HttpResponseMessage Put(int id, [FromBody]ProgressDto progressDto)
        {
            if (progressDto!=null && ModelState.IsValid && id==progressDto.Id)
            {
                if (progressDto.UserName != User.Identity.Name)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
                Progress progress = progressDto.ToEntity();
                progress.ReaderId = Uow.Readers.GetAll().Where(r => r.UserName == User.Identity.Name).First().Id;
                Uow.Progresses.Update(progress);
                Uow.Commit();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        }
    }
}
