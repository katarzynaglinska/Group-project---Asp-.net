using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uploader.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;


namespace Uploader.DAL
{ 
    public class Context : DbContext
    {
        public Context() : base("Context")
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Mail> Mails { get; set; }
        public DbSet<Entry> Entries { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}