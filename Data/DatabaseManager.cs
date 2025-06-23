using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Data
{
    public static class DatabaseManager
    {
        private static readonly string connectionString = "Data Source=unicomtic.db;Version=3;";

        public static async Task<SQLiteConnection> GetOpenConnectionAsync()
        {
            try
            {
                var connection = new SQLiteConnection(connectionString);
                await connection.OpenAsync();
                return connection;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DatabaseManager.GetOpenConnectionAsync");
                throw new Exception("❌ Failed to open database connection.");
            }
        }

        public static async Task<int> ExecuteNonQueryAsync(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var conn = await GetOpenConnectionAsync())
                {
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        AddParameters(cmd, parameters);
                        return await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DatabaseManager.ExecuteNonQueryAsync");
                throw;
            }
        }

        public static async Task<SQLiteDataReader> ExecuteReaderAsync(string query, Dictionary<string, object> parameters)
        {
            SQLiteConnection conn = null;
            SQLiteCommand cmd = null;
            try
            {
                conn = await GetOpenConnectionAsync(); // Let caller close this connection

                cmd = new SQLiteCommand(query, conn);
                AddParameters(cmd, parameters);

                return (SQLiteDataReader)await cmd.ExecuteReaderAsync(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                cmd?.Dispose();
                conn?.Dispose();
                ErrorLogger.Log(ex, "DatabaseManager.ExecuteReaderAsync");
                throw new Exception("❌ Failed to execute reader.");
            }
        }

        public static async Task<object> ExecuteScalarAsync(string query, Dictionary<string, object> parameters)
        {
            try
            {
                using (var conn = await GetOpenConnectionAsync())
                {
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        AddParameters(cmd, parameters);
                        return await cmd.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DatabaseManager.ExecuteScalarAsync");
                throw new Exception("❌ Failed to execute scalar.");
            }
        }

        private static void AddParameters(SQLiteCommand cmd, Dictionary<string, object> parameters)
        {
            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                }
            }
        }
        public static async Task EnableWALModeAsync()
        {
            try
            {
                using (var conn = new SQLiteConnection("Data Source=unicomtic.db;Version=3;"))
                {
                    await conn.OpenAsync();
                    using (var cmd = new SQLiteCommand("PRAGMA journal_mode=WAL;", conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DatabaseManager.EnableWALModeAsync");
                throw;
            }
        }
    }
}
