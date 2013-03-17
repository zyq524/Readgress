using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Readgress.Data.Contracts;
using Readgress.Models;
using Readgress.PresentationModel.Controllers;
using Readgress.PresentationModel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace Readgress.PresentationModel.UnitTests
{
    [TestClass]
    public class BookmarkControllerUnitTests
    {
        private Mock<IPrincipal> user;
        private Mock<IIdentity> identity;
        private Mock<IReadgressUow> uowMock;
        private Mock<IBookmarkRepository> bookmarkRepositoryMock;

        private BookmarkController sut;
        private List<Bookmark> bookmarksTest;
        private Progress progressTest;
        private Reader readerTest;

        [TestInitialize]
        public void Setup()
        {
            user = new Mock<IPrincipal>();
            identity = new Mock<IIdentity>();
            user.Setup(x => x.Identity).Returns(identity.Object);
            identity.Setup(x => x.Name).Returns("Tom");
            Thread.CurrentPrincipal = user.Object;

            uowMock = new Mock<IReadgressUow>();
            bookmarkRepositoryMock = new Mock<IBookmarkRepository>();

            HttpConfiguration config = new HttpConfiguration();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/bookmark");
            IHttpRoute route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            HttpRouteData routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "bookmark" } });

            sut = new BookmarkController(uowMock.Object);
            sut.ControllerContext = new HttpControllerContext(config, routeData, request);
            sut.Request = request;
            sut.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            sut.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);

            readerTest = new Reader
            {
                Id = 1,
                FirstName = "Tom",
                LastName = "Jerry",
                Email = "tom@jerry.com",
                Gender = "Male",
                UserName = "Tom",
                Link = "www.jerry.com",
                CreatedOn = DateTime.Now
            };

           progressTest = new Progress()
           {
               Id = 1,
               Isbn = "OL12345",
               ReaderId = 1,
               Reader = readerTest,
               IsFinished = false
           };

            bookmarksTest = new List<Bookmark>()
            {
                new Bookmark()
                {
                    Id=1, ProgressId=1, Progress=progressTest, PageNumber=20, CreatedOn=DateTime.Now
                },
                new Bookmark()
                {
                    Id=2, ProgressId=1, Progress=progressTest, PageNumber=30, CreatedOn=DateTime.Now
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyDetails_ThrowException()
        {
            sut = new BookmarkController(null);
        }

        #region GET
        [TestMethod]
        public void Get_WithId_ReturnsBookmark()
        {
            uowMock.Setup(u => u.Bookmarks.GetById(1)).Returns(bookmarksTest[0]);

            BookmarkDto actual = sut.Get(1);

            Assert.IsNotNull(actual);
            Assert.AreEqual(bookmarksTest[0].Id, actual.Id);
            Assert.AreEqual(bookmarksTest[0].Progress.ReaderId, actual.ReaderId);
            Assert.AreEqual(bookmarksTest[0].Progress.Reader.UserName, actual.UserName);
            Assert.AreEqual(bookmarksTest[0].ProgressId, actual.ProgressId);
            Assert.AreEqual(bookmarksTest[0].PageNumber, actual.PageNumber);
            Assert.AreEqual(bookmarksTest[0].CreatedOn, actual.CreatedOn);
        }


        [TestMethod]
        public void Get_WithNoExistingBookmarkId_ThrowNotFound()
        {
            uowMock.Setup(u => u.Bookmarks.GetById(3)).Returns((Bookmark)null);

            HttpResponseException exception = null;
            try
            {
                BookmarkDto actual = sut.Get(3);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.NotFound, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Get_WithNotAuthorizedBookmarkId_ThrowUnauthorized()
        {
            identity.Setup(x => x.Name).Returns("Mary");
            uowMock.Setup(u => u.Bookmarks.GetById(1)).Returns(bookmarksTest[0]);

            HttpResponseException exception = null;
            try
            {
                BookmarkDto actual = sut.Get(1);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.Unauthorized, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Get_WithProgressId_ReturnsThatProgressBookmarks()
        {
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressTest);
            uowMock.Setup(u => u.Bookmarks.GetByProgressId(1)).Returns(bookmarksTest.AsQueryable());

            List<BookmarkDto> actual = sut.GetByProgressId(1).ToList();

            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count());

            Assert.AreEqual(bookmarksTest[0].Id, actual[0].Id);
            Assert.AreEqual(bookmarksTest[0].Progress.ReaderId, actual[0].ReaderId);
            Assert.AreEqual(bookmarksTest[0].Progress.Reader.UserName, actual[0].UserName);
            Assert.AreEqual(bookmarksTest[0].ProgressId, actual[0].ProgressId);
            Assert.AreEqual(bookmarksTest[0].PageNumber, actual[0].PageNumber);
            Assert.AreEqual(bookmarksTest[0].CreatedOn, actual[0].CreatedOn);

            Assert.AreEqual(bookmarksTest[1].Id, actual[1].Id);
            Assert.AreEqual(bookmarksTest[1].Progress.ReaderId, actual[1].ReaderId);
            Assert.AreEqual(bookmarksTest[1].Progress.Reader.UserName, actual[1].UserName);
            Assert.AreEqual(bookmarksTest[1].ProgressId, actual[1].ProgressId);
            Assert.AreEqual(bookmarksTest[1].PageNumber, actual[1].PageNumber);
            Assert.AreEqual(bookmarksTest[1].CreatedOn, actual[1].CreatedOn);
        }

        [TestMethod]
        public void Get_WithNoExistingProgressId_ThrowNotFound()
        {
            uowMock.Setup(u => u.Progresses.GetById(2)).Returns((Progress)null);

            HttpResponseException exception = null;
            try
            {
                IEnumerable<BookmarkDto> actual = sut.GetByProgressId(2);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.NotFound, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Get_WithNotAuthorizedProgressId_ThrowUnauthorized()
        {
            identity.Setup(x => x.Name).Returns("Mary");
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressTest);

            HttpResponseException exception = null;
            try
            {
                IEnumerable<BookmarkDto> actual = sut.GetByProgressId(1);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.Unauthorized, exception.Response.StatusCode);
        }
        #endregion

        #region POST

        [TestMethod]
        public void Post_WithValidValues_CallsAddAndCommit()
        {
            BookmarkDto dto = new BookmarkDto(bookmarksTest[0]);
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressTest);
            uowMock.Setup(u => u.Bookmarks).Returns(bookmarkRepositoryMock.Object);

            HttpResponseMessage actual = sut.Post(dto);

            Assert.AreEqual(HttpStatusCode.Created, actual.StatusCode);

            bookmarkRepositoryMock.Verify(b => b.Add(It.IsAny<Bookmark>()), Times.Once());
            uowMock.Verify(u => u.Commit(), Times.Once());

            Assert.AreEqual("http://localhost/api/bookmark/1", actual.Headers.Location.ToString());
        }

        [TestMethod]
        public void Post_ToAFinishedProgress_ThrowsNotAllowed()
        {
            BookmarkDto dto = new BookmarkDto(bookmarksTest[0]);
            progressTest.IsFinished = true;

            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressTest);

            HttpResponseException exception = null;
            try
            {
                HttpResponseMessage actual = sut.Post(dto);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.MethodNotAllowed, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Post_WithEmptyBookmarkDto_ThrowBadRequest()
        {
            HttpResponseException exception = null;
            try
            {
                sut.Post(null);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Post_WithInvalidBookmarkDto_ThrowBadRequest()
        {
            HttpResponseException exception = null;
            try
            {
                BookmarkDto dto = new BookmarkDto(bookmarksTest[0]);
                dto.UserName = null;

                sut.ModelState.AddModelError("UserName", "Can't find the value");
                sut.Post(dto);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Post_WithNotAuthorizedProgressId_ThrowUnauthorized()
        {
            identity.Setup(x => x.Name).Returns("Mary");
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressTest);

            HttpResponseException exception = null;
            try
            {
                sut.Post(new BookmarkDto(bookmarksTest[0]));
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.Unauthorized, exception.Response.StatusCode);
        }
        #endregion

        #region PUT
        [TestMethod]
        public void Put_WithValidValues_CallsUpdateAndCommit()
        {
            BookmarkDto dto = new BookmarkDto(bookmarksTest[0]);

            uowMock.Setup(u => u.Bookmarks).Returns(bookmarkRepositoryMock.Object);

            HttpResponseMessage actual = sut.Put(1, dto);

            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);

            bookmarkRepositoryMock.Verify(b => b.Update(It.IsAny<Bookmark>()), Times.Once());
            uowMock.Verify(u => u.Commit(), Times.Once());
        }

        [TestMethod]
        public void Put_WithEmptyProgressDto_ThrowBadRequest()
        {
            HttpResponseException exception = null;
            try
            {
                sut.Put(1, null);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Put_WithDifferentIds_ThrowBadRequest()
        {
            HttpResponseException exception = null;
            try
            {
                sut.Put(1, new BookmarkDto(bookmarksTest[1]));
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Put_WithInvalidProgressDto_ThrowBadRequest()
        {
            HttpResponseException exception = null;
            try
            {
                BookmarkDto dto = new BookmarkDto(bookmarksTest[0]);
                dto.UserName = null;

                sut.ModelState.AddModelError("UserName", "Can't find the value");
                sut.Put(1, dto);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Put_WithNotAuthorizedProgressId_ThrowUnauthorized()
        {
            identity.Setup(x => x.Name).Returns("Mary");
            uowMock.Setup(u => u.Bookmarks.GetById(1)).Returns(bookmarksTest[0]);

            HttpResponseException exception = null;
            try
            {
                sut.Put(1, new BookmarkDto(bookmarksTest[0]));
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.Unauthorized, exception.Response.StatusCode);
        }
        #endregion

        #region DELETE

        [TestMethod]
        public void Delete_WithValidId_CallsDeleteAndCommit()
        {
            uowMock.Setup(u => u.Bookmarks).Returns(bookmarkRepositoryMock.Object);
            bookmarkRepositoryMock.Setup(b => b.GetById(1)).Returns(bookmarksTest[0]);
            
            HttpResponseMessage actual = sut.Delete(1);

            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);

            bookmarkRepositoryMock.Verify(b => b.Delete(It.IsAny<Bookmark>()), Times.Once());
            uowMock.Verify(u => u.Commit(), Times.Once());
        }

        [TestMethod]
        public void Delete_WithNotExistingBookmarkId_ThrowNotFound()
        {
            uowMock.Setup(u => u.Bookmarks.GetById(3)).Returns((Bookmark)null);

            HttpResponseException exception = null;
            try
            {
                sut.Delete(3);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.NotFound, exception.Response.StatusCode);
        }

        [TestMethod]
        public void Delete_WithNotAuthorizedBookmarkId_ThrowUnauthorized()
        {
            identity.Setup(x => x.Name).Returns("Mary");
            uowMock.Setup(u => u.Bookmarks.GetById(1)).Returns(bookmarksTest[0]);

            HttpResponseException exception = null;
            try
            {
                sut.Delete(1);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.Unauthorized, exception.Response.StatusCode);
        }
        #endregion
    }
}
