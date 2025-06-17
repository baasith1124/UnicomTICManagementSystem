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
    public class StudentRepository : IStudentRepository
    {
        public void AddStudent(int userID, string name, int courseID, DateTime enrollmentDate)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"INSERT INTO Students 
                                (UserID, Name, CourseID, EnrollmentDate)
                                VALUES (@UserID, @Name, @CourseID, @EnrollmentDate)";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    cmd.Parameters.AddWithValue("@EnrollmentDate", enrollmentDate.ToString("yyyy-MM-dd"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"UPDATE Students SET 
                                    Name = @Name,
                                    CourseID = @CourseID,
                                    EnrollmentDate = @EnrollmentDate
                                WHERE StudentID = @StudentID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", student.Name);
                    cmd.Parameters.AddWithValue("@CourseID", student.CourseID);
                    cmd.Parameters.AddWithValue("@EnrollmentDate", student.EnrollmentDate.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@StudentID", student.StudentID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteStudent(int studentID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "DELETE FROM Students WHERE StudentID = @StudentID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName 
                                 FROM Students s
                                 INNER JOIN Courses c ON s.CourseID = c.CourseID";

                using (var cmd = new SQLiteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        students.Add(new Student
                        {
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            Name = reader["Name"].ToString(),
                            CourseID = Convert.ToInt32(reader["CourseID"]),
                            EnrollmentDate = DateTime.Parse(reader["EnrollmentDate"].ToString()),
                            CourseName = reader["CourseName"].ToString() // For DataGridView display
                        });
                    }
                }
            }
            return students;
        }

        public List<Student> SearchStudents(string keyword)
        {
            var students = new List<Student>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName 
                                 FROM Students s
                                 INNER JOIN Courses c ON s.CourseID = c.CourseID
                                 WHERE s.Name LIKE @keyword";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            }
            return students;
        }

        public Student GetStudentByID(int studentID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate 
                                 FROM Students s
                                 WHERE s.StudentID = @StudentID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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
            }
            return null;
        }

        public StudentDetails GetStudentFullDetailsByID(int studentID)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"
            SELECT s.StudentID, s.UserID, u.Username, u.Email, u.Phone, 
                   s.Name as FullName, s.CourseID, c.CourseName, s.EnrollmentDate
            FROM Students s
            INNER JOIN Users u ON s.UserID = u.UserID
            INNER JOIN Courses c ON s.CourseID = c.CourseID
            WHERE s.StudentID = @StudentID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StudentID", studentID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
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
            }
            return null;
        }

        public List<Student> GetStudentsByCourse(int courseID)
        {
            var students = new List<Student>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName
                         FROM Students s
                         INNER JOIN Courses c ON s.CourseID = c.CourseID
                         WHERE s.CourseID = @CourseID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            }
            return students;
        }

        public List<Student> GetStudentsBySubject(int subjectID)
        {
            var students = new List<Student>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT s.StudentID, s.Name, s.CourseID, s.EnrollmentDate, c.CourseName
                         FROM Students s
                         INNER JOIN Courses c ON s.CourseID = c.CourseID
                         INNER JOIN Subjects subj ON s.CourseID = subj.CourseID
                         WHERE subj.SubjectID = @SubjectID";

                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            }
            return students;
        }

    }
}
