using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Uploader.Models
{
    public class Mail
    {
        [Key]
        public int Id { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Path { get; set; }

        public virtual Category Category { get; set; }
        public virtual File File { get; set; }
    }
}