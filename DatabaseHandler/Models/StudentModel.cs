using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseHandler.Entity;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace DatabaseHandler.Models
{

    public class StudentModel
    {

        private static string serverName = "localhost";
        private static string userName = "root";
        private static string databaseName = "demo_uwp";
        private static string port = "3306";
        private static string password = "";
        private static string connectionString = $"server={serverName};user={userName};database={databaseName};port={port};password={password}";

        public static List<Student> findAll()
        {
            var studentList = new List<Student>();
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "select * from students";
                MySqlCommand command = new MySqlCommand(query, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var rollNumber = reader["roll_number"];
                    var fullName = reader.GetString("name");
                    var birthday = reader.GetDateTime("birthday");
                    var phone = reader.GetString("phone");
                    var email = reader.GetString("email");
                    var address = reader.GetString("address");
                    var gender = reader.GetInt32("gender");
                    var status = reader.GetInt32("status");
                
                    studentList.Add(new Student
                    { 
                        RollNumber = (String)rollNumber,
                        FullName = fullName,
                        Dob = birthday,
                        Email = email,
                        Phone = (String)phone,
                        Gender = gender,
                        Status = (StudentStatus)status,
                        Address = address
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                conn.Close();
            }

            return studentList;
        }

        public static void findById(int studentId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = $"select * from students where id = {studentId}";
                MySqlCommand command = new MySqlCommand(query, conn);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var RollNumber = reader["roll_number"];
                    var FullName = reader["name"];
                    var Birthday = reader["birthday"];
                    Debug.WriteLine($"Roll Number {RollNumber}, FullName {FullName}, Birthday {Birthday}");
                }
            }
        }
        public static bool Save(Student student)
        {
            MySqlConnection conn = new MySqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = "INSERT INTO students(roll_number, name, birthday, gender, phone, email,  address)" +
                       " VALUES(@RollNumber, @Name, @Birthday, @Gender, @Phone, @Email, @Address)";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@RollNumber", student.RollNumber);
                command.Parameters.AddWithValue("@Name", student.FullName);
                command.Parameters.AddWithValue("@Birthday",$"{student.Dob.Year}-{student.Dob.Month}-{student.Dob.Day}");
                command.Parameters.AddWithValue("@Gender", student.Gender);
                command.Parameters.AddWithValue("@Phone", student.Phone);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@Address", student.Address);
                command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            finally
            {
                 conn.Close();
            }

          

        }

        public static bool Update(string studentId, Student student)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = $"update students set roll_number=@RollNumber, name=@FullName, birthday=@Birthday" +
                    $", phone=@Phone, email=@Email, address=@Address , gender=@Gender where roll_number = '{studentId}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.Parameters.AddWithValue("@RollNumber", student.RollNumber);
                command.Parameters.AddWithValue("@FullName", student.FullName);
                command.Parameters.AddWithValue("@Birthday", $"{student.Dob.Year}-{student.Dob.Month}-{student.Dob.Day}");
                command.Parameters.AddWithValue("@Gender", student.Gender);
                command.Parameters.AddWithValue("@Phone", student.Phone);
                command.Parameters.AddWithValue("@Email", student.Email);
                command.Parameters.AddWithValue("@Address", student.Address);
                command.ExecuteNonQuery();
                Debug.WriteLine("Update success");
                return true;
                
            }
        }

        public static bool Delete(string studentId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = $"delete from students where roll_number = '{studentId}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
                Debug.WriteLine("Delete success");
                return true;
            }
        }

        public static bool DeleteMulti(List<String> rollNumbers)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                foreach(var rollNumber in rollNumbers)
                {
                    string query = $"delete from students where roll_number = '{rollNumber}' ";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                }
                return true;
            }
        }
    }
}
