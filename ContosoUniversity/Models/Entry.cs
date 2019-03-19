using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Uploader.Models
{
    public class Entry
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Path { get; set; }
        public byte[] Size { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public User User { get; set; }
        public Entry ParentEntry { get; set; }
        public string Type { get; set; }
        
    }
}