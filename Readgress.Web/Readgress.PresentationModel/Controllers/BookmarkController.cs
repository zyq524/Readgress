﻿using Readgress.Data.Contracts;
using Readgress.Models;
using Readgress.PresentationModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Readgress.PresentationModel.Controllers

{
    [Authorize]
    public class BookmarkController : ApiBaseController
    {
        public BookmarkController(IReadgressUow uow)
        {
            if (uow == null)
            {
                throw new ArgumentNullException("uow");
            }
            Uow = uow;
        }

        // GET api/Bookmark/5
        public BookmarkDto Get(int id)
        {
            Bookmark bookmark = Uow.Bookmarks.GetById(id);
            if (bookmark == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            if (bookmark.Progress.Reader.UserName != User.Identity.Name)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            return new BookmarkDto(bookmark); 
        }

        // GET api/Bookmark/?ProgressId=1
        [ActionName("getByProgressId")]
        public IEnumerable<BookmarkDto> GetByProgressId(int progressId)
        {
            Progress progress = Uow.Progresses.GetById(progressId);

            if (progress == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            if (progress.Reader.UserName != User.Identity.Name)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }

            var bookmarks = Uow.Bookmarks.GetByProgressId(progressId)
                .AsEnumerable()
                .Select(b => new BookmarkDto(b));
            if (bookmarks.Count() == 0)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return bookmarks;
        }

        // POST api/bookmark
        public HttpResponseMessage Post([FromBody]BookmarkDto bookmarkDto)
        {
            if (bookmarkDto != null && ModelState.IsValid)
            {
                if (bookmarkDto.UserName != User.Identity.Name)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }
                else
                {
                    Progress progress = Uow.Progresses.GetById(bookmarkDto.ProgressId);
                    if (progress.IsFinished)
                    {
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.MethodNotAllowed));
                    }

                    Bookmark bookmark = bookmarkDto.ToEntity();
                    Uow.Bookmarks.Add(bookmark);
                    Uow.Commit();

                    HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, new BookmarkDto(bookmark));
                    response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = bookmark.Id }));
                    return response;
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // PUT api/Bookmark/5
        public HttpResponseMessage Put(int id, [FromBody]BookmarkDto bookmarkDto)
        {
            if (bookmarkDto != null && ModelState.IsValid && id == bookmarkDto.Id)
            {
                if (bookmarkDto.UserName != User.Identity.Name)
                {
                    throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                }

                Uow.Bookmarks.Update(bookmarkDto.ToEntity());
                Uow.Commit();

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Bookmark/5
        public HttpResponseMessage Delete(int id)
        {
            Bookmark bookmark = Uow.Bookmarks.GetById(id);
            if (bookmark == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            if (bookmark.Progress.Reader.UserName != User.Identity.Name)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            }
            Uow.Bookmarks.Delete(bookmark);
            Uow.Commit();

            return Request.CreateResponse(HttpStatusCode.OK, bookmark);
        }
    }
}