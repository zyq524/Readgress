using System;
using System.ComponentModel.DataAnnotations;

namespace Readgress.Models
{
    public class Bookmark
    {
        [Key]
        public int Id { get; set; }
        public int PageNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        
        public Progress Progress { get; set; }
    }
}
