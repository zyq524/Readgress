using Readgress.Data.Contracts;
using Readgress.Models;
using System.Data.Entity;
using System.Linq;

namespace Readgress.Data
{
    public class ProgressRepository : EFRepository<Progress>, IProgressRepository
    {
        public ProgressRepository(DbContext context) : base(context) { }

        public override IQueryable<Progress> GetAll()
        {
            return base.GetAll().Include("Bookmarks").Include("Reader");
        }

        public override Progress GetById(int Id)
        {
            return this.GetAll().FirstOrDefault(p => p.Id == Id);
        }

        public Progress GetByBookmarkId(int bookmarkId)
        {
            return this.GetAll().FirstOrDefault(p => p.Bookmarks.FirstOrDefault(b => b.Id == bookmarkId) != null);
        }

        public IQueryable<Progress> GetByReaderId(int readerId)
        {
            return this.GetAll().Where(p => p.ReaderId == readerId);
        }

        public Progress GetByOLId(string oLId)
        {
            return this.GetAll().FirstOrDefault(p => string.Compare(p.OLId, oLId, true) == 0);
        }
    }
}
