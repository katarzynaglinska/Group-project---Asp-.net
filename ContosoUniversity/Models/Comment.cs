using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Uploader.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int EntryId { get; set; }
        public string Autor { get; set; }
        public string Text { get; set; }

        [Required]
        public virtual Entry Entry { get; set; }
        /*[Required]
        public virtual User User { get; set; }*/
    }
}