using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uploader.DAL;
using Uploader.Models;
using System.Data.Entity;
using System.Net;
using System.IO;
using System.Configuration;
using System.Data.SqlClient;
using System.Text;
using System.Net.Mail;
using System.Threading;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Security;
using ContosoUniversity.DAL;

namespace Uploader.Controllers
{
    [Authorize]
    public class FileSystemController : Controller
    {
        private Context db = new Context();


        // GET: File
        public ActionResult Index(int? id)
        {
            FileSystemModel fileSystemModel = new FileSystemModel();

            if (id == null)
            {
                //error 
                // sciaga wszystkie pliki i foldery ktore sa w root katalogu
                fileSystemModel.Entries = db.Entries.Where(entry => entry.ParentEntry == null).ToList();
            } else
            {
                fileSystemModel.CurrentEntry = db.Entries.Include(entry => entry.ParentEntry).Where(entry => entry.Id == id).FirstOrDefault();
                fileSystemModel.Entries = db.Entries.Where(entry => entry.ParentEntry != null && entry.ParentEntry.Id == id).ToList();
            }

            return View(fileSystemModel);
        }


        // GET: File/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Description,ParentEntry")] Entry file, int? id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    file.ParentEntry = db.Entries.Where(entry => entry.Id == id).FirstOrDefault();
                    file.Type = "dir";
                    file.EnrollmentDate = DateTime.Now;
                    db.Entries.Add(file);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return View(file);
        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase postedFile, String url, int? id)
        {
            
            if(postedFile == null)
            {
                return RedirectToAction("Index");
            }

            //TODO: sprawdzenie czy filename istnieje w danym folderze, żeby nie było duplikatów
            var path = "";
            if(postedFile.ContentLength > 0)
            {
                var filename = Path.GetFileName(postedFile.FileName);
                path = Path.Combine(Server.MapPath("~/Uploads/"), filename);
                postedFile.SaveAs(path);
            }


            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }

            try
            {
                if (ModelState.IsValid)
                {
                    Entry file = new Entry();
                    file.ParentEntry = db.Entries.Where(entry => entry.Id == id).FirstOrDefault();
                    file.Type = "";
                    file.Name = Path.GetFileName(postedFile.FileName);
                    file.Path = path;
                    file.Size = bytes;
                    file.EnrollmentDate = DateTime.Now;

                    
                    db.Entries.Add(file);
                    db.SaveChanges();

                    
                    Mail(file.Id, url, null);

                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

           
            return RedirectToAction("Index");
        }

        [HttpPost]
        public FileResult DownloadFile(int fileId)
        {
            byte[] bytes;
            string fileName, contentType;
            string constr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Path, Size FROM Entry WHERE Id=@Id";
                    cmd.Parameters.AddWithValue("@Id", fileId);
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        sdr.Read();
                        bytes = (byte[])sdr["Size"];
                        contentType = sdr["Path"].ToString();
                        fileName = sdr["Name"].ToString();
                    }
                    con.Close();
                }
            }

            

            return File(bytes, contentType, fileName);
        }


        private static List<Entry> GetFiles()
        {
            List<Entry> files = new List<Entry>();
            string constr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT Id, Name FROM Entry"))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            files.Add(new Entry
                            {
                                Id = Convert.ToInt32(sdr["Id"]),
                                Name = sdr["Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return files;
        }


        public ActionResult AddComment(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Entry entry = db.Entries.Find(id);
            if (entry == null)
            {
                return HttpNotFound();
            }

            return View(entry);
        }


        public ActionResult Comment(int id, string text)
        {
            string constr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                string query = "INSERT INTO Comment VALUES (@Autor, @Text, @EntryId )";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@Autor", User.Identity.GetUserName());
                    cmd.Parameters.AddWithValue("@Text", text);
                    cmd.Parameters.AddWithValue("@EntryId", id);
                    
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }

            Entry entry = db.Entries.Find(id);
            return View(entry);
        }

        public Entry GetById(int id)
        {
            return this.db.Entries.FirstOrDefault(book => book.Id == id);
        }

        public void Remove(Entry entry)
        {
            List<Entry> ts = db.Entries.Where(entryy => entryy.ParentEntry != null && entryy.ParentEntry.Id == entry.Id).ToList();

            if (ts.Count == 0)
            {
                db.Entries.Remove(entry);
                db.SaveChanges();
            }
            else if (entry.Type == "dir")
            {
                TempData["msg"] = "<script>alert('Nie można usunąć folderu. Usuń zawartość folderu');</script>";
                RedirectToAction("Index", "FileSystem");
            }
            else
            {
                TempData["msg"] = "<script>alert('Wystąpił  błąd');</script>";
                RedirectToAction("Index", "FileSystem");
            }
        }

        public ActionResult Delete(int id)
        {
            var entry = GetById(id);
            Remove(entry);

            return this.RedirectToAction("Index");
        }


        public void Mail(int? id, String url, Uploader.Models.Mail model)
        {
            String userName = User.Identity.GetUserName().ToString();
            List<User> usersDoswiadczalniList = db.Users.Where(user => user.Roles == "doswiadczalny").ToList();
            List<User> usersTeoretycyList = db.Users.Where(user => user.Roles == "teoretyk").ToList();
            User currentUser = db.Users.Where(user => user.Mail == userName ).FirstOrDefault();

            if (currentUser.Roles == "teoretyk")
            {

                foreach (var addr in usersDoswiadczalniList)
                {
                    MailMessage mm = new MailMessage("projekt123grupowy@gmail.com", addr.Mail);

                    Entry file = db.Entries.Find(id);
                    mm.Subject = "New entry in Super System";
                    var fileName = file.Name;
                    var fileDesc = file.Description;
                    var fileUser = currentUser.Mail.ToString(); // from a in db.Files where a.Id == id select a.User.Name;
                    mm.Body = "Nowy plik został dodany " + "\n" + "Nazwa pliku: " + fileName + "\n" + "Autor: " + fileUser + "\n" + "Link do katalogu z plikiem: " + url + "\n" + fileDesc;
                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    NetworkCredential nc = new NetworkCredential("projekt123grupowy@gmail.com", "projekt123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Send(mm);
                }
            }

            if (currentUser.Roles == "doswiadczalny")
            {

                foreach (var addr in usersTeoretycyList)
                {
                    MailMessage mm = new MailMessage("projekt123grupowy@gmail.com", addr.Mail );

                    Entry file = db.Entries.Find(id);
                    mm.Subject = "New entry in Super System";
                    var fileName = file.Name;
                    var fileDesc = file.Description;
                    var fileUser = currentUser.Mail.ToString(); // from a in db.Files where a.Id == id select a.User.Name;
                    mm.Body = "Nowy plik został dodany " + "\n" + "Nazwa pliku: " + fileName + "\n" + "Autor: " + fileUser + "\n" + "Link do katalogu z plikiem: " + url + "\n" + fileDesc;
                    mm.IsBodyHtml = false;

                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;

                    NetworkCredential nc = new NetworkCredential("projekt123grupowy@gmail.com", "projekt123");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Send(mm);
                }
            }




        }

        public ActionResult GetUsers()
        {
            var users = Repository.GetUsers();

            return View( users );
        }




    }
}