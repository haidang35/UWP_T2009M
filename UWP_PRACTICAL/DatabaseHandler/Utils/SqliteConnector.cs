using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace DatabaseHandler.Utils
{
    public class SqliteConnector
    {
        private static string DatabaseName = "uwp_exam.db";
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(DatabaseName, CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, DatabaseName);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS contacts (Primary_Key INTEGER PRIMARY KEY, " +
                    "name NVARCHAR(255) NULL, phone NVARCHAR(255))";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }
    }
}
