using Readgress.Models;
using System.Data.Entity.ModelConfiguration;

namespace Readgress.Data.Configuration
{
    public class ProgressMap : EntityTypeConfiguration<Progress>
    {
        public ProgressMap()
        {
            HasRequired(p => p.Reader).WithMany(r => r.Progresses);
        }
    }
}
