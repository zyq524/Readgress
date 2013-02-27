using System.Collections.Generic;
using System.Linq;

namespace Readgress.WP8.Models
{
    public class Book 
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Url { get; set; }
        public string Publish_Date { get; set; }
        public int Number_Of_Pages { get; set; }

        public Cover Cover;
        public List<Subject> Subjects;
        public List<Author> Authors;
        public List<Publisher> Publishers;

        public string TrimmedTitle
        {
            get
            {
                return !string.IsNullOrEmpty(Title) && Title.Length > 17 ? Title.Substring(0, 17) + "..." : Title;
            }
        }

        public string SubjectsStr
        {
            get
            {
                return Subjects != null ? string.Join(",", Subjects.Select(x => x.Name).ToArray()) : string.Empty;
            }
        }

        public string AuthorsStr
        {
            get
            {
                string a = Authors != null ? string.Join(",", Authors.Select(x => x.Name).ToArray()) : string.Empty;
                return a.Length > 22 ? a.Substring(0, 20) + "..." : a;
            }
        }

        public string PublishersStr
        {
            get
            {
                return Publishers != null ? string.Join(",", Publishers.Select(x => x.Name).ToArray()) : string.Empty;
            }
        }

        public string Cover_Small
        {
            get
            {
                return Cover == null || string.IsNullOrEmpty(Cover.Small) ? "/Assets/cover_s.png" : Cover.Small;
            }
        }

        public string Cover_Medium
        {
            get
            {
                return Cover == null || string.IsNullOrEmpty(Cover.Medium) ? "/Assets/cover_m.png" : Cover.Medium;
            }
        }

        public string Cover_Large
        {
            get
            {
                return Cover == null || string.IsNullOrEmpty(Cover.Large) ? "/Assets/cover_l.png" : Cover.Large;
            }
        }
    }

    public class Author
    {
        public string Name { get; set; }
        public string Url { get; set; }
    }

    public class Identifier
    {
        public List<string> Isbn10;
        public List<string> isbn13;
        public List<string> Lccn;
        public List<string> OpenLibrary;
        public List<string> Oclc;
    }

    public class Cover
    {
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Large { get; set; }
    }

    public class Subject
    {
        public string Url;
        public string Name;
    }

    public class Publisher
    {
        public string Name;
    }

}
