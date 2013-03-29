using System;

namespace Readgress.WP8.Models
{
    public class Bookmark
    {
        public DateTime CreatedOn { get; set; }
        public int PageNumber { get; set; }

        public string CreatedOnDisplayFormat
        {
            get
            {
                return CreatedOn.ToLongDateString();
            }
        }

        public string PageNumberDisplayFormat
        {
            get
            {
                return "Page: " + PageNumber.ToString();
            }
        }

    }
}
