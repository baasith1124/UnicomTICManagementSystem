using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnicomTICManagementSystem.Data
{
    public class DatabaseManager
    {
        private static readonly string connectionString = "Data Source=unicomtic.db;Version=3;";

        private DatabaseManager() { }

        public static SQLiteConnection GetConnection()
        {
            var connection = new SQLiteConnection(connectionString);
            connection.Open();
            return connection;
        }
    }
}
