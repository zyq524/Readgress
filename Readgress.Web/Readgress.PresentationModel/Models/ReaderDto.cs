using Readgress.Models;
using System;

namespace Readgress.PresentationModel.Models
{
    /// <summary>
    /// Data transfer object for <see cref="Reader"/>
    /// </summary>
    public class ReaderDto
    {
        public ReaderDto() { }

        public ReaderDto(Reader reader)
        {
            ReaderId = reader.Id;
            UserName = reader.UserName;
            FullName = reader.FullName;
            Gender = reader.Gender;
            Link = reader.Link;
            CreatedOn = reader.CreatedOn;
        }

        public int ReaderId { get; set; }

        public string UserName { get; set; }

        public string FullName { get; set; }

        public string Gender { get; set; }

        public string Link { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
