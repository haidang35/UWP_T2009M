using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseHandler.Utils
{
    class ConnectionMySQL
    {
        private static string serverName = "localhost";
        private static string databaseName = "demo_uwp";
        private static string userName = "root";
        private static string password = "";
        private static string port = "3306";
        private static string connectionString = $"server={serverName};user={userName};database={databaseName};port={port};password={password}";


        public static MySqlConnection connectionMySql()
        {
            var connect = new MySqlConnection(connectionString);
            return connect;
        }
    }
}
