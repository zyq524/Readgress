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
using System.Security.Principal;
using System.Threading;
using System.Web.Http;

namespace Readgress.PresentationModel.UnitTests
{
    [TestClass]
    public class ReaderControllerUnitTests
    {
        private Mock<IPrincipal> user;
        private Mock<IIdentity> identity;
        private Mock<IReadgressUow> uowMock;
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
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyDetails_ThrowException()
        {
            ReaderController sut = new ReaderController(null);
        }

        [TestMethod]
        public void Get_WithEmpty_ReturnsCurrentReaderDto()
        {
            uowMock.Setup(u => u.Readers.GetAll()).Returns(new List<Reader>() { readerTest }.AsQueryable());

            ReaderController sut = new ReaderController(uowMock.Object);
            ReaderDto actual = sut.Get();

            Assert.AreEqual(readerTest.Id, actual.Id);
            Assert.AreEqual(readerTest.UserName, actual.UserName);
        }

        [TestMethod]
        public void Get_NotFoundReader_ThrowBadRequest()
        {
            uowMock.Setup(u => u.Readers.GetAll()).Returns(new List<Reader>().AsQueryable());
            HttpResponseException exception = null;
            try
            {
                ReaderController sut = new ReaderController(uowMock.Object);
                ReaderDto actual = sut.Get();
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }
        //[TestMethod]
        //public void Get_WithUnauthenticated_ThrowUnAuthroized()
        //{
        //    Thread.CurrentPrincipal = null;
        //    HttpResponseException exception = null;
        //    try
        //    {
        //        ReaderController sut = new ReaderController(uowMock.Object);
        //        ReaderDto actual = sut.Get();
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        exception = ex;
        //    }
        //    Assert.IsNotNull(exception);
        //    Assert.AreEqual(HttpStatusCode.Unauthorized, exception.Response.StatusCode);
        //}
    }
}
