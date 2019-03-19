using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uploader.DAL;

namespace Uploader.Models
{
    public class FileSystemModel
    {
        public Entry CurrentEntry { get; set; }
        public List<Entry> Entries { get; set; }
    }
}