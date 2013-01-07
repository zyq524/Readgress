using Readgress.Models;
using System.Linq;

namespace Readgress.Data.Contracts
{
    public interface IBookmarkRepository : IRepository<Bookmark>
    {
        IQueryable<Bookmark> GetByProgressId(int progressId);
        IQueryable<Bookmark> GetByReaderId(int readerId);
    }
}
