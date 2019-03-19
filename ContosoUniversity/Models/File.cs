using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Uploader.Models
{
    public class File
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Path { get; set; }
        public byte Size { get; set; }

        public virtual Category Category { get; set; }
        public virtual User User { get; set; }
    }
}