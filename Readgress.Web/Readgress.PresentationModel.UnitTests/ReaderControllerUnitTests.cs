using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Readgress.Data.Contracts;
using Readgress.PresentationModel.Controllers;
using Readgress.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Net;

namespace Readgress.PresentationModel.UnitTests
{
    [TestClass]
    public class ReaderControllerUnitTests
    {
        private Mock<IReadgressUow> uowMock;
        private IList<Reader> readersTest;

        [TestInitialize]
        public void Setup()
        {
            uowMock = new Mock<IReadgressUow>();
            readersTest = new List<Reader>()
            { 
                new Reader{Id = 1, FirstName = "test1"},
                new Reader{Id = 2, FirstName = "test2"}
            };
        }

        //[TestMethod]
        //[ExpectedException(typeof(ArgumentNullException))]
        //public void Constructor_WithEmptyDetails_ThrowException()
        //{
        //    var sut = new ReaderController(null);
        //}

        //[TestMethod]
        //public void Get_WithEmpty_ReturnsCurrentReader()
        //{
        //    uowMock.Setup(u => u.Readers.GetAll()).Returns(readersTest.AsQueryable());

        //    var sut = new ReaderController(uowMock.Object);
        //    var actual = sut.Get().ToList();

        //    Assert.AreEqual(2, actual.Count());
        //    Assert.AreEqual(readersTest[0], actual[0]);
        //    Assert.AreEqual(readersTest[1], actual[1]);
        //}

        //[TestMethod]
        //public void Get_WithReaderId_ReturnsReader()
        //{
        //    uowMock.Setup(u => u.Readers.GetById(1)).Returns(readersTest[0]);

        //    var sut = new ReaderController(uowMock.Object);
        //    var actual = sut.Get(1);

        //    Assert.AreSame(readersTest[0], actual);
        //}

        //[TestMethod]
        //public void Get_WithNonExistingReaderId_ThrowNotFound()
        //{
        //    uowMock.Setup(u => u.Readers.GetById(1)).Returns((Reader)null);
        //    var sut = new ReaderController(uowMock.Object);
        //    HttpResponseException exception = null;
        //    try
        //    {
        //        var actual = sut.Get(1);
        //    }
        //    catch (HttpResponseException ex)
        //    {
        //        exception = ex;
        //    }
        //    Assert.IsNotNull(exception);
        //    Assert.AreEqual(HttpStatusCode.NotFound, exception.Response.StatusCode);

        //}
    }
}
