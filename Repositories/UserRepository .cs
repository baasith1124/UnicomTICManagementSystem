using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Data;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        public User GetUserByUsername(string username)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE Username = @Username";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", username);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToUser(reader);
                        }
                    }
                }
                return null;
            }
        }
        public User GetUserByEmail(string email)
        {
            string query = "SELECT * FROM Users WHERE Email = @Email";
            using (var conn = DatabaseManager.GetConnection())
            {
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToUser(reader);
                        }
                    }
                }
            }
            return null;
        }


        public User GetUserByID(int userID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE UserID = @UserID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToUser(reader);
                        }
                    }
                }
                return null;
            }
        }

        public void RegisterUser(User user)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"INSERT INTO Users 
        (Username, Password, Role, FullName, Email, Phone, RegisteredDate, IsApproved) 
        VALUES 
        (@Username, @Password, @Role, @FullName, @Email, @Phone, @RegisteredDate, @IsApproved)";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@Role", user.Role);
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@Phone", user.Phone);
                    cmd.Parameters.AddWithValue("@RegisteredDate", user.RegisteredDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@IsApproved", user.IsApproved ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        public void ApproveUser(int userID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Users SET IsApproved = 1 WHERE UserID = @UserID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<User> GetPendingApprovals()
        {
            var users = new List<User>();

            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Users WHERE IsApproved = 0";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            users.Add(MapReaderToUser(reader));
                        }
                    }
                }
            }
            return users;
        }

        // 🔧 Private helper to map reader to model object
        private User MapReaderToUser(SQLiteDataReader reader)
        {
            return new User
            {
                UserID = Convert.ToInt32(reader["UserID"]),
                Username = reader["Username"].ToString(),
                Password = reader["Password"].ToString(),
                Role = reader["Role"].ToString(),
                Email = reader["Email"].ToString(),
                Phone = reader["Phone"].ToString(),
                RegisteredDate = DateTime.Parse(reader["RegisteredDate"].ToString()),
                IsApproved = Convert.ToInt32(reader["IsApproved"]) == 1
            };
        }
    }
}
