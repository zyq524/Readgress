using Readgress.Data.Contracts;
using Readgress.Models;
using System.Data.Entity;
using System.Linq;

namespace Readgress.Data
{
    public class BookmarkRepository : EFRepository<Bookmark>, IBookmarkRepository
    {
        public BookmarkRepository(DbContext context) : base(context) { }

        public override Bookmark GetById(int Id)
        {
            return base.GetAll().Include("Progress.Reader").FirstOrDefault(b => b.Id == Id);
        }

        public IQueryable<Bookmark> GetByProgressId(int progressId)
        {
            return this.DbSet.Where(b => b.ProgressId == progressId);
        }

        public IQueryable<Bookmark> GetByReaderId(int readerId)
        {
            return this.DbSet.Where(b => b.Progress.ReaderId == readerId);
        }

    }
}
