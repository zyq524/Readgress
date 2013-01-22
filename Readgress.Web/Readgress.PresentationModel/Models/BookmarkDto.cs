using Readgress.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Readgress.PresentationModel.Models
{
    /// <summary>
    /// Data transfer object for <see cref="Bookmark"/>
    /// </summary>
    public class BookmarkDto
    {
        public BookmarkDto() { }

        public BookmarkDto(Bookmark bookmark)
        {
            Id = bookmark.Id;
            ReaderId = bookmark.Progress.ReaderId;
            UserName = bookmark.Progress.Reader.UserName;
            ProgressId = bookmark.ProgressId;
            PageNumber = bookmark.PageNumber;
            CreatedOn = bookmark.CreatedOn;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int ReaderId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int ProgressId { get; set; }

        [Required]
        public int PageNumber { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Bookmark ToEntity()
        {
            return new Bookmark
            {
                Id = Id,
                ProgressId = ProgressId,
                PageNumber = PageNumber,
                CreatedOn = CreatedOn.HasValue ? CreatedOn.Value : DateTime.Now,
            };
        }
    }
}
