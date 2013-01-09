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
        public int Id { get; set; }
        //Open Library Id
        public string OLId { get; set; }

        public bool IsFinished { get; set; }

        [JsonIgnore]
        public Reader Reader { get; set; }
        public ICollection<Bookmark> Bookmarks { get; set; }
    }
}
