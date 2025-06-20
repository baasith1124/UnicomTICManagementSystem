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
    public class StudentRepository : IStudentRepository
    {
        public async Task AddStudentAsync(int userID, string name, int courseID, DateTime enrollmentDate)
        {
            try
            {
                string query = @"INSERT INTO Students (UserID, Name, CourseID, EnrollmentDate)
                                 VALUES (@UserID, @Name, @CourseID, @EnrollmentDate)";

                var parameters = new Dictionary<string, object>
                {
                    {"@UserID", userID},
                    {"@Name", name},
                    {"@CourseID", courseID},
                    {"@EnrollmentDate", enrollmentDate.ToString("yyyy-MM-dd")}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.AddStudentAsync");
            }
        }

        public async Task UpdateStudentAsync(Student student)
        {
            try
            {
                string query = @"UPDATE Students SET Name = @Name, CourseID = @CourseID, EnrollmentDate = @EnrollmentDate
                                 WHERE StudentID = @StudentID";

                var parameters = new Dictionary<string, object>
                {
                    {"@Name", student.Name},
                    {"@CourseID", student.CourseID},
                    {"@EnrollmentDate", student.EnrollmentDate.ToString("yyyy-MM-dd")},
                    {"@StudentID", student.StudentID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.UpdateStudentAsync");
            }
        }

        public async Task DeleteStudentAsync(int studentID)
        {
            try
            {
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                var parameters = new Dictionary<string, object> { { "@StudentID", studentID } };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.DeleteStudentAsync");
            }
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = new List<Student>();
            try
            {
                string query = @"SELECT s.StudentID, s.UserID, s.Name, s.CourseID, s.EnrollmentDate,
                                       c.CourseName, u.Email, u.Phone
                                FROM Students s
                                INNER JOIN Courses c ON s.CourseID = c.CourseID
                                INNER JOIN Users u ON s.UserID = u.UserID";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString()),
                            CourseName = reader["CourseName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetAllStudentsAsync");
            }
            return students;
        }
        //-----------

        public async Task<List<Student>> SearchStudentsAsync(string keyword)
        {
            var students = new List<Student>();
            try
            {
                string query = @"
            SELECT s.StudentID, s.UserID, s.Name, s.CourseID, s.EnrollmentDate, 
                   c.CourseName, u.Email, u.Phone
            FROM Students s
            INNER JOIN Courses c ON s.CourseID = c.CourseID
            INNER JOIN Users u ON s.UserID = u.UserID
            WHERE s.Name LIKE @keyword";

                var parameters = new Dictionary<string, object>
        {
            { "@keyword", $"%{keyword}%" }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString()),
                            CourseName = reader["CourseName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.SearchStudentsAsync");
            }

            return students;
        }



        public async Task<Student> GetStudentByIDAsync(int studentID)
        {
            try
            {
                string query = @"
            SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate 
            FROM Students s
            WHERE s.StudentID = @StudentID";

                var parameters = new Dictionary<string, object>
        {
            { "@StudentID", studentID }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString())
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetStudentByIDAsync");
            }

            return null;
        }

        public async Task<StudentDetails> GetStudentFullDetailsByIDAsync(int studentID)
        {
            try
            {
                string query = @"
            SELECT s.StudentID, s.UserID, u.Username, u.Email, u.Phone, 
                   s.Name as FullName, s.CourseID, c.CourseName, s.EnrollmentDate
            FROM Students s
            INNER JOIN Users u ON s.UserID = u.UserID
            INNER JOIN Courses c ON s.CourseID = c.CourseID
            WHERE s.StudentID = @StudentID";

                var parameters = new Dictionary<string, object>
        {
            { "@StudentID", studentID }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new StudentDetails
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            UserID = Convert.ToInt32(reader["UserID"]),
                            Username = reader["Username"].ToString(),
                            FullName = reader["FullName"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            CourseName = reader["CourseName"].ToString(),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString())
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetStudentFullDetailsByIDAsync");
            }

            return null;
        }


        public async Task<List<Student>> GetStudentsByCourseAsync(int courseID)
        {
            var students = new List<Student>();
            try
            {
                string query = @"
            SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName
            FROM Students s
            INNER JOIN Courses c ON s.CourseID = c.CourseID
            WHERE s.CourseID = @CourseID";

                var parameters = new Dictionary<string, object>
        {
            { "@CourseID", courseID }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString()),
                            CourseName = reader["CourseName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetStudentsByCourseAsync");
            }

            return students;
        }


        public async Task<List<Student>> GetStudentsBySubjectAsync(int subjectID)
        {
            var students = new List<Student>();
            try
            {
                string query = @"
            SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName
            FROM Students s
            INNER JOIN Courses c ON s.CourseID = c.CourseID
            INNER JOIN Subjects subj ON s.CourseID = subj.CourseID
            WHERE subj.SubjectID = @SubjectID";

                var parameters = new Dictionary<string, object>
        {
            { "@SubjectID", subjectID }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString()),
                            CourseName = reader["CourseName"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetStudentsBySubjectAsync");
            }

            return students;
        }

        public async Task<Student> GetStudentByUserIdAsync(int userID)
        {
            try
            {
                string query = @"
            SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName
            FROM Students s
            INNER JOIN Courses c ON s.CourseID = c.CourseID
            WHERE s.UserID = @UserID";

                var parameters = new Dictionary<string, object>
        {
            { "@UserID", userID }
        };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            UserID = userID,
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString()),
                            CourseName = reader["CourseName"].ToString()
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetStudentByUserIdAsync");
            }

            return null;
        }

        public async Task<int> GetStudentIDByUserIDAsync(int userID)
        {
            try
            {
                string query = "SELECT StudentID FROM Students WHERE UserID = @userID";

                var parameters = new Dictionary<string, object>
        {
            { "@userID", userID }
        };

                var result = await DatabaseManager.ExecuteScalarAsync(query, parameters);

                return result != null ? Convert.ToInt32(result) : -1;
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "StudentRepository.GetStudentIDByUserIDAsync");
                return -1;
            }
        }





    }
}
