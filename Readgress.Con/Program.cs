using Readgress.Data;
using Readgress.Data.Helpers;
using Readgress.Data.SampleData;
using Readgress.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Readgress.Con
{
    class Program
    {
        static void Main(string[] args)
        {
            //Database.SetInitializer(
            //    new DropCreateDatabaseAlways<ReadgressDbContext>());
            Database.SetInitializer(new ReadgressDatabaseInitializer());
            var factories = new RepositoryFactories();
            var repoistoryProvider = new RepositoryProvider(factories);
            //var readgressUow = new ReadgressUow(repoistoryProvider);

            using (var context = new ReadgressUow(repoistoryProvider))
            //using (var context = new ReadgressDbContext())
            {
                var reader = context.Readers.GetAll().First();
                var progress = context.Progresses.GetByReaderId(1);
                var bookmark = context.Bookmarks.GetAll();
                Console.WriteLine(reader.FullName);
            }

            Console.WriteLine("Done");
        }
    }
}
