using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayerDemo.Entities
{
    public enum AccountGender
    {
        Male = 1,
        Female = 0,
        Other = 2,
    }
    public class Account
    {
        public int id { get; set; }
        public int role { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string address { get; set; }
        public string avatar { get; set; }
        public int gender { get; set; }
        public string birthday { get; set; }
        public string introduction { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public int status { get; set; }

        public string GetUserRole()
        {
            return role == 99 ? "Admin" : "User";
        }

        public string GetUserGender()
        {
            return gender == 1 ? "Male" : gender == 0 ? "Female" : "Other";
        }

        public string GetStatus()
        {
            return status == 1 ? "Active" : "Deactive"; 
        }
    }
}
