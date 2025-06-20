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
    public class AttendanceRepository : IAttendanceRepository
    {
        public async Task AddAttendanceAsync(Attendance attendance)
        {
            try
            {
                string query = @"
            INSERT INTO Attendance (TimetableID, StudentID, Status, MarkedBy, MarkedDate)
            VALUES (@TimetableID, @StudentID, @Status, @MarkedBy, @MarkedDate)";

                var parameters = new Dictionary<string, object>
                {
                    {"@TimetableID", attendance.TimetableID},
                    {"@StudentID", attendance.StudentID},
                    {"@Status", attendance.Status},
                    {"@MarkedBy", attendance.MarkedBy},
                    {"@MarkedDate", attendance.MarkedDate.ToString("yyyy-MM-dd")}
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.AddAttendanceAsync");
            }
        }


        public async Task UpdateAttendanceAsync(Attendance attendance)
        {
            try
            {
                string query = @"
            UPDATE Attendance 
            SET Status = @Status 
            WHERE AttendanceID = @AttendanceID";

                var parameters = new Dictionary<string, object>
                {
                    { "@Status", attendance.Status },
                    { "@AttendanceID", attendance.AttendanceID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.UpdateAttendanceAsync");
            }
        }


        public async Task DeleteAttendanceAsync(int attendanceID)
        {
            try
            {
                string query = "DELETE FROM Attendance WHERE AttendanceID = @AttendanceID";

                var parameters = new Dictionary<string, object>
                {
                    { "@AttendanceID", attendanceID }
                };

                await DatabaseManager.ExecuteNonQueryAsync(query, parameters);
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.DeleteAttendanceAsync");
            }
        }


        public async Task<Attendance> GetAttendanceByIDAsync(int attendanceID)
        {
            try
            {
                string query = @"
                    SELECT AttendanceID, TimetableID, StudentID, Status, MarkedBy, MarkedDate 
                    FROM Attendance
                    WHERE AttendanceID = @AttendanceID";

                var parameters = new Dictionary<string, object>
                {
                    { "@AttendanceID", attendanceID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    if (await reader.ReadAsync())
                    {
                        return new Attendance
                        {
                            AttendanceID = Convert.ToInt32(reader["AttendanceID"]),
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            Status = reader["Status"].ToString(),
                            MarkedBy = Convert.ToInt32(reader["MarkedBy"]),
                            MarkedDate = DateTime.Parse(reader["MarkedDate"].ToString())
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.GetAttendanceByIDAsync");
            }

            return null;
        }


        public async Task<List<Attendance>> GetAttendanceByTimetableAsync(int timetableID)
        {
            var list = new List<Attendance>();
            try
            {
                string query = @"
                    SELECT a.AttendanceID, a.TimetableID, a.StudentID, s.Name AS StudentName, 
                           a.Status, a.MarkedBy, a.MarkedDate
                    FROM Attendance a
                    INNER JOIN Students s ON a.StudentID = s.StudentID
                    WHERE a.TimetableID = @TimetableID";

                var parameters = new Dictionary<string, object>
                {
                    { "@TimetableID", timetableID }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        list.Add(new Attendance
                        {
                            AttendanceID = Convert.ToInt32(reader["AttendanceID"]),
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            StudentName = reader["StudentName"].ToString(),
                            Status = reader["Status"].ToString(),
                            MarkedBy = Convert.ToInt32(reader["MarkedBy"]),
                            MarkedDate = DateTime.Parse(reader["MarkedDate"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.GetAttendanceByTimetableAsync");
            }

            return list;
        }


        public async Task<List<Attendance>> GetFullAttendanceAsync()
        {
            var attendanceList = new List<Attendance>();
            try
            {
                string query = @"
                    SELECT a.AttendanceID, a.TimetableID, a.StudentID, s.Name AS StudentName,
                           sub.SubjectName, a.Status, a.MarkedBy, a.MarkedDate
                    FROM Attendance a
                    INNER JOIN Students s ON a.StudentID = s.StudentID
                    INNER JOIN Timetable t ON a.TimetableID = t.TimetableID
                    INNER JOIN Subjects sub ON t.SubjectID = sub.SubjectID";

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, null))
                {
                    while (await reader.ReadAsync())
                    {
                        attendanceList.Add(new Attendance
                        {
                            AttendanceID = Convert.ToInt32(reader["AttendanceID"]),
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            StudentName = reader["StudentName"].ToString(),
                            SubjectName = reader["SubjectName"].ToString(),
                            Status = reader["Status"].ToString(),
                            MarkedBy = Convert.ToInt32(reader["MarkedBy"]),
                            MarkedDate = DateTime.Parse(reader["MarkedDate"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.GetFullAttendanceAsync");
            }

            return attendanceList;
        }


        public async Task<List<Attendance>> SearchAttendanceAsync(int subjectID, string date)
        {
            var attendanceList = new List<Attendance>();
            try
            {
                string query = @"
                    SELECT a.AttendanceID, a.TimetableID, a.StudentID, s.Name AS StudentName,
                           sub.SubjectName, a.Status, a.MarkedBy, a.MarkedDate
                    FROM Attendance a
                    INNER JOIN Students s ON a.StudentID = s.StudentID
                    INNER JOIN Timetable t ON a.TimetableID = t.TimetableID
                    INNER JOIN Subjects sub ON t.SubjectID = sub.SubjectID
                    WHERE t.SubjectID = @SubjectID AND a.MarkedDate = @Date";

                var parameters = new Dictionary<string, object>
                {
                    { "@SubjectID", subjectID },
                    { "@Date", date }
                };

                using (var reader = await DatabaseManager.ExecuteReaderAsync(query, parameters))
                {
                    while (await reader.ReadAsync())
                    {
                        attendanceList.Add(new Attendance
                        {
                            AttendanceID = Convert.ToInt32(reader["AttendanceID"]),
                            TimetableID = Convert.ToInt32(reader["TimetableID"]),
                            StudentID = Convert.ToInt32(reader["StudentID"]),
                            StudentName = reader["StudentName"].ToString(),
                            SubjectName = reader["SubjectName"].ToString(),
                            Status = reader["Status"].ToString(),
                            MarkedBy = Convert.ToInt32(reader["MarkedBy"]),
                            MarkedDate = DateTime.Parse(reader["MarkedDate"].ToString())
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.SearchAttendanceAsync");
            }

            return attendanceList;
        }




    }
}
