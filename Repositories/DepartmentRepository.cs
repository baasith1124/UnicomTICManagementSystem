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
        public async Task AddDepartmentAsync(Department department)
        {
            try
            {
                string query = "INSERT INTO Departments (DepartmentName) VALUES (@DepartmentName)";
                var parameters = new Dictionary<string, object>
                {
                    {"@DepartmentName", department.DepartmentName}
                };
                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.AddDepartmentAsync");
            }
        }

        public async Task UpdateDepartmentAsync(Department department)
        {
            try
            {
                string query = "UPDATE Departments SET DepartmentName = @DepartmentName WHERE DepartmentID = @DepartmentID";
                var parameters = new Dictionary<string, object>
                {
                    {"@DepartmentName", department.DepartmentName},
                    {"@DepartmentID", department.DepartmentID}
                };
                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.UpdateDepartmentAsync");
            }
        }

        public async Task DeleteDepartmentAsync(int departmentID)
        {
            try
            {
                string query = "DELETE FROM Departments WHERE DepartmentID = @DepartmentID";
                var parameters = new Dictionary<string, object>
                {
                    {"@DepartmentID", departmentID}
                };
                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.DeleteDepartmentAsync");
            }
        }

        public async Task<List<Department>> GetAllDepartmentsAsync()
        {
            var departments = new List<Department>();
            try
            {
                string query = "SELECT DepartmentID, DepartmentName FROM Departments";
                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        departments.Add(new Department
                        {
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.GetAllDepartmentsAsync");
            }
            return departments;
        }

        public async Task<Department> GetDepartmentByIDAsync(int departmentID)
        {
            try
            {
                string query = "SELECT DepartmentID, DepartmentName FROM Departments WHERE DepartmentID = @DepartmentID";
                var parameters = new Dictionary<string, object>
                {
                    {"@DepartmentID", departmentID}
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Department
                        {
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "DepartmentRepository.GetDepartmentByIDAsync");
            }
            return null;
        }
    }
    
}
