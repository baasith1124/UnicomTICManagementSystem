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
    public class DepartmentRepository : IDepartmentRepository
    {
        public void AddDepartment(Department department)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.AddDepartment");
            }
        }

        public void UpdateDepartment(Department department)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "UPDATE Departments SET DepartmentName = @DepartmentName WHERE DepartmentID = @DepartmentID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
                        cmd.Parameters.AddWithValue("@DepartmentID", department.DepartmentID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.UpdateDepartment");
            }
        }

        public void DeleteDepartment(int departmentID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "DELETE FROM Departments WHERE DepartmentID = @DepartmentID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.DeleteDepartment");
            }
        }

        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT DepartmentID, DepartmentName FROM Departments";
                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            departments.Add(new Department
                            {
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.GetAllDepartments");
            }
            return departments;
        }

        public Department GetDepartmentByID(int departmentID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "SELECT DepartmentID, DepartmentName FROM Departments WHERE DepartmentID = @DepartmentID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Department
                                {
                                    DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                    DepartmentName = reader["DepartmentName"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.GetDepartmentByID");
            }
            return null;
        }
    }
    
}
