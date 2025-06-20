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
    public class SubjectRepository : ISubjectRepository
    {
        public void AddSubject(Subject subject)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"INSERT INTO Subjects (SubjectName, SubjectCode, CourseID) 
                                     VALUES (@SubjectName, @SubjectCode, @CourseID)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                        cmd.Parameters.AddWithValue("@SubjectCode", subject.SubjectCode);
                        cmd.Parameters.AddWithValue("@CourseID", subject.CourseID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.AddSubject");
            }
        }

        public void UpdateSubject(Subject subject)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"UPDATE Subjects 
                                     SET SubjectName = @SubjectName, SubjectCode = @SubjectCode, CourseID = @CourseID
                                     WHERE SubjectID = @SubjectID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectName", subject.SubjectName);
                        cmd.Parameters.AddWithValue("@SubjectCode", subject.SubjectCode);
                        cmd.Parameters.AddWithValue("@CourseID", subject.CourseID);
                        cmd.Parameters.AddWithValue("@SubjectID", subject.SubjectID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.UpdateSubject");
            }
        }

        public void DeleteSubject(int subjectID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"DELETE FROM Subjects WHERE SubjectID = @SubjectID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.DeleteSubject");
            }
        }

        public List<Subject> GetAllSubjects()
        {
            var subjects = new List<Subject>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                     FROM Subjects s
                                     INNER JOIN Courses c ON s.CourseID = c.CourseID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subjects.Add(new Subject
                            {
                                SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                SubjectName = reader["SubjectName"].ToString(),
                                SubjectCode = reader["SubjectCode"].ToString(),
                                CourseID = Convert.ToInt32(reader["CourseID"]),
                                CourseName = reader["CourseName"].ToString()
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetAllSubjects");
            }
            return subjects;
        }

        public List<Subject> SearchSubjects(string keyword)
        {
            var subjects = new List<Subject>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                     FROM Subjects s
                                     INNER JOIN Courses c ON s.CourseID = c.CourseID
                                     WHERE s.SubjectName LIKE @keyword";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(new Subject
                                {
                                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                    SubjectName = reader["SubjectName"].ToString(),
                                    SubjectCode = reader["SubjectCode"].ToString(),
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.SearchSubjects");
            }
            return subjects;
        }

        public Subject GetSubjectByID(int subjectID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                     FROM Subjects s
                                     INNER JOIN Courses c ON s.CourseID = c.CourseID
                                     WHERE s.SubjectID = @SubjectID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Subject
                                {
                                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                    SubjectName = reader["SubjectName"].ToString(),
                                    SubjectCode = reader["SubjectCode"].ToString(),
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetSubjectByID");
            }
            return null;
        }

        public List<Subject> GetSubjectsByCourse(int courseID)
        {
            var subjects = new List<Subject>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                     FROM Subjects s
                                     INNER JOIN Courses c ON s.CourseID = c.CourseID
                                     WHERE s.CourseID = @CourseID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CourseID", courseID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(new Subject
                                {
                                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                    SubjectName = reader["SubjectName"].ToString(),
                                    SubjectCode = reader["SubjectCode"].ToString(),
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetSubjectsByCourse");
            }
            return subjects;
        }
        public List<Subject> GetSubjectsByLecturer(int lecturerID)
        {
            var subjects = new List<Subject>();

            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                        FROM LecturerSubjects ls
                        INNER JOIN Subjects s ON ls.SubjectID = s.SubjectID
                        INNER JOIN Courses c ON s.CourseID = c.CourseID
                        WHERE ls.LecturerID = @LecturerID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@LecturerID", lecturerID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                subjects.Add(new Subject
                                {
                                    SubjectID = Convert.ToInt32(reader["SubjectID"]),
                                    SubjectName = reader["SubjectName"].ToString(),
                                    SubjectCode = reader["SubjectCode"].ToString(),
                                    CourseID = Convert.ToInt32(reader["CourseID"]),
                                    CourseName = reader["CourseName"].ToString()
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetSubjectsByLecturer");
            }
            return subjects;
        }

    }
}
