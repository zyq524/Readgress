using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace Readgress.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        public int ProgressId { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        
        [JsonIgnore]
        public Progress Progress { get; set; }
    }
}
