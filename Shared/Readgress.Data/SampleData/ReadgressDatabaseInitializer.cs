using Readgress.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace Readgress.Data.SampleData
{
    public class ReadgressDatabaseInitializer : DropCreateDatabaseIfModelChanges<ReadgressDbContext>
    {
        protected override void Seed(ReadgressDbContext context)
        {
            var reader = new Reader()
            {
                Email = "zhangyuqing524@gmail.com",
                FirstName = "Yuqing",
                LastName = "Zhang",
                CreatedOn = DateTime.Now
            };

            var bookmark1 = new Bookmark()
            {
                PageNumber = 20,
                CreatedOn = DateTime.Now
            };

            var bookmark2 = new Bookmark()
            {
                PageNumber = 50,
                CreatedOn = DateTime.Now
            };

            var progess = new Progress()
            {
                Bookmarks = new List<Bookmark>()
                {
                    bookmark1,bookmark2
                },
                Reader = reader,
                IsFinished = false
            };

            context.Readers.Add(reader);
            context.Bookmarks.Add(bookmark1);
            context.Bookmarks.Add(bookmark2);
            context.Progresses.Add(progess);

            context.SaveChanges();
        }
    }
}
