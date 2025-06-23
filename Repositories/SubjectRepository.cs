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
        public async Task AddSubjectAsync(Subject subject)
        {
            try
            {
                string query = @"INSERT INTO Subjects (SubjectName, SubjectCode, CourseID) 
                                 VALUES (@SubjectName, @SubjectCode, @CourseID)";

                var parameters = new Dictionary<string, object>
                {
                    {"@SubjectName", subject.SubjectName},
                    {"@SubjectCode", subject.SubjectCode},
                    {"@CourseID", subject.CourseID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.AddSubjectAsync");
            }
        }

        public async Task UpdateSubjectAsync(Subject subject)
        {
            try
            {
                string query = @"UPDATE Subjects 
                                 SET SubjectName = @SubjectName, SubjectCode = @SubjectCode, CourseID = @CourseID
                                 WHERE SubjectID = @SubjectID";

                var parameters = new Dictionary<string, object>
                {
                    {"@SubjectName", subject.SubjectName},
                    {"@SubjectCode", subject.SubjectCode},
                    {"@CourseID", subject.CourseID},
                    {"@SubjectID", subject.SubjectID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.UpdateSubjectAsync");
            }
        }

        public async Task DeleteSubjectAsync(int subjectID)
        {
            try
            {
                string query = "UPDATE Subjects SET Status = 'Inactive' WHERE SubjectID = @SubjectID";
                var parameters = new Dictionary<string, object> { { "@SubjectID", subjectID } };
                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.DeleteSubjectAsync");
            }
        }

        public async Task<List<Subject>> GetAllSubjectsAsync()
        {
            var subjects = new List<Subject>();
            try
            {
                string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                 FROM Subjects s
                                 INNER JOIN Courses c ON s.CourseID = c.CourseID
                                 WHERE s.Status = 'Active'";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetAllSubjectsAsync");
            }
            return subjects;
        }

        public async Task<List<Subject>> SearchSubjectsAsync(string keyword)
        {
            var subjects = new List<Subject>();
            try
            {
                string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                 FROM Subjects s
                                 INNER JOIN Courses c ON s.CourseID = c.CourseID
                                 WHERE WHERE s.Status = 'Active' AND s.SubjectName LIKE @keyword";

                var parameters = new Dictionary<string, object> { { "@keyword", "%" + keyword + "%" } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.SearchSubjectsAsync");
            }
            return subjects;
        }

        public async Task<Subject> GetSubjectByIDAsync(int subjectID)
        {
            try
            {
                string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                 FROM Subjects s
                                 INNER JOIN Courses c ON s.CourseID = c.CourseID
                                 WHERE s.Status = 'Active' AND s.SubjectID = @SubjectID";

                var parameters = new Dictionary<string, object> { { "@SubjectID", subjectID } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetSubjectByIDAsync");
            }
            return null;
        }

        public async Task<List<Subject>> GetSubjectsByCourseAsync(int courseID)
        {
            var subjects = new List<Subject>();
            try
            {
                string query = @"SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                                 FROM Subjects s
                                 INNER JOIN Courses c ON s.CourseID = c.CourseID
                                 WHERE s.Status = 'Active' AND s.CourseID = @CourseID";

                var parameters = new Dictionary<string, object> { { "@CourseID", courseID } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetSubjectsByCourseAsync");
            }
            return subjects;
        }

        public async Task<List<Subject>> GetSubjectsByLecturerAsync(int lecturerID)
        {
            var subjects = new List<Subject>();
            try
            {
                string query = @"
                    SELECT s.SubjectID, s.SubjectName, s.SubjectCode, s.CourseID, c.CourseName
                    FROM LecturerSubjects ls
                    INNER JOIN Subjects s ON ls.SubjectID = s.SubjectID
                    INNER JOIN Courses c ON s.CourseID = c.CourseID
                    WHERE s.Status = 'Active' AND ls.LecturerID = @LecturerID";

                var parameters = new Dictionary<string, object> { { "@LecturerID", lecturerID } };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "SubjectRepository.GetSubjectsByLecturerAsync");
            }
            return subjects;
        }

    }
}
