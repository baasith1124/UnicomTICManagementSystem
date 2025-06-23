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
                // 1. Soft delete course
                string updateCourseQuery = "UPDATE Courses SET Status = 'Inactive' WHERE CourseID = @CourseID";
                var parameters = new Dictionary<string, object> { { "@CourseID", courseId } };
                await DatabaseManager.ExecuteNonQueryAsync(updateCourseQuery, parameters);

                // 2. Soft delete subjects
                string updateSubjectsQuery = "UPDATE Subjects SET Status = 'Inactive' WHERE CourseID = @CourseID";
                await DatabaseManager.ExecuteNonQueryAsync(updateSubjectsQuery, parameters);

                // 3. Soft delete students
                string updateStudentsQuery = "UPDATE Students SET Status = 'Inactive' WHERE CourseID = @CourseID";
                await DatabaseManager.ExecuteNonQueryAsync(updateStudentsQuery, parameters);

                // 3a. Soft delete related Users
                string getStudentUserIDs = "SELECT UserID FROM Students WHERE CourseID = @CourseID AND Status = 'Inactive'";
                using (var reader = await DatabaseManager.ExecuteReaderAsync(getStudentUserIDs, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        int userID = reader.GetInt32(0);
                        string updateUserQuery = "UPDATE Users SET Status = 'Inactive' WHERE UserID = @UserID";
                        await DatabaseManager.ExecuteNonQueryAsync(updateUserQuery, new Dictionary<string, object> { { "@UserID", userID } });
                    }
                }

                // 4. Soft delete assigned subjects
                string updateAssignedSubjects = @"
                    UPDATE LecturerSubjects SET Status = 'Inactive'
                    WHERE SubjectID IN (SELECT SubjectID FROM Subjects WHERE CourseID = @CourseID)";
                await DatabaseManager.ExecuteNonQueryAsync(updateAssignedSubjects, parameters);

                // 5. Soft delete timetables
                string updateTimetables = @"
                    UPDATE Timetables SET Status = 'Inactive'
                    WHERE SubjectID IN (SELECT SubjectID FROM Subjects WHERE CourseID = @CourseID)";
                await DatabaseManager.ExecuteNonQueryAsync(updateTimetables, parameters);

                // 6. Soft delete exams
                string updateExams = @"
                    UPDATE Exams SET Status = 'Inactive'
                    WHERE SubjectID IN (SELECT SubjectID FROM Subjects WHERE CourseID = @CourseID)";
                await DatabaseManager.ExecuteNonQueryAsync(updateExams, parameters);

                // 7. Soft delete marks
                string updateMarks = @"
                    UPDATE Marks SET Status = 'Inactive'
                    WHERE ExamID IN (SELECT ExamID FROM Exams WHERE SubjectID IN (
                SELECT SubjectID FROM Subjects WHERE CourseID = @CourseID))";
                await DatabaseManager.ExecuteNonQueryAsync(updateMarks, parameters);

                // 8. Soft delete attendance
                string updateAttendance = @"
                    UPDATE Attendance SET Status = 'Inactive'
                    WHERE TimetableID IN (
                        SELECT TimetableID FROM Timetables WHERE SubjectID IN (
                            SELECT SubjectID FROM Subjects WHERE CourseID = @CourseID))";
                await DatabaseManager.ExecuteNonQueryAsync(updateAttendance, parameters);

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
                string query = "SELECT * FROM Courses WHERE Status = 'Active' AND CourseID = @CourseID";
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
                         LEFT JOIN Departments d ON c.DepartmentID = d.DepartmentID
                         WHERE c.Status = 'Active'";

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
                         WHERE c.Status = 'Active' AND c.CourseName LIKE @CourseName";

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
                string query = @"
                    SELECT c.*, d.DepartmentName
                    FROM Courses c
                    JOIN Departments d ON c.DepartmentID = d.DepartmentID
                    WHERE c.DepartmentID = @DepartmentID AND Status = 'Active'";
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
