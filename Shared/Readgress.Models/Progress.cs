using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Readgress.Models
{
    public class Progress
    {
        public Progress()
        {
            this.Bookmarks = new List<Bookmark>();
        }

        [Key]
        [Required]
        public int Id { get; set; }
        public int ReaderId { get; set; }
        //ISBN 10
        public string Isbn { get; set; }

        public string GoogleBookId { get; set; }
        public bool IsFinished { get; set; }

        [JsonIgnore]
        public Reader Reader { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
    }
}
