
namespace GoogleBooksAPI
{
    public interface IDetails
    {
        BooksData FindBooksByTitle(string title, int startIndex = 0, int maxResults = 10);
        BooksData FindBooksByTitleAndAuthor(string title, string author, int startIndex = 0, int maxResults =10);
    }
}
