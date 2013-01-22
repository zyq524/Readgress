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
        private BookData bookTest;

        [TestInitialize]
        public void Setup()
        {
            detailsMock = new Mock<IDetails>();
            bookTest = new BookData()
            {
                Title = "WebApi Introdutcion",
                SubTitle = "For Dummy",
                Subjects = new List<Subject>() { new Subject() { Name = "IT" } },
                Authors = new List<Author>() { new Author() { Name = "Microsoft", Url = "www.microsoft.com" } },
                Url = "www.openlibray.com",
                Identifiers = new Identifier() { OpenLibrary = new List<string>() { "OL3315082M" } }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyDetails_ThrowException()
        {
            BookController sut = new BookController(null);
        }

        [TestMethod]
        public void GetByOLId_WithValidOLId_ReturnsOneBook()
        {
            detailsMock.Setup(d => d.FindBooksByOLIDs(new List<string>(){bookTest.Identifiers.OpenLibrary[0]})).Returns(new List<BookData>(){bookTest});

            BookController sut = new BookController(detailsMock.Object);
            BookData actual = sut.GetByOLId(bookTest.Identifiers.OpenLibrary[0]);

            Assert.AreSame(bookTest, actual);
        }

        [TestMethod]
        public void GetByOLId_WithEmptyOLId_ThrowBadRequest()
        {
            HttpResponseException exception = null;

            try
            {
                BookController sut = new BookController(detailsMock.Object);
                BookData actual = sut.GetByOLId(null);
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
            string nonExistingOILd = "OL12345";
            detailsMock.Setup(d => d.FindBooksByOLIDs(new List<string>() { nonExistingOILd })).Returns(new List<BookData>());
            HttpResponseException exception = null;

            try
            {
                BookController sut = new BookController(detailsMock.Object);
                BookData actual = sut.GetByOLId(nonExistingOILd);
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
            detailsMock.Setup(d => d.FindBooksByTitle(bookTest.Title)).Returns(new List<BookData>() { bookTest });

            BookController sut = new BookController(detailsMock.Object); 
            List<BookData> actual = sut.GetByTitle(bookTest.Title);

            Assert.AreEqual(1, actual.Count);
            Assert.AreSame(bookTest, actual[0]);
        }

        [TestMethod]
        public void GetByTitle_WithValidTitle_ReturnsTwoBooks()
        {
            BookData bookTest2 = bookTest;
            bookTest2.Title += " Version1";
            List<BookData> expected = new List<BookData>() { bookTest, bookTest2 };
            detailsMock.Setup(d => d.FindBooksByTitle(bookTest.Title)).Returns(expected);

            BookController sut = new BookController(detailsMock.Object);
            List<BookData> actual = sut.GetByTitle(bookTest.Title);

            Assert.AreEqual(2, actual.Count);
            Assert.AreSame(expected, actual);
        }


        [TestMethod]
        public void GetByTitle_WithEmptyTitle_ThrowBadRequest()
        {
            HttpResponseException exception = null;

            try
            {
                BookController sut = new BookController(detailsMock.Object);
                List<BookData> actual = sut.GetByTitle(null);
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
            string nonExistingTitle = "What is this";
            detailsMock.Setup(d => d.FindBooksByTitle(nonExistingTitle)).Returns(new List<BookData>());
            HttpResponseException exception = null;

            try
            {
                BookController sut = new BookController(detailsMock.Object);
                var actual = sut.GetByTitle(nonExistingTitle);
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
