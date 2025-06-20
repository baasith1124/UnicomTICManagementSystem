using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Data;
using UnicomTICManagementSystem.Helpers;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;

namespace UnicomTICManagementSystem.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE Username = @Username";
                var parameters = new Dictionary<string, object> { { "@Username", username } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                        return MapReaderToUser(reader);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.GetUserByUsernameAsync");
            }
            return null;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE Email = @Email";
                var parameters = new Dictionary<string, object> { { "@Email", email } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                        return MapReaderToUser(reader);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.GetUserByEmailAsync");
            }
            return null;
        }

        public async Task<User> GetUserByIDAsync(int userID)
        {
            try
            {
                string query = "SELECT * FROM Users WHERE UserID = @UserID";
                var parameters = new Dictionary<string, object> { { "@UserID", userID } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                        return MapReaderToUser(reader);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.GetUserByIDAsync");
            }
            return null;
        }

        public async Task RegisterUserAsync(User user)
        {
            try
            {
                string query = @"
                    INSERT INTO Users 
                    (Username, Password, Role, FullName, Email, Phone, RegisteredDate, IsApproved) 
                    VALUES 
                    (@Username, @Password, @Role, @FullName, @Email, @Phone, @RegisteredDate, @IsApproved)";

                var parameters = new Dictionary<string, object>
                {
                    { "@Username", user.Username },
                    { "@Password", user.Password },
                    { "@Role", user.Role },
                    { "@FullName", user.FullName },
                    { "@Email", user.Email },
                    { "@Phone", user.Phone },
                    { "@RegisteredDate", user.RegisteredDate.ToString("yyyy-MM-dd") },
                    { "@IsApproved", user.IsApproved ? 1 : 0 }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.RegisterUserAsync");
            }
        }

        public async Task ApproveUserAsync(int userID)
        {
            try
            {
                string query = "UPDATE Users SET IsApproved = 1 WHERE UserID = @UserID";
                var parameters = new Dictionary<string, object> { { "@UserID", userID } };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.ApproveUserAsync");
            }
        }

        public async Task<List<User>> GetPendingApprovalsAsync()
        {
            var users = new List<User>();
            try
            {
                string query = "SELECT * FROM Users WHERE IsApproved = 0";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(MapReaderToUser(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.GetPendingApprovalsAsync");
            }
            return users;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var users = new List<User>();
            try
            {
                string query = "SELECT * FROM Users";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        users.Add(MapReaderToUser(reader));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.GetUsersAsync");
            }
            return users;
        }

        private User MapReaderToUser(SQLiteDataReader reader)
        {
            try
            {
                return new User
                {
                    UserID = Convert.ToInt32(reader["UserID"]),
                    Username = reader["Username"].ToString(),
                    Password = reader["Password"].ToString(),
                    FullName = reader["FullName"].ToString(),
                    Role = reader["Role"].ToString(),
                    Email = reader["Email"].ToString(),
                    Phone = reader["Phone"].ToString(),
                    RegisteredDate = DateTime.Parse(reader["RegisteredDate"].ToString()),
                    IsApproved = Convert.ToInt32(reader["IsApproved"]) == 1
                };
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "UserRepository.MapReaderToUser");
                return null;
            }
        }

    }
}
