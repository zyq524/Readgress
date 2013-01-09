using Readgress.Data.Contracts;
using Readgress.Models;
using System.Data.Entity;
using System.Linq;

namespace Readgress.Data
{
    public class ReaderRepository : EFRepository<Reader>, IReaderRepository
    {
        public ReaderRepository(DbContext context) : base(context) { }

        public override IQueryable<Reader> GetAll()
        {
            return base.GetAll().Include("Progresses.Bookmarks");
        }
    }
}
