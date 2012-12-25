using System;
using System.Collections.Generic;

namespace OpenLibrary
{
    public class BookData
    {
        public string Title;
        public string SubTitle;
        public List<Subject> Subjects;
        public List<Author> Authors;
        public string Url;
        public Identifier Identifiers;
        public List<Publish> Publishers;
        public List<Publish> Publish_Places;
        public string Publish_Date;
        public Cover Cover;
        public int Number_Of_Pages;
    }

    public class Author
    {
        public string Name;
        public string Url;
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
        public string Small;
        public string Medium;
        public string Large;
    }

    public class Subject
    {
        public string Url;
        public string Name;
    }

    public class Publish
    {
        public string Name;
    }
}
