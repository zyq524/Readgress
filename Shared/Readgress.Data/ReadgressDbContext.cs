using Readgress.Data.Configuration;
using Readgress.Data.SampleData;
using Readgress.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Readgress.Data
{
    public class ReadgressDbContext : DbContext
    {
        // ToDo: Move Initializer to Global.asax; don't want dependence on SampleData
        //static ReadgressDbContext()
        //{
        //    Database.SetInitializer(new ReadgressDatabaseInitializer());
        //}

        public ReadgressDbContext()
            : base(nameOrConnectionString: "ReadgressDbContext") 
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Use singular table names
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ProgressMap());
            modelBuilder.Configurations.Add(new BookmarkMap());
        }

        public DbSet<Reader> Readers { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }
        public DbSet<Progress> Progresses { get; set; }
    }
}
