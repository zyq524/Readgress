using Readgress.Data.Contracts;
using Readgress.Models;
using System.Data.Entity;
using System.Linq;

namespace Readgress.Data
{
    public class BookmarkRepository : EFRepository<Bookmark>, IBookmarkRepository
    {
        public BookmarkRepository(DbContext context) : base(context) { }

        public IQueryable<Bookmark> GetByProgressId(int progressId)
        {
            return this.DbSet.Where(b => b.Progress.Id == progressId);
        }

        public IQueryable<Bookmark> GetByReaderId(int readerId)
        {
            return this.DbSet.Where(b => b.Progress.Reader.Id == readerId);
        }

    }
}
