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
    public class CourseRepository : ICourseRepository
    {
        public async Task AddCourseAsync(Course course)
        {
            try
            {
                string query = "INSERT INTO Courses (CourseName, Description, DepartmentID) VALUES (@CourseName, @Description, @DepartmentID)";
                var parameters = new Dictionary<string, object>
                {
                    {"@CourseName", course.CourseName},
                    {"@Description", course.Description},
                    {"@DepartmentID", course.DepartmentID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.AddCourseAsync");
            }
        }


        public async Task UpdateCourseAsync(Course course)
        {
            try
            {
                string query = "UPDATE Courses SET CourseName = @CourseName, Description = @Description, DepartmentID = @DepartmentID WHERE CourseID = @CourseID";
                var parameters = new Dictionary<string, object>
                {
                    {"@CourseID", course.CourseID},
                    {"@CourseName", course.CourseName},
                    {"@Description", course.Description},
                    {"@DepartmentID", course.DepartmentID}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.UpdateCourseAsync");
            }
        }


        public async Task DeleteCourseAsync(int courseId)
        {
            try
            {
                string query = "DELETE FROM Courses WHERE CourseID = @CourseID";
                var parameters = new Dictionary<string, object>
                {
                    { "@CourseID", courseId }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.DeleteCourseAsync");
            }
        }


        public async Task<Course> GetCourseByIdAsync(int courseId)
        {
            try
            {
                string query = "SELECT * FROM Courses WHERE CourseID = @CourseID";
                var parameters = new Dictionary<string, object>
                {
                    { "@CourseID", courseId }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return MapReaderToCourse(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.GetCourseByIdAsync");
            }
            return null;
        }


        public async Task<List<Course>> GetAllCoursesAsync()
        {
            var courses = new List<Course>();
            try
            {
                string query = @"SELECT c.*, d.DepartmentName 
                         FROM Courses c 
                         LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        var course = MapReaderToCourse(reader);
                        if (course != null)
                            courses.Add(course);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.GetAllCoursesAsync");
            }
            return courses;
        }


        public async Task<List<Course>> SearchCoursesByNameAsync(string courseName)
        {
            var courses = new List<Course>();
            try
            {
                string query = @"SELECT c.*, d.DepartmentName 
                         FROM Courses c 
                         LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID
                         WHERE c.CourseName LIKE @CourseName";

                var parameters = new Dictionary<string, object>
                {
                    { "@CourseName", "%" + courseName + "%" }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        var course = MapReaderToCourse(reader);
                        if (course != null)
                            courses.Add(course);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.SearchCoursesByNameAsync");
            }
            return courses;
        }


        public async Task<List<Course>> GetCoursesByDepartmentAsync(int departmentId)
        {
            var courses = new List<Course>();
            try
            {
                string query = "SELECT * FROM Courses WHERE DepartmentID = @DepartmentID";
                var parameters = new Dictionary<string, object>
                {
                    { "@DepartmentID", departmentId }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        var course = MapReaderToCourse(reader);
                        if (course != null)
                            courses.Add(course);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.GetCoursesByDepartmentAsync");
            }
            return courses;
        }


        private Course MapReaderToCourse(SQLiteDataReader reader)
        {
            try
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
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "CourseRepository.MapReaderToCourse");
                return null;
            }
        }


    }
}
