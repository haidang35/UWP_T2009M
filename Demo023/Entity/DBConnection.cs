using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data;
using MySql.Data.MySqlClient;


namespace Demo023.Entity
{
    public class DBConnection
    {
       
        public static MySqlConnection Connect()
        {
            string connStr = "server=localhost;user=root;database=demo_uwp;port=3306;password=";
            MySqlConnection conn = new MySqlConnection(connStr);
            return conn;
        }
       
    }
}
