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
    public class ProgressControllerUnitTests
    {
        private Mock<IPrincipal> user;
        private Mock<IIdentity> identity;
        private Mock<IReadgressUow> uowMock;
        private Mock<IProgressRepository> progressRepositoryMock;

        private ProgressController sut;
        private List<Progress> progressesTest;
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
            progressRepositoryMock = new Mock<IProgressRepository>();

            HttpConfiguration config = new HttpConfiguration();
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/progress");
            IHttpRoute route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}");
            HttpRouteData routeData = new HttpRouteData(route, new HttpRouteValueDictionary { { "controller", "progress" } });

            sut = new ProgressController(uowMock.Object);
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

            progressesTest = new List<Progress>()
            {
                new Progress()
                {
                    Id=1, OLId="OL12345", ReaderId=1, Reader=readerTest, IsFinished=false
                },
                new Progress()
                {
                    Id=2, OLId="OL56789", ReaderId=1, Reader=readerTest, IsFinished=false
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyDetails_ThrowException()
        {
            sut = new ProgressController(null);
        }

        #region GET
        [TestMethod]
        public void Get_WithEmpty_ReturnsCurrentReaderAllProgresses()
        {
            uowMock.Setup(u => u.Progresses.GetAll()).Returns(progressesTest.AsQueryable());

            List<ProgressDto> actual = sut.Get().ToList();

            Assert.AreEqual(2, actual.Count());

            Assert.AreEqual(readerTest.UserName, actual[1].UserName);
            Assert.AreEqual(progressesTest[0].Id, actual[1].Id);
            Assert.AreEqual(progressesTest[0].OLId, actual[1].OLId);
            Assert.AreEqual(progressesTest[0].ReaderId, actual[1].ReaderId);
            Assert.AreEqual(progressesTest[0].IsFinished, actual[1].IsFinished);

            Assert.AreEqual(readerTest.UserName, actual[0].UserName);
            Assert.AreEqual(progressesTest[1].Id, actual[0].Id);
            Assert.AreEqual(progressesTest[1].OLId, actual[0].OLId);
            Assert.AreEqual(progressesTest[1].ReaderId, actual[0].ReaderId);
            Assert.AreEqual(progressesTest[1].IsFinished, actual[0].IsFinished);
        }

        [TestMethod]
        public void Get_WithEmptyButReaderHasNoProgress_ReturnsEmpty()
        {
            uowMock.Setup(u => u.Progresses.GetAll()).Returns(new List<Progress>().AsQueryable());

            List<ProgressDto> actual = sut.Get().ToList();

            Assert.AreEqual(0, actual.Count());
        }

        [TestMethod]
        public void Get_WithProgressId_ReturnsCurrentReaderProgressWithSameId()
        {
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressesTest[0]);

            ProgressDto actual = sut.Get(1);

            Assert.IsNotNull(actual);

            Assert.AreEqual(readerTest.UserName, actual.UserName);
            Assert.AreEqual(progressesTest[0].Id, actual.Id);
            Assert.AreEqual(progressesTest[0].OLId, actual.OLId);
            Assert.AreEqual(progressesTest[0].ReaderId, actual.ReaderId);
            Assert.AreEqual(progressesTest[0].IsFinished, actual.IsFinished);
        }

        [TestMethod]
        public void Get_WithNoExistingProgressId_ThrowNotFound()
        {
            uowMock.Setup(u => u.Progresses.GetById(3)).Returns((Progress)null);

            HttpResponseException exception = null;
            try
            {
                ProgressDto actual = sut.Get(3);
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
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressesTest[0]);

            HttpResponseException exception = null;
            try
            {
                ProgressDto actual = sut.Get(1);
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
            ProgressDto dto = new ProgressDto(progressesTest[0]);
            uowMock.Setup(u => u.Progresses).Returns(progressRepositoryMock.Object);

            HttpResponseMessage actual = sut.Post(dto);

            Assert.AreEqual(HttpStatusCode.Created, actual.StatusCode);

            progressRepositoryMock.Verify(p => p.Add(It.IsAny<Progress>()), Times.Once());
            uowMock.Verify(u => u.Commit(), Times.Once());

            Assert.AreEqual("http://localhost/api/progress/1", actual.Headers.Location.ToString());
        }

        [TestMethod]
        public void Post_WithEmptyProgressDto_ThrowBadRequest()
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
        public void Post_WithInvalidProgressDto_ThrowBadRequest()
        {
            HttpResponseException exception = null;
            try
            {
                ProgressDto dto = new ProgressDto(progressesTest[0]);
                dto.OLId = null;

                sut.ModelState.AddModelError("OLId", "Can't find the value");
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
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressesTest[0]);

            HttpResponseException exception = null;
            try
            {
                sut.Post(new ProgressDto(progressesTest[0]));
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
            ProgressDto dto = new ProgressDto(progressesTest[0]);
        
            uowMock.Setup(u => u.Progresses).Returns(progressRepositoryMock.Object);

            HttpResponseMessage actual = sut.Put(1, dto);

            Assert.AreEqual(HttpStatusCode.OK, actual.StatusCode);

            progressRepositoryMock.Verify(p => p.Update(It.IsAny<Progress>()), Times.Once());
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
                sut.Put(1, new ProgressDto(progressesTest[1]));
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
                ProgressDto dto = new ProgressDto(progressesTest[0]);
                dto.OLId = null;

                sut.ModelState.AddModelError("OLId", "Can't find the value");
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
            uowMock.Setup(u => u.Progresses.GetById(1)).Returns(progressesTest[0]);

            HttpResponseException exception = null;
            try
            {
                sut.Put(1, new ProgressDto(progressesTest[0]));
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
