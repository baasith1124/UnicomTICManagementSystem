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
    public class LecturerRepository : ILecturerRepository
    {
        public async Task AddLecturerAsync(int userID, string name, int departmentID)
        {
            try
            {
                if (await LecturerExistsByUserIdAsync(userID))
                    throw new Exception("❌ Lecturer already exists for this user.");

                string query = @"INSERT INTO Lecturers 
                         (UserID, Name, DepartmentID) 
                         VALUES (@UserID, @Name, @DepartmentID)";

                var parameters = new Dictionary<string, object>
                {
                    { "@UserID", userID },
                    { "@Name", name },
                    { "@DepartmentID", departmentID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.AddLecturerAsync");
                throw;
            }
        }


        public async Task<bool> LecturerExistsByUserIdAsync(int userId)
        {
            try
            {
                string query = "SELECT COUNT(1) FROM Lecturers WHERE UserID = @UserID";

                var parameters = new Dictionary<string, object>
                {
                    { "@UserID", userId }
                };

                object result = await DatabaseManager.ExecuteScalarAsync(query, parameters);

                return Convert.ToInt32(result) > 0;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.LecturerExistsByUserIdAsync");
                return false;
            }
        }


        public async Task UpdateLecturerAsync(Lecturer lecturer)
        {
            try
            {
                string query = @"
                    UPDATE Lecturers SET 
                        Name = @Name,
                        DepartmentID = @DepartmentID
                    WHERE LecturerID = @LecturerID";

                var parameters = new Dictionary<string, object>
                {
                    { "@Name", lecturer.Name },
                    { "@DepartmentID", lecturer.DepartmentID },
                    { "@LecturerID", lecturer.LecturerID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.UpdateLecturerAsync");
            }
        }


        public async Task DeleteLecturerAsync(int lecturerID)
        {
            try
            {
                string query = "DELETE FROM Lecturers WHERE LecturerID = @LecturerID";

                var parameters = new Dictionary<string, object>
                {
                    { "@LecturerID", lecturerID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.DeleteLecturerAsync");
            }
        }


        public async Task<List<Lecturer>> GetAllLecturersAsync()
        {
            var lecturers = new List<Lecturer>();
            try
            {
                string query = @"
                    SELECT l.LecturerID, l.UserID, l.Name, 
                           l.DepartmentID, d.DepartmentName,
                           u.Email, u.Phone
                    FROM Lecturers l
                    INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID
                    INNER JOIN Users u ON l.UserID = u.UserID";

                var reader = await DatabaseManager.ExecuteReaderAsync(query, null);
                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        lecturers.Add(new Lecturer
                        {
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.GetAllLecturersAsync");
            }
            return lecturers;
        }


        public async Task<List<Lecturer>> SearchLecturersAsync(string keyword)
        {
            var lecturers = new List<Lecturer>();
            try
            {
                string query = @"
                    SELECT l.LecturerID, l.UserID, l.Name, 
                           l.DepartmentID, d.DepartmentName
                    FROM Lecturers l
                    INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID
                    WHERE l.Name LIKE @keyword";

                var parameters = new Dictionary<string, object>
                {
                    { "@keyword", $"%{keyword}%" }
                };

                var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters);
                using (reader)
                {
                    while (await reader.ReadAsync())
                    {
                        lecturers.Add(new Lecturer
                        {
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.SearchLecturersAsync");
            }
            return lecturers;
        }


        public async Task<Lecturer> GetLecturerByIDAsync(int lecturerID)
        {
            try
            {
                string query = @"
                    SELECT l.LecturerID, l.UserID, l.Name, 
                           l.DepartmentID, d.DepartmentName
                    FROM Lecturers l
                    INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID
                    WHERE l.LecturerID = @LecturerID";

                var parameters = new Dictionary<string, object>
                {
                    { "@LecturerID", lecturerID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Lecturer
                        {
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.GetLecturerByIDAsync");
            }
            return null;
        }


        public async Task<Lecturer> GetLecturerByUserIdAsync(int userID)
        {
            try
            {
                string query = @"
                    SELECT l.LecturerID, l.UserID, l.Name, 
                           l.DepartmentID, d.DepartmentName
                    FROM Lecturers l
                    INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID
                    WHERE l.UserID = @UserID";

                var parameters = new Dictionary<string, object>
                {
                    { "@UserID", userID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Lecturer
                        {
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.GetLecturerByUserIdAsync");
            }
            return null;
        }


        public async Task<int> GetLecturerIDByUserIDAsync(int userID)
        {
            try
            {
                string query = "SELECT LecturerID FROM Lecturers WHERE UserID = @userID";

                var parameters = new Dictionary<string, object>
                {
                    { "@userID", userID }
                };

                object result = await DatabaseManager.ExecuteScalarAsync(query, parameters);

                return result != null && int.TryParse(result.ToString(), out int lecturerID)
                    ? lecturerID
                    : -1;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "LecturerRepository.GetLecturerIDByUserIDAsync");
                return -1;
            }
        }




    }
}
