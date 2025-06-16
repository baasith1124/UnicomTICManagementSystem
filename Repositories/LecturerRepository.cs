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
    public class LecturerRepository : ILecturerRepository
    {
        public void AddLecturer(int userID, string name, int departmentID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"INSERT INTO Lecturers 
                            (UserID, Name, DepartmentID)
                            VALUES (@UserID, @Name, @DepartmentID)";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateLecturer(Lecturer lecturer)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"UPDATE Lecturers SET 
                                Name = @Name,
                                DepartmentID = @DepartmentID
                                WHERE LecturerID = @LecturerID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", lecturer.Name);
                    cmd.Parameters.AddWithValue("@DepartmentID", lecturer.DepartmentID);
                    cmd.Parameters.AddWithValue("@LecturerID", lecturer.LecturerID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteLecturer(int lecturerID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "DELETE FROM Lecturers WHERE LecturerID = @LecturerID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Lecturer> GetAllLecturers()
        {
            var lecturers = new List<Lecturer>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT l.LecturerID, l.Name, l.DepartmentID, d.DepartmentName
                             FROM Lecturers l
                             INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lecturers.Add(new Lecturer
                        {
                            LecturerID = Convert.ToInt32(reader["LecturerID"]),
                            Name = reader["Name"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                            DepartmentName = reader["DepartmentName"].ToString()
                        });
                    }
                }
            }
            return lecturers;
        }

        public List<Lecturer> SearchLecturers(string keyword)
        {
            var lecturers = new List<Lecturer>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT l.LecturerID, l.Name, l.DepartmentID, d.DepartmentName 
                             FROM Lecturers l
                             INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID
                             WHERE l.Name LIKE @keyword";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lecturers.Add(new Lecturer
                            {
                                LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                Name = reader["Name"].ToString(),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString()
                            });
                        }
                    }
                }
            }
            return lecturers;
        }

        public Lecturer GetLecturerByID(int lecturerID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT l.LecturerID, l.Name, l.DepartmentID, d.DepartmentName
                             FROM Lecturers l
                             INNER JOIN Departments d ON l.DepartmentID = d.DepartmentID
                             WHERE l.LecturerID = @LecturerID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Lecturer
                            {
                                LecturerID = Convert.ToInt32(reader["LecturerID"]),
                                Name = reader["Name"].ToString(),
                                DepartmentID = Convert.ToInt32(reader["DepartmentID"]),
                                DepartmentName = reader["DepartmentName"].ToString()
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
