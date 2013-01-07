using Readgress.Models;
using System.Linq;

namespace Readgress.Data.Contracts
{
    public interface IProgressRepository : IRepository<Progress>
    {
        Progress GetByBookmarkId(int bookmarkId);
        IQueryable<Progress> GetByReaderId(int readerId);
    }
}
