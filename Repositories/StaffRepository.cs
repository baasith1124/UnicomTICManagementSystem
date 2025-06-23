using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnicomTICManagementSystem.Data;
using UnicomTICManagementSystem.Interfaces;
using UnicomTICManagementSystem.Models;
using UnicomTICManagementSystem.Helpers;

namespace UnicomTICManagementSystem.Repositories
{
    public class StaffRepository : IStaffRepository
    {
        public async Task AddStaffAsync(int userID, string name, int departmentID, int positionID)
        {
            try
            {
                string query = @"INSERT INTO Staff (UserID, Name, DepartmentID, PositionID) 
                         VALUES (@UserID, @Name, @DepartmentID, @PositionID)";

                var parameters = new Dictionary<string, object>
                {
                    { "@UserID", userID },
                    { "@Name", name },
                    { "@DepartmentID", departmentID },
                    { "@PositionID", positionID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(AddStaffAsync));
            }
        }


        public async Task UpdateStaffAsync(Staff staff)
        {
            try
            {
                string query = @"UPDATE Staff SET 
                         Name = @Name, 
                         DepartmentID = @DepartmentID, 
                         PositionID = @PositionID
                         WHERE StaffID = @StaffID";

                var parameters = new Dictionary<string, object>
                {
                    { "@Name", staff.Name },
                    { "@DepartmentID", staff.DepartmentID },
                    { "@PositionID", staff.PositionID },
                    { "@StaffID", staff.StaffID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(UpdateStaffAsync));
            }
        }


        public async Task DeleteStaffAsync(int staffID)
        {
            try
            {
                //Get associated UserID from Staff table
                string getUserQuery = "SELECT UserID FROM Staff WHERE StaffID = @StaffID";
                var getParams = new Dictionary<string, object> { { "@StaffID", staffID } };
                object userIdObj = await DatabaseManager.ExecuteScalarAsync(getUserQuery, getParams);

                if (userIdObj != null)
                {
                    int userID = Convert.ToInt32(userIdObj);

                    //Delete from Staff table
                    string deleteStaffQuery = "UPDATE Staff SET Status = 'Inactive' WHERE StaffID = @StaffID";
                    await DatabaseManager.ExecuteNonQueryAsync(deleteStaffQuery, getParams);

                    //Delete from Users table
                    string deleteUserQuery = "UPDATE Users SET Status = 'Inactive' WHERE UserID = @UserID";
                    var deleteUserParams = new Dictionary<string, object> { { "@UserID", userID } };
                    await DatabaseManager.ExecuteNonQueryAsync(deleteUserQuery, deleteUserParams);
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(DeleteStaffAsync));
            }
        }


        public async Task<List<Staff>> GetAllStaffAsync()
        {
            var staffList = new List<Staff>();
            try
            {
                string query = @"
                    SELECT s.StaffID, s.UserID, s.Name,
                           s.DepartmentID, d.DepartmentName,
                           s.PositionID, p.PositionName,
                           u.Email, u.Phone
                    FROM Staff s
                    INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                    INNER JOIN Positions p ON s.PositionID = p.PositionID
                    INNER JOIN Users u ON s.UserID = u.UserID
                    WHERE s.Status = 'Active' AND u.Status = 'Active'";

                var reader = await DatabaseManager.ExecuteReaderAsync(query, null);
                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        staffList.Add(new Staff
                        {
                            StaffID = Convert.ToInt32(reader["StaffID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            PositionID = Convert.ToInt32(reader["PositionID"]),
                            PositionName = reader["PositionName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetAllStaffAsync));
            }
            return staffList;
        }


        public async Task<Staff> GetStaffByIDAsync(int staffID)
        {
            try
            {
                string query = @"
                    SELECT s.StaffID, s.UserID, s.Name, 
                           s.DepartmentID, d.DepartmentName, 
                           s.PositionID, p.PositionName
                    FROM Staff s
                    INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                    INNER JOIN Positions p ON s.PositionID = p.PositionID
                    WHERE s.Status = 'Active' AND s.StaffID = @StaffID";

                var parameters = new Dictionary<string, object>
                {
                    { "@StaffID", staffID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Staff
                        {
                            StaffID = Convert.ToInt32(reader["StaffID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            PositionID = Convert.ToInt32(reader["PositionID"]),
                            PositionName = reader["PositionName"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetStaffByIDAsync));
            }

            return null;
        }


        public async Task<List<Staff>> SearchStaffAsync(string keyword)
        {
            var staffList = new List<Staff>();
            try
            {
                string query = @"
                    SELECT s.StaffID, s.UserID, s.Name, 
                           s.DepartmentID, d.DepartmentName, 
                           s.PositionID, p.PositionName
                    FROM Staff s
                    INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                    INNER JOIN Positions p ON s.PositionID = p.PositionID
                    WHERE s.Status = 'Active' AND u.Status = 'Active' AND s.Name LIKE @keyword";

                var parameters = new Dictionary<string, object>
                {
                    { "@keyword", $"%{keyword}%" }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        staffList.Add(new Staff
                        {
                            StaffID = Convert.ToInt32(reader["StaffID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            PositionID = Convert.ToInt32(reader["PositionID"]),
                            PositionName = reader["PositionName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(SearchStaffAsync));
            }
            return staffList;
        }


        public async Task<Staff> GetStaffByUserIdAsync(int userID)
        {
            try
            {
                string query = @"
                    SELECT s.StaffID, s.UserID, s.Name, 
                           s.DepartmentID, d.DepartmentName, 
                           s.PositionID, p.PositionName
                    FROM Staff s
                    INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                    INNER JOIN Positions p ON s.PositionID = p.PositionID
                    WHERE s.Status = 'Active' AND s.UserID = @UserID";

                var parameters = new Dictionary<string, object>
                {
                    { "@UserID", userID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Staff
                        {
                            StaffID = Convert.ToInt32(reader["StaffID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            PositionID = Convert.ToInt32(reader["PositionID"]),
                            PositionName = reader["PositionName"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetStaffByUserIdAsync));
            }

            return null;
        }


        public async Task<bool> StaffExistsByUserIdAsync(int userId)
        {
            try
            {
                string query = "SELECT COUNT(1) FROM Staff WHERE Status = 'Active' AND UserID = @UserID";
                var parameters = new Dictionary<string, object>
                {
                    { "@UserID", userId }
                };

                var result = await DatabaseManager.ExecuteScalarAsync(query, parameters);
                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(StaffExistsByUserIdAsync));
                return false;
            }
        }


        public async Task<int> GetUserIDByStaffIDAsync(int staffID)
        {
            try
            {
                string query = "SELECT UserID FROM Staff WHERE Status = 'Active' AND StaffID = @staffID";
                var parameters = new Dictionary<string, object>
                {
                    { "@staffID", staffID }
                };

                var result = await DatabaseManager.ExecuteScalarAsync(query, parameters);
                if (result != null && int.TryParse(result.ToString(), out int userID))
                {
                    return userID;
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetUserIDByStaffIDAsync));
            }
            return -1;
        }
        public async Task UpdateStaffNameByUserIdAsync(int userId, string newName)
        {
            string query = "UPDATE Staff SET Name = @Name WHERE UserID = @UserID";
            var parameters = new Dictionary<string, object>
            {
                { "@Name", newName },
                { "@UserID", userId }
            };

            await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
        }


    }
}
