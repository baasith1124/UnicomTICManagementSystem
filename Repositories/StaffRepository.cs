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
        public void AddStaff(int userID, string name, int departmentID, int positionID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"INSERT INTO Staff (UserID, Name, DepartmentID, PositionID) 
                         VALUES (@UserID, @Name, @DepartmentID, @PositionID)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                        cmd.Parameters.AddWithValue("@PositionID", positionID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(AddStaff));
            }
        }

        public void UpdateStaff(Staff staff)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"UPDATE Staff SET 
                         Name = @Name, 
                         DepartmentID = @DepartmentID, 
                         PositionID = @PositionID
                         WHERE StaffID = @StaffID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Name", staff.Name);
                        cmd.Parameters.AddWithValue("@DepartmentID", staff.DepartmentID);
                        cmd.Parameters.AddWithValue("@PositionID", staff.PositionID);
                        cmd.Parameters.AddWithValue("@StaffID", staff.StaffID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(UpdateStaff));
            }
        }

        public void DeleteStaff(int staffID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "DELETE FROM Staff WHERE StaffID = @StaffID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffID", staffID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(DeleteStaff));
            }
        }

        public List<Staff> GetAllStaff()
        {
            var staffList = new List<Staff>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT s.StaffID, s.UserID, s.Name,
                               s.DepartmentID, d.DepartmentName,
                               s.PositionID, p.PositionName,
                               u.Email, u.Phone
                        FROM Staff s
                        INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                        INNER JOIN Positions p ON s.PositionID = p.PositionID
                        INNER JOIN Users u ON s.UserID = u.UserID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetAllStaff));
            }
            return staffList;
        }

        public Staff GetStaffByID(int staffID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                SELECT s.StaffID, s.UserID, s.Name, 
                       s.DepartmentID, d.DepartmentName, 
                       s.PositionID, p.PositionName
                FROM Staff s
                INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                INNER JOIN Positions p ON s.PositionID = p.PositionID
                WHERE s.StaffID = @StaffID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@StaffID", staffID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetStaffByID));
            }
            return null;
        }

        public List<Staff> SearchStaff(string keyword)
        {
            var staffList = new List<Staff>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                SELECT s.StaffID, s.UserID, s.Name, 
                       s.DepartmentID, d.DepartmentName, 
                       s.PositionID, p.PositionName
                FROM Staff s
                INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                INNER JOIN Positions p ON s.PositionID = p.PositionID
                WHERE s.Name LIKE @keyword";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(SearchStaff));
            }
            return staffList;
        }

        public Staff GetStaffByUserId(int userID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                    SELECT s.StaffID, s.UserID, s.Name, 
                           s.DepartmentID, d.DepartmentName, 
                           s.PositionID, p.PositionName
                    FROM Staff s
                    INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                    INNER JOIN Positions p ON s.PositionID = p.PositionID
                    WHERE s.UserID = @UserID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetStaffByUserId));
            }
            return null;
        }

        public bool StaffExistsByUserId(int userId)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT COUNT(1) FROM Staff WHERE UserID = @UserID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserID", userId);
                        return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(StaffExistsByUserId));
                return false;
            }
        }

        public int GetUserIDByStaffID(int staffID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    using (var cmd = new SQLiteCommand("SELECT UserID FROM Staff WHERE StaffID = @staffID", conn))
                    {
                        cmd.Parameters.AddWithValue("@staffID", staffID);

                        var result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int userID))
                        {
                            return userID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, nameof(GetUserIDByStaffID));
            }
            return -1;
        }
    }
}
