
namespace GoogleBooksAPI
{
    public interface IDetails
    {
        BooksData FindBooksByTitle(string title);
        BooksData FindBooksByTitleAndAuthor(string title, string author);
    }
}
