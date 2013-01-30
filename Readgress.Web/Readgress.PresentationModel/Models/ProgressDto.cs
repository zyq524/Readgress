using Readgress.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Readgress.PresentationModel.Models
{
    /// <summary>
    /// Data transfer object for <see cref="Progress"/>
    /// </summary>
    public class ProgressDto
    {
        public ProgressDto() { }

        public ProgressDto(Progress progress)
        {
            Id = progress.Id;
            ReaderId = progress.ReaderId;
            UserName = progress.Reader.UserName;
            OLId = progress.OLId;
            IsFinished = progress.IsFinished;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public int ReaderId { get; set; }

        [Required]
        public string UserName { get; set; }

        //Open Library Id
        [Required]
        public string OLId { get; set; }

        public bool IsFinished { get; set; }

        public Progress ToEntity()
        {
            return new Progress
            {
                Id = Id,
                ReaderId = ReaderId,
                OLId = OLId,
                IsFinished = IsFinished,
            };
        }
    }
}
