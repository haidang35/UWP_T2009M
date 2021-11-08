using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHandler.Entity
{
    public enum StudentStatus
    {
        Active = 1,
        Unactive = 0,
        Deleted = -1
    }
    public class Student
    {
 
        public string RollNumber { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Dob { get; set; }
        public string Address { get; set; }
        public int Gender { get; set; }
        public StudentStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public string ToString()
        {
            return $"Roll Number {RollNumber}, Fullname {FullName}, Birthday {Dob}, Email {Email}, " +
                $"Phone {Phone}, Address {Address}, Gender {Gender}";
        }

    }
}
