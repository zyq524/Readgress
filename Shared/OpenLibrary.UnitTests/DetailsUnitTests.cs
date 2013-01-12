using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenLibrary.UnitTests
{
    [TestClass]
    public class DetailsUnitTests
    {
        private Details sut = new Details();

        [TestMethod]
        public void FindOLIDsByTitle_WithValidTitle_ReturnsOLIDs()
        {
            string title = "Python Essential Reference";

            List<string> actual = sut.FindOLIDsByTitle(title);

            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindOLIDsByTitle_WithEmptyTitle_ShouldThrowArgumentNullException()
        {

            List<string> actual = sut.FindOLIDsByTitle(string.Empty);
        }

        [TestMethod]
        public void FindOLIDsByTitle_WithNonExistingTitle_ReturnsEmpty()
        {
            string title = "Yuqing Zhang";

            List<string> actual = sut.FindOLIDsByTitle(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FindOLIDsByTitle_WithInvalidTitle_ReturnsEmpty()
        {
            string title = "Python Essential Reference\"";

            List<string> actual = sut.FindOLIDsByTitle(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FindBooksByTitle_WithValidTitle_ReturnsBooks()
        {
            string title = "Python Essential Reference";

            List<BookData> actual = sut.FindBooksByTitle(title);

            Assert.AreEqual(4, actual.Count);
            foreach (var book in actual)
            {
                Assert.IsTrue(book.Title.Contains(title));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindBooksByTitle_WithEmptyTitle_ShouldThrowArgumentNullException()
        {

            List<BookData> actual = sut.FindBooksByTitle(string.Empty);
        }

        [TestMethod]
        public void FindBooksByTitle_WithNonExistingTitle_ReturnsEmpty()
        {
            string title = "Yuqing Zhang";

            List<BookData> actual = sut.FindBooksByTitle(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FindBooksByTitle_WithInvalidTitle_ReturnsEmpty()
        {
            string title = "Python Essential Reference\"";

            List<BookData> actual = sut.FindBooksByTitle(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public void FindBooksByOLIDs_WithValidOLIDs_ReturnsBooks()
        {
            List<string> oLIDs = new List<string>()
            {
                "OL24778132M",
                "OL24652845M",
            };

            List<string> oLIDsWithOneInvalidOLID = oLIDs;
            oLIDsWithOneInvalidOLID.Add("123XX");
            List<BookData> actual = sut.FindBooksByOLIDs(oLIDsWithOneInvalidOLID);

            Assert.AreEqual(2, actual.Count);
            foreach (var book in actual)
            {
                Assert.IsTrue(oLIDs.Contains(book.Identifiers.OpenLibrary[0]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void FindBooksByOLIDs_WithNullOLIDs_ShouldThrowArgumentNullException()
        {

            List<BookData> actual = sut.FindBooksByOLIDs(null);
        }

        [TestMethod]
        public void FindBooksByOLIDs_WithEmptyOLIDs_ReturnsEmpty()
        {

            List<BookData> actual = sut.FindBooksByOLIDs(new List<string>());

            Assert.IsTrue(actual.Count == 0);
        }

        [TestMethod]
        public void FindBooksByOLIDs_WithInvalidOLID_ReturnsEmpty()
        {

            List<BookData> actual = sut.FindBooksByOLIDs(new List<string>() { "123" });

            Assert.IsTrue(actual.Count == 0);
        }

        [TestMethod]
        public async Task FindOLIDsByTitleAsync_WithValidTitle_ReturnsOLIDs()
        {
            string title = "Python Essential Reference";

            List<string> actual = await sut.FindOLIDsByTitleAsync(title);

            Assert.AreEqual(4, actual.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task FindOLIDsByTitleAsync_WithEmptyTitle_ShouldThrowArgumentNullException()
        {

            List<string> actual = await sut.FindOLIDsByTitleAsync(string.Empty);
        }

        [TestMethod]
        public async Task FindOLIDsByTitleAsync_WithNonExistingTitle_ReturnsEmpty()
        {
            string title = "Yuqing Zhang";

            List<string> actual = await sut.FindOLIDsByTitleAsync(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public async Task FindOLIDsByTitleAsync_WithInvalidTitle_ReturnsEmpty()
        {
            string title = "Python Essential Reference\"";

            List<string> actual = await sut.FindOLIDsByTitleAsync(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public async Task FindBooksByTitleAsync_WithValidTitle_ReturnsBooks()
        {
            string title = "Python Essential Reference";

            List<BookData> actual = await sut.FindBooksByTitleAsync(title);

            Assert.AreEqual(4, actual.Count);
            foreach (var book in actual)
            {
                Assert.IsTrue(book.Title.Contains(title));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task FindBooksByTitleAsync_WithEmptyTitle_ShouldThrowArgumentNullException()
        {

            List<BookData> actual = await sut.FindBooksByTitleAsync(string.Empty);
        }

        [TestMethod]
        public async Task FindBooksByTitleAsync_WithNonExistingTitle_ReturnsEmpty()
        {
            string title = "Yuqing Zhang";

            List<BookData> actual = await sut.FindBooksByTitleAsync(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public async Task FindBooksByTitleAsync_WithInvalidTitle_ReturnsEmpty()
        {
            string title = "Python Essential Reference\"";

            List<BookData> actual = await sut.FindBooksByTitleAsync(title);

            Assert.AreEqual(0, actual.Count);
        }

        [TestMethod]
        public async Task FindBooksByOLIDsAsync_WithValidOLIDs_ReturnsBooks()
        {
            List<string> oLIDs = new List<string>()
            {
                "OL24778132M",
                "OL24652845M",
            };

            List<string> oLIDsWithOneInvalidOLID = oLIDs;
            oLIDsWithOneInvalidOLID.Add("123XX");
            List<BookData> actual = await sut.FindBooksByOLIDsAsync(oLIDsWithOneInvalidOLID);

            Assert.AreEqual(2, actual.Count);
            foreach (var book in actual)
            {
                Assert.IsTrue(oLIDs.Contains(book.Identifiers.OpenLibrary[0]));
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task FindBooksByOLIDsAsync_WithNullOLIDs_ShouldThrowArgumentNullException()
        {

            List<BookData> actual = await sut.FindBooksByOLIDsAsync(null);
        }

        [TestMethod]
        public async Task FindBooksByOLIDsAsync_WithEmptyOLIDs_ReturnsEmpty()
        {

            List<BookData> actual = await sut.FindBooksByOLIDsAsync(new List<string>());

            Assert.IsTrue(actual.Count == 0);
        }

        [TestMethod]
        public async Task FindBooksByOLIDsAsync_WithInvalidOLID_ReturnsEmpty()
        {

            List<BookData> actual = await sut.FindBooksByOLIDsAsync(new List<string>() { "123" });

            Assert.IsTrue(actual.Count == 0);
        }
    }
}
