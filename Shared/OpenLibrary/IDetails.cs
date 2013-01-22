using System.Collections.Generic;
using System.Threading.Tasks;

namespace OpenLibrary
{
    public interface IDetails
    {
        List<BookData> FindBooksByOLIDs(List<string> oLIDs);
        Task<List<BookData>> FindBooksByOLIDsAsync(List<string> oLIDs);
        List<BookData> FindBooksByTitle(string title);
        Task<List<BookData>> FindBooksByTitleAsync(string title);
        List<string> FindOLIDsByTitle(string title);
        Task<List<string>> FindOLIDsByTitleAsync(string title);
    }
}
