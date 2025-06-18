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
    public class CourseRepository : ICourseRepository
    {
        public void AddCourse(Course course)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "INSERT INTO Courses (CourseName, Description, DepartmentID) VALUES (@CourseName, @Description, @DepartmentID)";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseName", course.CourseName);
                    cmd.Parameters.AddWithValue("@Description", course.Description);
                    cmd.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void UpdateCourse(Course course)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "UPDATE Courses SET CourseName = @CourseName, Description = @Description, DepartmentID = @DepartmentID WHERE CourseID = @CourseID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", course.CourseID);
                    cmd.Parameters.AddWithValue("@CourseName", course.CourseName);
                    cmd.Parameters.AddWithValue("@Description", course.Description);
                    cmd.Parameters.AddWithValue("@DepartmentID", course.DepartmentID);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCourse(int courseId)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "DELETE FROM Courses WHERE CourseID = @CourseID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Course GetCourseById(int courseId)
        {
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Courses WHERE CourseID = @CourseID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseID", courseId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return MapReaderToCourse(reader);
                        }
                    }
                }
            }
            return null;
        }

        public List<Course> GetAllCourses()
        {
            var courses = new List<Course>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT c.*, d.DepartmentName 
                                FROM Courses c 
                                LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID
                                ";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(MapReaderToCourse(reader));
                        }
                    }
                }
            }
            return courses;
        }

        private Course MapReaderToCourse(SQLiteDataReader reader)
        {
            return new Course
            {
                CourseID = Convert.ToInt32(reader["CourseID"]),
                CourseName = reader["CourseName"].ToString(),
                Description = reader["Description"].ToString(),
                DepartmentName = reader["DepartmentName"] != DBNull.Value ? reader["DepartmentName"].ToString() : string.Empty,
                DepartmentID = reader["DepartmentID"] != DBNull.Value ? Convert.ToInt32(reader["DepartmentID"]) : 0

            };
        }

        public List<Course> SearchCoursesByName(string courseName)
        {
            var courses = new List<Course>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = @"SELECT c.*, d.DepartmentName 
                                FROM Courses c 
                                LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID
                                WHERE c.CourseName LIKE @CourseName
                                ";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseName", "%" + courseName + "%");
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(MapReaderToCourse(reader));
                        }
                    }
                }
            }
            return courses;
        }
        public List<Course> GetCoursesByDepartment(int departmentId)
        {
            var courses = new List<Course>();
            using (var conn = DatabaseManager.GetConnection())
            {
                string query = "SELECT * FROM Courses WHERE DepartmentID = @DepartmentID";
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(MapReaderToCourse(reader));
                        }
                    }
                }
            }
            return courses;
        }


    }
}
