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
        public void AddAttendance(Attendance attendance)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        INSERT INTO Attendance (TimetableID, StudentID, Status, MarkedBy, MarkedDate)
                        VALUES (@TimetableID, @StudentID, @Status, @MarkedBy, @MarkedDate)";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimetableID", attendance.TimetableID);
                        cmd.Parameters.AddWithValue("@StudentID", attendance.StudentID);
                        cmd.Parameters.AddWithValue("@Status", attendance.Status);
                        cmd.Parameters.AddWithValue("@MarkedBy", attendance.MarkedBy);
                        cmd.Parameters.AddWithValue("@MarkedDate", attendance.MarkedDate.ToString("yyyy-MM-dd"));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.AddAttendance");
            }
        }

        public void UpdateAttendance(Attendance attendance)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        UPDATE Attendance 
                        SET Status = @Status 
                        WHERE AttendanceID = @AttendanceID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Status", attendance.Status);
                        cmd.Parameters.AddWithValue("@AttendanceID", attendance.AttendanceID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.UpdateAttendance");
            }
        }

        public void DeleteAttendance(int attendanceID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = "DELETE FROM Attendance WHERE AttendanceID = @AttendanceID";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.DeleteAttendance");
            }
        }

        public Attendance GetAttendanceByID(int attendanceID)
        {
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT AttendanceID, TimetableID, StudentID, Status, MarkedBy, MarkedDate 
                        FROM Attendance
                        WHERE AttendanceID = @AttendanceID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.GetAttendanceByID");
            }
            return null;
        }

        public List<Attendance> GetAttendanceByTimetable(int timetableID)
        {
            var list = new List<Attendance>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT a.AttendanceID, a.TimetableID, a.StudentID, s.Name AS StudentName, 
                               a.Status, a.MarkedBy, a.MarkedDate
                        FROM Attendance a
                        INNER JOIN Students s ON a.StudentID = s.StudentID
                        WHERE a.TimetableID = @TimetableID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TimetableID", timetableID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.GetAttendanceByTimetable");
            }
            return list;
        }

        public List<Attendance> GetFullAttendance()
        {
            var attendanceList = new List<Attendance>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT a.AttendanceID, a.TimetableID, a.StudentID, s.Name AS StudentName,
                               sub.SubjectName, a.Status, a.MarkedBy, a.MarkedDate
                        FROM Attendance a
                        INNER JOIN Students s ON a.StudentID = s.StudentID
                        INNER JOIN Timetable t ON a.TimetableID = t.TimetableID
                        INNER JOIN Subjects sub ON t.SubjectID = sub.SubjectID";

                    using (var cmd = new SQLiteCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
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
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.GetFullAttendance");
            }
            return attendanceList;
        }

        public List<Attendance> SearchAttendance(int subjectID, string date)
        {
            var attendanceList = new List<Attendance>();
            try
            {
                using (var conn = DatabaseManager.GetConnection())
                {
                    string query = @"
                        SELECT a.AttendanceID, a.TimetableID, a.StudentID, s.Name AS StudentName,
                               sub.SubjectName, a.Status, a.MarkedBy, a.MarkedDate
                        FROM Attendance a
                        INNER JOIN Students s ON a.StudentID = s.StudentID
                        INNER JOIN Timetable t ON a.TimetableID = t.TimetableID
                        INNER JOIN Subjects sub ON t.SubjectID = sub.SubjectID
                        WHERE t.SubjectID = @SubjectID AND a.MarkedDate = @Date";

                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SubjectID", subjectID);
                        cmd.Parameters.AddWithValue("@Date", date);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
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
                }
            }
            catch (Exception ex)
            {
                ErrorLogger.Log(ex, "AttendanceRepository.SearchAttendance");
            }
            return attendanceList;
        }



    }
}
