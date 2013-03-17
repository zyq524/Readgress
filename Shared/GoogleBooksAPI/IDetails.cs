
namespace GoogleBooksAPI
{
    public interface IDetails
    {
        BookData FindBookById(string id);
        BookData FindBookByIsbn(string isbn);
        BooksData FindBooksByTitle(string title, int startIndex = 0, int maxResults = 10);
        BooksData FindBooksByTitleAndAuthor(string title, string author, int startIndex = 0, int maxResults =10);
        int FindBooksTotalItemsByTitle(string title);
        int FindBooksTotalItemsByTitleAndAuthor(string title, string author);
    }
}
