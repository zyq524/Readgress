using GoogleBooksAPI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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

            bookTest = new BookData
            {
                Id = "AuMpAQAAMAA",
                VolumeInfo = new VolumeInfo
                {
                    Title = "WebApi Introdutcion",
                    SubTitle = "For Dummy",
                    Authors = new List<string>() { "Microsoft" }
                }
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_WithEmptyDetails_ThrowException()
        {
            BookController sut = new BookController(null);
        }

        [TestMethod]
        public void GetByTitle_WithValidTitle_ReturnsOneBook()
        {
            detailsMock.Setup(d => d.FindBooksByTitle(bookTest.VolumeInfo.Title, 0, 10)).Returns(new BooksData { TotalItems = 1, Items = new List<BookData>() { bookTest } });

            BookController sut = new BookController(detailsMock.Object); 
            List<BookData> actual = sut.GetByTitle(bookTest.VolumeInfo.Title);

            Assert.AreEqual(1, actual.Count);
            Assert.AreSame(bookTest, actual[0]);
        }

        [TestMethod]
        public void GetByTitle_WithValidTitle_ReturnsTwoBooks()
        {
            BookData bookTest2 = bookTest;
            bookTest2.VolumeInfo.Title += " Version1";
            List<BookData> expected = new List<BookData>() { bookTest, bookTest2 };
            detailsMock.Setup(d => d.FindBooksByTitle(bookTest.VolumeInfo.Title, 0, 10)).Returns(new BooksData { TotalItems = 2, Items = expected });

            BookController sut = new BookController(detailsMock.Object);
            List<BookData> actual = sut.GetByTitle(bookTest.VolumeInfo.Title);

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
            detailsMock.Setup(d => d.FindBooksByTitle(nonExistingTitle, 0, 10)).Returns(new BooksData());
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
