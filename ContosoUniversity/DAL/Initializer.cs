using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Uploader.Models;

namespace Uploader.DAL
{
    public class Initializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<Context>
    {
        protected override void Seed(Context context)
        {
            var files = new List<File>
            {/*
            new File{Name="File1",Description="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
            new File{Name="File2",Description="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new File{Name="File3",Description="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new File{Name="File4",Description="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new File{Name="File5",Description="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
            new File{Name="File6",Description="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
            new File{Name="File7",Description="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
            new File{Name="File8",Description="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")}*/
            };

            files.ForEach(s => context.Files.Add(s));
            context.SaveChanges();
            /*
            var users = new List<User>
            {
                new User{Name="Admin",Mail="admin@gmail.com",Password="admin123"}
            };
            users.ForEach(s => context.Users.Add(s));
            context.SaveChanges();
            */

            var categories = new List<Category>
            {
                new Category{Name="kategoria1", Description=""},
                new Category{Name="kategoria2", Description=""},
                new Category{Name="kategoria3", Description=""}
            };
            categories.ForEach(s => context.Categories.Add(s));
            context.SaveChanges();

            var comments = new List<Comment>
            {
            };
            comments.ForEach(s => context.Comments.Add(s));
            context.SaveChanges();
        }
    }
}