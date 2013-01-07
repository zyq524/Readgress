using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Readgress.Models
{
    public class Reader
    {
        public Reader()
        {
            this.Progresses = new List<Progress>();
        }

        public int Id { get; set; }

        [EmailAddress]
        public string Email { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Concat(FirstName + " " + LastName);
            }
        }

        public DateTime CreatedOn { get; set; }

        public ICollection<Progress> Progresses { get; set; }
    }
}
