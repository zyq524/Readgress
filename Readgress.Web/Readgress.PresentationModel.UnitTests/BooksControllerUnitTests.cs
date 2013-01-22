using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OpenLibrary;
using Readgress.PresentationModel.Controllers;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http;

namespace Readgress.PresentationModel.UnitTests
{
    [TestClass]
    public class BooksControllerUnitTests
    {
        private Mock<IDetails> detailsMock;
        private string oLIdTest;
        private string titleTest;

        [TestInitialize]
        public void Setup()
        {
            detailsMock = new Mock<IDetails>();
            oLIdTest = "OL3315089M";
            titleTest = "working effectively with legacy code";
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyDetails_ThrowException()
        {
            var sut = new BookController(null);
        }

        [TestMethod]
        public void GetByOLId_WithValidOLId_ReturnsOneBook()
        {
            var expected = new BookData() { Title = titleTest };
            detailsMock.Setup(d => d.FindBooksByOLIDs(new List<string>(){oLIdTest})).Returns(new List<BookData>(){expected});

            var sut = new BookController(detailsMock.Object);
            var actual = sut.GetByOLId(oLIdTest);

            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void GetByOLId_WithEmptyOLId_ThrowBadRequest()
        {
            var sut = new BookController(detailsMock.Object);
            HttpResponseException exception = null;

            try
            {
                var actual = sut.GetByOLId(null);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void GetByOLId_WithNonExistingOLId_ThrowNotFound()
        {
            var sut = new BookController(detailsMock.Object);
            detailsMock.Setup(d => d.FindBooksByOLIDs(new List<string>() { oLIdTest })).Returns(new List<BookData>());
            HttpResponseException exception = null;

            try
            {
                var actual = sut.GetByOLId(oLIdTest);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.NotFound, exception.Response.StatusCode);
        }

        [TestMethod]
        public void GetByTitle_WithValidTitle_ReturnsOneBook()
        {
            var expected = new List<BookData>() { new BookData() { Title = titleTest } };
            detailsMock.Setup(d => d.FindBooksByTitle(titleTest)).Returns(expected);

            var sut = new BookController(detailsMock.Object);
            var actual = sut.GetByTitle(titleTest);

            Assert.AreEqual(1, actual.Count);
            Assert.AreSame(expected, actual);
        }

        [TestMethod]
        public void GetByTitle_WithValidTitle_ReturnsTwoBooks()
        {
            var expected = new List<BookData>()
                            {
                                new BookData() { Title = titleTest },
                                new BookData(){ Title = titleTest + " new" }
                            };
            detailsMock.Setup(d => d.FindBooksByTitle(titleTest)).Returns(expected);

            var sut = new BookController(detailsMock.Object);
            var actual = sut.GetByTitle(titleTest);

            Assert.AreEqual(2, actual.Count);
            Assert.AreSame(expected, actual);
        }


        [TestMethod]
        public void GetByTitle_WithEmptyTitle_ThrowBadRequest()
        {
            var sut = new BookController(detailsMock.Object);
            HttpResponseException exception = null;

            try
            {
                var actual = sut.GetByTitle(null);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.BadRequest, exception.Response.StatusCode);
        }

        [TestMethod]
        public void GetByTitle_WithNonExistingTitle_ThrowNotFound()
        {
            var sut = new BookController(detailsMock.Object);
            detailsMock.Setup(d => d.FindBooksByTitle(titleTest)).Returns(new List<BookData>());
            HttpResponseException exception = null;

            try
            {
                var actual = sut.GetByTitle(titleTest);
            }
            catch (HttpResponseException ex)
            {
                exception = ex;
            }
            Assert.IsNotNull(exception);
            Assert.AreEqual(HttpStatusCode.NotFound, exception.Response.StatusCode);
        }
    }
}
