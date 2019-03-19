using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Uploader.DAL;
using Uploader.Models;
using System.Data;
using System.Data.Entity;
using System.Configuration;
using System.Data.SqlClient;

namespace ContosoUniversity.DAL
{
    public static class Repository
    {
         static List<User> users = new List<User>() {
          //  new User() {Mail="abc@gmail.com",Password="abcadmin", Name = "aaaa" },
          //  new User() {Mail="xyz@gmail.com",Password="xyzeditor", Name = "aaaa" }
          new User() {Name="Admin",Mail="admin@gmail.com",Password="admin123"}
        };


        public static void generateUsersFromDataBase()
        {
            string constr = ConfigurationManager.ConnectionStrings["Context"].ConnectionString;
            string name, email, pass, roles;

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "SELECT Name, Mail, Password, Roles FROM [dbo].[User]";
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            name = sdr["Name"].ToString();
                            email = sdr["Mail"].ToString();
                            pass = sdr["Password"].ToString();
                            roles = sdr["Roles"].ToString();

                            users.Add(new User() { Mail = email, Password = pass, Name = name, Roles = roles });
                        }
                    }
                    con.Close();
                }
            }
        }        



        public static void addUser(String newMail, String newPass )
        {
            users.Add(new User() { Mail = newMail, Password = newPass, Name = "aaaa" });
        }

        public static IEnumerable<User> GetUsers()
        {
            return users;
        }

        public static User GetUserDetails(User user)
        {
            return users.Where(u => u.Mail.ToLower() == user.Mail.ToLower() &&
            u.Password == user.Password).FirstOrDefault();
        }
    }
}