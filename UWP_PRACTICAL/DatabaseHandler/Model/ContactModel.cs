using DatabaseHandler.Entities;
using DatabaseHandler.Utils;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DatabaseHandler.Model
{
    public class ContactModel
    {
        private static string DatabaseName = "uwp_exam.db";
        public bool Save(Contact contact)
        {
            try
            {
                string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseName);
                using (SqliteConnection db =
                  new SqliteConnection($"Filename={dbpath}"))
                    {
                        db.Open();
                        SqliteCommand insertCommand = new SqliteCommand();
                        insertCommand.Connection = db;
                        insertCommand.CommandText = "INSERT INTO contacts VALUES (@Name, @Phone);";
                        insertCommand.Parameters.AddWithValue("@Name", contact.Name);
                        insertCommand.Parameters.AddWithValue("@Phone", contact.Phone);
                        insertCommand.ExecuteReader();
                        return true;
                    }
            }
            catch (Exception ex)
            {
                return false;
            }
           
        }

        public  Contact FindByName(string name)
        {
            List<Contact> contactList = new List<Contact>();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseName);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ($"SELECT * from contacts where name like %{name}% ", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    var contact = new Contact()
                    {
                        Name = query.GetString("name"),
                        Phone = query.GetString("phone"),
                    };
                    contactList.Add(contact);
                }
                db.Close();
            }
            return contactList[0];
        }

    }
}
