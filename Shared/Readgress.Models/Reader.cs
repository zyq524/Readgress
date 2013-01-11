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

        public string UserName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string FullName
        {
            get
            {
                return string.Concat(FirstName + " " + LastName);
            }
        }
        public string Gender { get; set; }

        public string Link { get; set; }
        //public string ProfilePictureUrlSmall
        //{
        //    get
        //    {
        //        return string.Format("http://graph.facebook.com/{0}/picture?type=small", FacebookId);
        //    }
        //}

        //public string ProfilePictureUrlNormal
        //{
        //    get
        //    {
        //        return string.Format("http://graph.facebook.com/{0}/picture?type=normal", FacebookId);
        //    }
        //}

        //public string ProfilePictureUrlLarge
        //{
        //    get
        //    {
        //        return string.Format("http://graph.facebook.com/{0}/picture?type=large", FacebookId);
        //    }
        //}

        public DateTime CreatedOn { get; set; }

        public ICollection<Progress> Progresses { get; set; }
    }
}
