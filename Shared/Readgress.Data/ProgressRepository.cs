using Readgress.Data.Contracts;
using Readgress.Models;
using System.Data.Entity;
using System.Linq;

namespace Readgress.Data
{
    public class ProgressRepository : EFRepository<Progress>, IProgressRepository
    {
        public ProgressRepository(DbContext context) : base(context) { }

        public Progress GetByBookmarkId(int bookmarkId)
        {
            return this.DbSet.Include("Bookmarks").FirstOrDefault(p => p.Bookmarks.FirstOrDefault(b => b.Id == bookmarkId) != null);
        }

        public IQueryable<Progress> GetByReaderId(int readerId)
        {
            return this.DbSet.Include("Bookmarks").Where(p => p.Reader.Id == readerId);
        }

        public Progress GetByOLId(string oLId)
        {
            return this.DbSet.Include("Bookmarks").FirstOrDefault(p => string.Compare(p.OLId, oLId, true) == 0);
        }
    }
}
