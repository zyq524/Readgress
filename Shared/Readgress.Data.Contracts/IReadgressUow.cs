using Readgress.Models;

namespace Readgress.Data.Contracts
{
    public interface IReadgressUow
    {
        void Commit();

        IReaderRepository Readers { get; }
        IProgressRepository Progresses { get; }
        IBookmarkRepository Bookmarks { get; }
    }
}
