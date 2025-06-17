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
    public class StaffRepository : IStaffRepository
    {
        public void AddStaff(int userID, string name, int departmentID, int positionID)
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


        public void UpdateStaff(Staff staff)
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


        public void DeleteStaff(int staffID)
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

        public List<Staff> GetAllStaff()
        {
            var staffList = new List<Staff>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"
                    SELECT s.StaffID, s.UserID, s.Name, 
                           s.DepartmentID, d.DepartmentName, 
                           s.PositionID, p.PositionName
                    FROM Staff s
                    INNER JOIN Departments d ON s.DepartmentID = d.DepartmentID
                    INNER JOIN Positions p ON s.PositionID = p.PositionID";
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
                            PositionName = reader["PositionName"].ToString()
                        });
                    }
                }
            }
            return staffList;
        }

        public Staff GetStaffByID(int staffID)
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
            return null;
        }


        public List<Staff> SearchStaff(string keyword)
        {
            var staffList = new List<Staff>();
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
            return staffList;
        }

    }
}
