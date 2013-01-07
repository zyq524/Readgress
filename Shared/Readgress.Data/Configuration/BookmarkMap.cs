using Readgress.Models;
using System.Data.Entity.ModelConfiguration;

namespace Readgress.Data.Configuration
{
    public class BookmarkMap : EntityTypeConfiguration<Bookmark>
    {
        public BookmarkMap()
        {
            HasRequired(b => b.Progress);
        }
    }
}
