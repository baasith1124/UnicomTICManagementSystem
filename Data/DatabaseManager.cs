using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Data
{
    public class DatabaseManager
    {
        private static readonly string connectionString = "Data Source=unicomtic.db;Version=3;";

        private DatabaseManager() { }

        public static SQLiteConnection GetConnection()
        {
            try
            {
                var connection = new SQLiteConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DatabaseManager.GetConnection");
                throw new Exception("Database connection failed. Please check the logs for details.");
            }
        }
    }
}
