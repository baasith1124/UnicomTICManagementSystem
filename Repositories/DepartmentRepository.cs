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
                // 1. Soft delete the department
                string updateDepartmentQuery = "UPDATE Departments SET Status = 'Inactive' WHERE DepartmentID = @DepartmentID";
                var parameters = new Dictionary<string, object> { { "@DepartmentID", departmentID } };
                await DatabaseManager.ExecuteNonQueryAsync(updateDepartmentQuery, parameters);

                // 2. Soft delete related courses
                string updateCoursesQuery = "UPDATE Courses SET Status = 'Inactive' WHERE DepartmentID = @DepartmentID";
                await DatabaseManager.ExecuteNonQueryAsync(updateCoursesQuery, parameters);

                // 3. Get CourseIDs for next steps
                string getCourseIDsQuery = "SELECT CourseID FROM Courses WHERE DepartmentID = @DepartmentID";
                List<int> courseIDs = new List<int>();
                using (var reader = await DatabaseManager.ExecuteReaderAsync(getCourseIDsQuery, parameters))
                {
                    while (await reader.ReadAsync())
                        courseIDs.Add(reader.GetInt32(0));
                }

                foreach (int courseID in courseIDs)
                {
                    // 4. Soft delete Subjects
                    string updateSubjectsQuery = "UPDATE Subjects SET Status = 'Inactive' WHERE CourseID = @CourseID";
                    await DatabaseManager.ExecuteNonQueryAsync(updateSubjectsQuery, new Dictionary<string, object> { { "@CourseID", courseID } });

                    // 5. Soft delete Students
                    string updateStudentsQuery = "UPDATE Students SET Status = 'Inactive' WHERE CourseID = @CourseID";
                    await DatabaseManager.ExecuteNonQueryAsync(updateStudentsQuery, new Dictionary<string, object> { { "@CourseID", courseID } });

                    // 5a. Soft delete Users linked to those Students
                    string getStudentUsersQuery = @"
                        SELECT UserID FROM Students WHERE CourseID = @CourseID AND Status = 'Inactive'";
                    using (var userReader = await DatabaseManager.ExecuteReaderAsync(getStudentUsersQuery, new Dictionary<string, object> { { "@CourseID", courseID } }))
                    {
                        while (await userReader.ReadAsync())
                        {
                            int userID = userReader.GetInt32(0);
                            string updateUserQuery = "UPDATE Users SET Status = 'Inactive' WHERE UserID = @UserID";
                            await DatabaseManager.ExecuteNonQueryAsync(updateUserQuery, new Dictionary<string, object> { { "@UserID", userID } });
                        }
                    }

                }

                // 6. Soft delete related Positions
                string updatePositionsQuery = "UPDATE Positions SET Status = 'Inactive' WHERE DepartmentID = @DepartmentID";
                await DatabaseManager.ExecuteNonQueryAsync(updatePositionsQuery, parameters);

                // 7. Soft delete Staff and their Users
                string getStaffQuery = "SELECT UserID FROM Staff WHERE DepartmentID = @DepartmentID";
                using (var reader = await DatabaseManager.ExecuteReaderAsync(getStaffQuery, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        int userID = reader.GetInt32(0);
                        string updateUserQuery = "UPDATE Users SET Status = 'Inactive' WHERE UserID = @UserID";
                        await DatabaseManager.ExecuteNonQueryAsync(updateUserQuery, new Dictionary<string, object> { { "@UserID", userID } });
                    }
                }
                string updateStaffQuery = "UPDATE Staff SET Status = 'Inactive' WHERE DepartmentID = @DepartmentID";
                await DatabaseManager.ExecuteNonQueryAsync(updateStaffQuery, parameters);

                // 7a. Soft delete Lecturers and their Users
                string getLecturersQuery = @"
                    SELECT UserID FROM Lecturers 
                    WHERE DepartmentID = @DepartmentID AND Status = 'Active'";
                using (var reader = await DatabaseManager.ExecuteReaderAsync(getLecturersQuery, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        int userID = reader.GetInt32(0);
                        string updateUserQuery = "UPDATE Users SET Status = 'Inactive' WHERE UserID = @UserID";
                        await DatabaseManager.ExecuteNonQueryAsync(updateUserQuery, new Dictionary<string, object> { { "@UserID", userID } });
                    }
                }
                string updateLecturersQuery = "UPDATE Lecturers SET Status = 'Inactive' WHERE DepartmentID = @DepartmentID";
                await DatabaseManager.ExecuteNonQueryAsync(updateLecturersQuery, parameters);

                // 8. Soft delete Lecturer AssignedSubjects
                string updateAssignedSubjects = @"
                    UPDATE LecturerSubjects SET Status = 'Inactive'
                    WHERE SubjectID IN (SELECT SubjectID FROM Subjects WHERE CourseID IN (
                        SELECT CourseID FROM Courses WHERE DepartmentID = @DepartmentID))";
                await DatabaseManager.ExecuteNonQueryAsync(updateAssignedSubjects, parameters);

                // 9. Soft delete Timetables
                string updateTimetables = @"
                    UPDATE Timetables SET Status = 'Inactive'
                    WHERE SubjectID IN (SELECT SubjectID FROM Subjects WHERE CourseID IN (
                        SELECT CourseID FROM Courses WHERE DepartmentID = @DepartmentID))";
                await DatabaseManager.ExecuteNonQueryAsync(updateTimetables, parameters);

                // 10. Soft delete Exams
                string updateExams = @"
                    UPDATE Exams SET Status = 'Inactive'
                    WHERE SubjectID IN (SELECT SubjectID FROM Subjects WHERE CourseID IN (
                        SELECT CourseID FROM Courses WHERE DepartmentID = @DepartmentID))";
                await DatabaseManager.ExecuteNonQueryAsync(updateExams, parameters);

                // 11. Soft delete Marks
                string updateMarks = @"
                    UPDATE Marks SET Status = 'Inactive'
                    WHERE ExamID IN (SELECT ExamID FROM Exams WHERE SubjectID IN (
                        SELECT SubjectID FROM Subjects WHERE CourseID IN (
                            SELECT CourseID FROM Courses WHERE DepartmentID = @DepartmentID)))";
                await DatabaseManager.ExecuteNonQueryAsync(updateMarks, parameters);

                // 12. Soft delete Attendance
                string updateAttendance = @"
                    UPDATE Attendance SET Status = 'Inactive'
                    WHERE TimetableID IN (
                        SELECT TimetableID FROM Timetables WHERE SubjectID IN (
                            SELECT SubjectID FROM Subjects WHERE CourseID IN (
                                SELECT CourseID FROM Courses WHERE DepartmentID = @DepartmentID)))";
                await DatabaseManager.ExecuteNonQueryAsync(updateAttendance, parameters);
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
                string query = "SELECT DepartmentID, DepartmentName FROM Departments WHERE Status = 'Active'";
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
                string query = "SELECT DepartmentID, DepartmentName FROM Departments WHERE Status = 'Active' AND DepartmentID = @DepartmentID";
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
